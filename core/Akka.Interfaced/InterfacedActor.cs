﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;

namespace Akka.Interfaced
{
    public abstract class InterfacedActor : UntypedActor, IWithUnboundedStash, IRequestWaiter, IFilterPerInstanceProvider
    {
        public IStash Stash { get; set; }

        private readonly InterfacedActorHandler _handler;
        private CancellationTokenSource _cancellationTokenSource;
        private int _activeReentrantCount;
        private HashSet<MessageHandleContext> _activeReentrantAsyncRequestSet;
        private MessageHandleContext _currentAtomicContext;
        private InterfacedActorRequestWaiter _requestWaiter;
        private InterfacedActorPerInstanceFilterList _perInstanceFilterList;

        protected new IActorRef Sender
        {
            get
            {
                var context = ActorSynchronizationContext.GetCurrentContext();
                return context != null ? context.Sender : base.Sender;
            }
        }

        // Return a token which will be cancelled when an actor stops or restarts.
        protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public InterfacedActor()
        {
            _handler = InterfacedActorHandlerTable.Get(GetType());
        }

        // Atomic async OnStart event (it will be called after PreStart, PostRestart)
        protected virtual Task OnStart(bool restarted)
        {
            return Task.FromResult(true);
        }

        // Atomic async OnGracefulStop event (it will be called when receives InterfacedPoisonPill)
        // After finishing this call, actor will be stopped.
        protected virtual Task OnGracefulStop()
        {
            return Task.FromResult(true);
        }

        public override void AroundPreStart()
        {
            InitializeActorState();
            base.AroundPreStart();
            InvokeOnStart(false);
        }

        public override void AroundPostRestart(Exception cause, object message)
        {
            InitializeActorState();
            base.AroundPostRestart(cause, message);
            InvokeOnStart(true);
        }

        private void InitializeActorState()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _activeReentrantCount = 0;
            _activeReentrantAsyncRequestSet = null;
            _currentAtomicContext = null;
            _requestWaiter = null;

            if (_handler.PerInstanceFilterCreators.Count > 0)
                _perInstanceFilterList = new InterfacedActorPerInstanceFilterList(this, _handler.PerInstanceFilterCreators);
        }

        private void InvokeOnStart(bool restarted)
        {
            var context = new MessageHandleContext { Self = Self, Sender = base.Sender, CancellationToken = CancellationToken };
            BecomeStacked(OnReceiveInAtomicTask);
            _currentAtomicContext = context;

            using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
            {
                OnStart(restarted).ContinueWith(
                    t => OnTaskCompleted(t.Exception, false),
                    TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        private void InvokeOnGracefulStop()
        {
            var context = new MessageHandleContext { Self = Self, CancellationToken = CancellationToken };
            BecomeStacked(OnReceiveInAtomicTask);
            _currentAtomicContext = context;

            using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
            {
                OnGracefulStop().ContinueWith(
                    t => OnTaskCompleted(t.Exception, false, stopOnCompleted: true),
                    TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        public override void AroundPreRestart(Exception cause, object message)
        {
            CancelAllTasks();
            base.AroundPreRestart(cause, message);
        }

        public override void AroundPostStop()
        {
            CancelAllTasks();
            base.AroundPostStop();
        }

        private void CancelAllTasks()
        {
            _cancellationTokenSource.Cancel();

            // Send responses to requesters that waits for a reentrant async job

            if (_activeReentrantAsyncRequestSet != null)
            {
                foreach (var i in _activeReentrantAsyncRequestSet)
                {
                    i.Sender.Tell(new ResponseMessage
                    {
                        RequestId = i.RequestId,
                        Exception = new RequestHaltException()
                    });
                }
            }

            if (_currentAtomicContext != null && _currentAtomicContext.RequestId != 0)
            {
                _currentAtomicContext.Sender.Tell(new ResponseMessage
                {
                    RequestId = _currentAtomicContext.RequestId,
                    Exception = new RequestHaltException()
                });
            }
        }

        protected override void OnReceive(object message)
        {
            var requestMessage = message as RequestMessage;
            if (requestMessage != null)
            {
                OnRequestMessage(requestMessage);
                return;
            }

            var continuationMessage = message as TaskContinuationMessage;
            if (continuationMessage != null)
            {
                OnTaskContinuationMessage(continuationMessage);
                return;
            }

            var responseMessage = message as ResponseMessage;
            if (responseMessage != null)
            {
                OnResponseMessage(responseMessage);
                return;
            }

            var notificationMessage = message as NotificationMessage;
            if (notificationMessage != null)
            {
                OnNotificationMessage(notificationMessage);
                return;
            }

            var taskRunMessage = message as TaskRunMessage;
            if (taskRunMessage != null)
            {
                OnTaskRunMessage(taskRunMessage);
                return;
            }

            var poisonPill = message as InterfacedPoisonPill;
            if (poisonPill != null)
            {
                OnInterfacedPoisonPill();
                return;
            }

            var messageHandler = _handler.MessageDispatcher.GetHandler(message.GetType());
            if (messageHandler != null)
            {
                HandleMessageByHandler(message, messageHandler);
                return;
            }

            OnReceiveUnhandled(message);
        }

        private void OnRequestMessage(RequestMessage request)
        {
            var sender = base.Sender;

            if (request.InvokePayload == null)
            {
                sender.Tell(new ResponseMessage
                {
                    RequestId = request.RequestId,
                    Exception = new RequestMessageException("Empty payload")
                });
                Context.System.EventStream.Publish(new Event.Warning(
                    Self.Path.ToString(), GetType(),
                    $"Receives a bad request without payload from {Sender}"));
                return;
            }

            var handlerItem = _handler.RequestDispatcher.GetHandler(request.InvokePayload.GetType());
            if (handlerItem == null)
            {
                sender.Tell(new ResponseMessage
                {
                    RequestId = request.RequestId,
                    Exception = new RequestHandlerNotFoundException()
                });
                Context.System.EventStream.Publish(new Event.Warning(
                    Self.Path.ToString(), GetType(),
                    $"Cannot find a handler for request {request.InvokePayload.GetType()} from {Sender}"));
                return;
            }

            if (handlerItem.Handler != null)
            {
                // sync handle

                handlerItem.Handler(this, request, (response, exception) =>
                {
                    if (request.RequestId != 0)
                        sender.Tell(response);

                    if (exception != null)
                        ((ActorCell)Context).InvokeFailure(exception);
                });
            }
            else
            {
                // async handle

                var context = new MessageHandleContext { Self = Self, Sender = base.Sender, CancellationToken = CancellationToken, RequestId = request.RequestId };
                if (handlerItem.IsReentrant)
                {
                    _activeReentrantCount += 1;
                    if (request.RequestId != 0)
                    {
                        if (_activeReentrantAsyncRequestSet == null)
                            _activeReentrantAsyncRequestSet = new HashSet<MessageHandleContext>();

                        _activeReentrantAsyncRequestSet.Add(context);
                    }
                }
                else
                {
                    BecomeStacked(OnReceiveInAtomicTask);
                    _currentAtomicContext = context;
                }

                using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
                {
                    var requestId = request.RequestId;
                    var isReentrant = handlerItem.IsReentrant;
                    handlerItem.AsyncHandler(this, request, (response, exception) =>
                    {
                        if (requestId != 0)
                        {
                            if (isReentrant)
                                _activeReentrantAsyncRequestSet.Remove(context);

                            sender.Tell(response);
                        }

                        OnTaskCompleted(exception, isReentrant);
                    });
                }
            }
        }

        private static void OnTaskContinuationMessage(TaskContinuationMessage continuation)
        {
            using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(continuation.Context)))
            {
                continuation.CallbackAction(continuation.CallbackState);
            }
        }

        private void OnResponseMessage(ResponseMessage response)
        {
            _requestWaiter?.OnResponseMessage(response, _currentAtomicContext);
        }

        private void OnNotificationMessage(NotificationMessage notification)
        {
            if (notification.ObserverId != 0)
            {
                Context.System.EventStream.Publish(new Event.Warning(
                    Self.Path.ToString(), GetType(),
                    $"Receives a bad notification with non-zero observerId from {Sender}"));
                return;
            }

            if (notification.InvokePayload == null)
            {
                Context.System.EventStream.Publish(new Event.Warning(
                    Self.Path.ToString(), GetType(),
                    $"Receives a bad notification with no payload from {Sender}"));
                return;
            }

            var handlerItem = _handler.NotificationDispatcher.GetHandler(notification.InvokePayload.GetType());
            if (handlerItem == null)
            {
                Context.System.EventStream.Publish(new Event.Warning(
                    Self.Path.ToString(), GetType(),
                    $"Cannot find a handler for notification {notification.InvokePayload.GetType()} from {Sender}"));
                return;
            }

            if (handlerItem.Handler != null)
            {
                // sync handle

                handlerItem.Handler(this, notification);
            }
            else
            {
                // async handle

                var context = new MessageHandleContext { Self = Self, Sender = base.Sender, CancellationToken = CancellationToken };
                if (handlerItem.IsReentrant)
                {
                    _activeReentrantCount += 1;
                }
                else
                {
                    BecomeStacked(OnReceiveInAtomicTask);
                    _currentAtomicContext = context;
                }

                using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
                {
                    handlerItem.AsyncHandler(this, notification)
                                .ContinueWith(t => OnTaskCompleted(t.Exception, handlerItem.IsReentrant),
                                                TaskContinuationOptions.ExecuteSynchronously);
                }
            }
        }

        private void OnTaskRunMessage(TaskRunMessage taskRunMessage)
        {
            var context = new MessageHandleContext { Self = Self, Sender = base.Sender, CancellationToken = CancellationToken };
            if (taskRunMessage.IsReentrant)
            {
                _activeReentrantCount += 1;
            }
            else
            {
                BecomeStacked(OnReceiveInAtomicTask);
                _currentAtomicContext = context;
            }

            using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
            {
                taskRunMessage.Function()
                              .ContinueWith(t => OnTaskCompleted(t.Exception, taskRunMessage.IsReentrant),
                                            TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        private void OnInterfacedPoisonPill()
        {
            if (_activeReentrantCount > 0)
            {
                BecomeStacked(OnReceiveInWaitingForReentrantsFinished);
            }
            else
            {
                InvokeOnGracefulStop();
            }
        }

        private void HandleMessageByHandler(object message, MessageHandlerItem handlerItem)
        {
            if (handlerItem.AsyncHandler != null)
            {
                var context = new MessageHandleContext { Self = Self, Sender = base.Sender, CancellationToken = CancellationToken };
                if (handlerItem.IsReentrant)
                {
                    _activeReentrantCount += 1;
                }
                else
                {
                    BecomeStacked(OnReceiveInAtomicTask);
                    _currentAtomicContext = context;
                }

                using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(context)))
                {
                    handlerItem.AsyncHandler(this, message)
                               .ContinueWith(t => OnTaskCompleted(t.Exception, handlerItem.IsReentrant),
                                             TaskContinuationOptions.ExecuteSynchronously);
                }
            }
            else
            {
                handlerItem.Handler(this, message);
            }
        }

        private void OnReceiveInAtomicTask(object message)
        {
            var continuationMessage = message as TaskContinuationMessage;
            if (continuationMessage != null && continuationMessage.Context == _currentAtomicContext)
            {
                using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(continuationMessage.Context)))
                {
                    continuationMessage.CallbackAction(continuationMessage.CallbackState);
                }
                return;
            }

            var response = message as ResponseMessage;
            if (response != null)
            {
                OnResponseMessage(response);
                return;
            }

            Stash.Stash();
        }

        private void OnReceiveInWaitingForReentrantsFinished(object message)
        {
            var continuationMessage = message as TaskContinuationMessage;
            if (continuationMessage != null)
            {
                using (new SynchronizationContextSwitcher(new ActorSynchronizationContext(continuationMessage.Context)))
                {
                    continuationMessage.CallbackAction(continuationMessage.CallbackState);
                }

                if (_activeReentrantCount == 0)
                {
                    UnbecomeStacked();
                    InvokeOnGracefulStop();
                }
                return;
            }

            var response = message as ResponseMessage;
            if (response != null)
            {
                OnResponseMessage(response);
                return;
            }

            Stash.Stash();
        }

        private void OnTaskCompleted(Exception exception, bool isReentrant, bool stopOnCompleted = false)
        {
            if (isReentrant)
            {
                _activeReentrantCount -= 1;
            }
            else
            {
                _currentAtomicContext = null;
                UnbecomeStacked();
                Stash.UnstashAll();
            }

            if (exception != null)
            {
                ((ActorCell)Context).InvokeFailure(exception);
            }
            else if (stopOnCompleted)
            {
                Context.Stop(Self);
            }
        }

        // from IRequestWaiter

        void IRequestWaiter.SendRequest(IActorRef target, RequestMessage requestMessage)
        {
            target.Tell(requestMessage);
        }

        Task IRequestWaiter.SendRequestAndWait(
            IActorRef target, RequestMessage request, TimeSpan? timeout)
        {
            if (_requestWaiter == null)
                _requestWaiter = new InterfacedActorRequestWaiter();

            return _requestWaiter.SendRequestAndWait(target, request, Self, timeout);
        }

        Task<TReturn> IRequestWaiter.SendRequestAndReceive<TReturn>(
            IActorRef target, RequestMessage request, TimeSpan? timeout)
        {
            if (_requestWaiter == null)
                _requestWaiter = new InterfacedActorRequestWaiter();

            return _requestWaiter.SendRequestAndReceive<TReturn>(target, request, Self, timeout);
        }

        // async support

        protected new void RunTask(Action action)
        {
            RunTask(() =>
            {
                action();
                return Task.FromResult(0);
            });
        }

        protected new void RunTask(Func<Task> function)
        {
            RunTask(function, false);
        }

        protected void RunTask(Func<Task> function, bool isReentrant)
        {
            Self.Tell(new TaskRunMessage { Function = function, IsReentrant = isReentrant });
        }

        // other messages

        protected virtual void OnReceiveUnhandled(object message)
        {
            Unhandled(message);
        }

        // observer support

        protected TObserver CreateObserver<TObserver>()
            where TObserver : IInterfacedObserver
        {
            var proxy = InterfacedObserver.Create(typeof(TObserver));
            proxy.Channel = new ActorNotificationChannel(Self);
            return (TObserver)(object)proxy;
        }

        // PerInstance Filter related

        IFilter IFilterPerInstanceProvider.GetFilter(int index)
        {
            return _perInstanceFilterList.Get(index);
        }
    }
}
