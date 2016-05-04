﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Akka.Interfaced CodeGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Akka.Interfaced;
using Akka.Actor;

#region Akka.Interfaced.Tests.IBasic

namespace Akka.Interfaced.Tests
{
    [PayloadTableForInterfacedActor(typeof(IBasic))]
    public static class IBasic_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(Call_Invoke), null },
                { typeof(CallWithParameter_Invoke), null },
                { typeof(CallWithParameterAndReturn_Invoke), typeof(CallWithParameterAndReturn_Return) },
                { typeof(CallWithReturn_Invoke), typeof(CallWithReturn_Return) },
                { typeof(ThrowException_Invoke), typeof(ThrowException_Return) },
            };
        }

        public class Call_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Type GetInterfaceType() { return typeof(IBasic); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((IBasic)__target).Call();
                return null;
            }
        }

        public class CallWithParameter_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 value;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((IBasic)__target).CallWithParameter(value);
                return null;
            }
        }

        public class CallWithParameterAndReturn_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 value;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IBasic)__target).CallWithParameterAndReturn(value);
                return (IValueGetable)(new CallWithParameterAndReturn_Return { v = __v });
            }
        }

        public class CallWithParameterAndReturn_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public object Value { get { return v; } }
        }

        public class CallWithReturn_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Type GetInterfaceType() { return typeof(IBasic); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IBasic)__target).CallWithReturn();
                return (IValueGetable)(new CallWithReturn_Return { v = __v });
            }
        }

        public class CallWithReturn_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public object Value { get { return v; } }
        }

        public class ThrowException_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Boolean throwException;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IBasic)__target).ThrowException(throwException);
                return (IValueGetable)(new ThrowException_Return { v = __v });
            }
        }

        public class ThrowException_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IBasic); }
            public object Value { get { return v; } }
        }
    }

    public interface IBasic_NoReply
    {
        void Call();
        void CallWithParameter(System.Int32 value);
        void CallWithParameterAndReturn(System.Int32 value);
        void CallWithReturn();
        void ThrowException(System.Boolean throwException);
    }

    public class BasicRef : InterfacedActorRef, IBasic, IBasic_NoReply
    {
        public BasicRef(IActorRef actor) : base(actor)
        {
        }

        public BasicRef(IActorRef actor, IRequestWaiter requestWaiter, TimeSpan? timeout) : base(actor, requestWaiter, timeout)
        {
        }

        public IBasic_NoReply WithNoReply()
        {
            return this;
        }

        public BasicRef WithRequestWaiter(IRequestWaiter requestWaiter)
        {
            return new BasicRef(Actor, requestWaiter, Timeout);
        }

        public BasicRef WithTimeout(TimeSpan? timeout)
        {
            return new BasicRef(Actor, RequestWaiter, timeout);
        }

        public Task Call()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.Call_Invoke {  }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task CallWithParameter(System.Int32 value)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithParameter_Invoke { value = value }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task<System.Int32> CallWithParameterAndReturn(System.Int32 value)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithParameterAndReturn_Invoke { value = value }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        public Task<System.Int32> CallWithReturn()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithReturn_Invoke {  }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        public Task<System.Int32> ThrowException(System.Boolean throwException)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.ThrowException_Invoke { throwException = throwException }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        void IBasic_NoReply.Call()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.Call_Invoke {  }
            };
            SendRequest(requestMessage);
        }

        void IBasic_NoReply.CallWithParameter(System.Int32 value)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithParameter_Invoke { value = value }
            };
            SendRequest(requestMessage);
        }

        void IBasic_NoReply.CallWithParameterAndReturn(System.Int32 value)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithParameterAndReturn_Invoke { value = value }
            };
            SendRequest(requestMessage);
        }

        void IBasic_NoReply.CallWithReturn()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.CallWithReturn_Invoke {  }
            };
            SendRequest(requestMessage);
        }

        void IBasic_NoReply.ThrowException(System.Boolean throwException)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.ThrowException_Invoke { throwException = throwException }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.Tests.IDummy

namespace Akka.Interfaced.Tests
{
    [PayloadTableForInterfacedActor(typeof(IDummy))]
    public static class IDummy_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(Call_Invoke), typeof(Call_Return) },
            };
        }

        public class Call_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Object param;
            public Type GetInterfaceType() { return typeof(IDummy); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IDummy)__target).Call(param);
                return (IValueGetable)(new Call_Return { v = __v });
            }
        }

        public class Call_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Object v;
            public Type GetInterfaceType() { return typeof(IDummy); }
            public object Value { get { return v; } }
        }
    }

    public interface IDummy_NoReply
    {
        void Call(System.Object param);
    }

    public class DummyRef : InterfacedActorRef, IDummy, IDummy_NoReply
    {
        public DummyRef(IActorRef actor) : base(actor)
        {
        }

        public DummyRef(IActorRef actor, IRequestWaiter requestWaiter, TimeSpan? timeout) : base(actor, requestWaiter, timeout)
        {
        }

        public IDummy_NoReply WithNoReply()
        {
            return this;
        }

        public DummyRef WithRequestWaiter(IRequestWaiter requestWaiter)
        {
            return new DummyRef(Actor, requestWaiter, Timeout);
        }

        public DummyRef WithTimeout(TimeSpan? timeout)
        {
            return new DummyRef(Actor, RequestWaiter, timeout);
        }

        public Task<System.Object> Call(System.Object param)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IDummy_PayloadTable.Call_Invoke { param = param }
            };
            return SendRequestAndReceive<System.Object>(requestMessage);
        }

        void IDummy_NoReply.Call(System.Object param)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IDummy_PayloadTable.Call_Invoke { param = param }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.Tests.IOverloaded

namespace Akka.Interfaced.Tests
{
    [PayloadTableForInterfacedActor(typeof(IOverloaded))]
    public static class IOverloaded_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(Min_Invoke), typeof(Min_Return) },
                { typeof(Min_2_Invoke), typeof(Min_2_Return) },
                { typeof(Min_3_Invoke), typeof(Min_3_Return) },
            };
        }

        public class Min_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 a;
            public System.Int32 b;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IOverloaded)__target).Min(a, b);
                return (IValueGetable)(new Min_Return { v = __v });
            }
        }

        public class Min_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public object Value { get { return v; } }
        }

        public class Min_2_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 a;
            public System.Int32 b;
            public System.Int32 c;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IOverloaded)__target).Min(a, b, c);
                return (IValueGetable)(new Min_2_Return { v = __v });
            }
        }

        public class Min_2_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public object Value { get { return v; } }
        }

        public class Min_3_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32[] nums;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                var __v = await ((IOverloaded)__target).Min(nums);
                return (IValueGetable)(new Min_3_Return { v = __v });
            }
        }

        public class Min_3_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;
            public Type GetInterfaceType() { return typeof(IOverloaded); }
            public object Value { get { return v; } }
        }
    }

    public interface IOverloaded_NoReply
    {
        void Min(System.Int32 a, System.Int32 b);
        void Min(System.Int32 a, System.Int32 b, System.Int32 c);
        void Min(params System.Int32[] nums);
    }

    public class OverloadedRef : InterfacedActorRef, IOverloaded, IOverloaded_NoReply
    {
        public OverloadedRef(IActorRef actor) : base(actor)
        {
        }

        public OverloadedRef(IActorRef actor, IRequestWaiter requestWaiter, TimeSpan? timeout) : base(actor, requestWaiter, timeout)
        {
        }

        public IOverloaded_NoReply WithNoReply()
        {
            return this;
        }

        public OverloadedRef WithRequestWaiter(IRequestWaiter requestWaiter)
        {
            return new OverloadedRef(Actor, requestWaiter, Timeout);
        }

        public OverloadedRef WithTimeout(TimeSpan? timeout)
        {
            return new OverloadedRef(Actor, RequestWaiter, timeout);
        }

        public Task<System.Int32> Min(System.Int32 a, System.Int32 b)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_Invoke { a = a, b = b }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        public Task<System.Int32> Min(System.Int32 a, System.Int32 b, System.Int32 c)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_2_Invoke { a = a, b = b, c = c }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        public Task<System.Int32> Min(params System.Int32[] nums)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_3_Invoke { nums = nums }
            };
            return SendRequestAndReceive<System.Int32>(requestMessage);
        }

        void IOverloaded_NoReply.Min(System.Int32 a, System.Int32 b)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_Invoke { a = a, b = b }
            };
            SendRequest(requestMessage);
        }

        void IOverloaded_NoReply.Min(System.Int32 a, System.Int32 b, System.Int32 c)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_2_Invoke { a = a, b = b, c = c }
            };
            SendRequest(requestMessage);
        }

        void IOverloaded_NoReply.Min(params System.Int32[] nums)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IOverloaded_PayloadTable.Min_3_Invoke { nums = nums }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.Tests.ISubject

namespace Akka.Interfaced.Tests
{
    [PayloadTableForInterfacedActor(typeof(ISubject))]
    public static class ISubject_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(MakeEvent_Invoke), null },
                { typeof(Subscribe_Invoke), null },
                { typeof(Unsubscribe_Invoke), null },
            };
        }

        public class MakeEvent_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.String eventName;
            public Type GetInterfaceType() { return typeof(ISubject); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((ISubject)__target).MakeEvent(eventName);
                return null;
            }
        }

        public class Subscribe_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Akka.Interfaced.Tests.SubjectObserver observer;
            public Type GetInterfaceType() { return typeof(ISubject); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((ISubject)__target).Subscribe(observer);
                return null;
            }
        }

        public class Unsubscribe_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Akka.Interfaced.Tests.SubjectObserver observer;
            public Type GetInterfaceType() { return typeof(ISubject); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((ISubject)__target).Unsubscribe(observer);
                return null;
            }
        }
    }

    public interface ISubject_NoReply
    {
        void MakeEvent(System.String eventName);
        void Subscribe(Akka.Interfaced.Tests.ISubjectObserver observer);
        void Unsubscribe(Akka.Interfaced.Tests.ISubjectObserver observer);
    }

    public class SubjectRef : InterfacedActorRef, ISubject, ISubject_NoReply
    {
        public SubjectRef(IActorRef actor) : base(actor)
        {
        }

        public SubjectRef(IActorRef actor, IRequestWaiter requestWaiter, TimeSpan? timeout) : base(actor, requestWaiter, timeout)
        {
        }

        public ISubject_NoReply WithNoReply()
        {
            return this;
        }

        public SubjectRef WithRequestWaiter(IRequestWaiter requestWaiter)
        {
            return new SubjectRef(Actor, requestWaiter, Timeout);
        }

        public SubjectRef WithTimeout(TimeSpan? timeout)
        {
            return new SubjectRef(Actor, RequestWaiter, timeout);
        }

        public Task MakeEvent(System.String eventName)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.MakeEvent_Invoke { eventName = eventName }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task Subscribe(Akka.Interfaced.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Subscribe_Invoke { observer = (Akka.Interfaced.Tests.SubjectObserver)observer }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task Unsubscribe(Akka.Interfaced.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Unsubscribe_Invoke { observer = (Akka.Interfaced.Tests.SubjectObserver)observer }
            };
            return SendRequestAndWait(requestMessage);
        }

        void ISubject_NoReply.MakeEvent(System.String eventName)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.MakeEvent_Invoke { eventName = eventName }
            };
            SendRequest(requestMessage);
        }

        void ISubject_NoReply.Subscribe(Akka.Interfaced.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Subscribe_Invoke { observer = (Akka.Interfaced.Tests.SubjectObserver)observer }
            };
            SendRequest(requestMessage);
        }

        void ISubject_NoReply.Unsubscribe(Akka.Interfaced.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Unsubscribe_Invoke { observer = (Akka.Interfaced.Tests.SubjectObserver)observer }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.Tests.IWorker

namespace Akka.Interfaced.Tests
{
    [PayloadTableForInterfacedActor(typeof(IWorker))]
    public static class IWorker_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(Atomic_Invoke), null },
                { typeof(Reentrant_Invoke), null },
            };
        }

        public class Atomic_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 id;
            public Type GetInterfaceType() { return typeof(IWorker); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((IWorker)__target).Atomic(id);
                return null;
            }
        }

        public class Reentrant_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 id;
            public Type GetInterfaceType() { return typeof(IWorker); }
            public async Task<IValueGetable> InvokeAsync(object __target)
            {
                await ((IWorker)__target).Reentrant(id);
                return null;
            }
        }
    }

    public interface IWorker_NoReply
    {
        void Atomic(System.Int32 id);
        void Reentrant(System.Int32 id);
    }

    public class WorkerRef : InterfacedActorRef, IWorker, IWorker_NoReply
    {
        public WorkerRef(IActorRef actor) : base(actor)
        {
        }

        public WorkerRef(IActorRef actor, IRequestWaiter requestWaiter, TimeSpan? timeout) : base(actor, requestWaiter, timeout)
        {
        }

        public IWorker_NoReply WithNoReply()
        {
            return this;
        }

        public WorkerRef WithRequestWaiter(IRequestWaiter requestWaiter)
        {
            return new WorkerRef(Actor, requestWaiter, Timeout);
        }

        public WorkerRef WithTimeout(TimeSpan? timeout)
        {
            return new WorkerRef(Actor, RequestWaiter, timeout);
        }

        public Task Atomic(System.Int32 id)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IWorker_PayloadTable.Atomic_Invoke { id = id }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task Reentrant(System.Int32 id)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IWorker_PayloadTable.Reentrant_Invoke { id = id }
            };
            return SendRequestAndWait(requestMessage);
        }

        void IWorker_NoReply.Atomic(System.Int32 id)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IWorker_PayloadTable.Atomic_Invoke { id = id }
            };
            SendRequest(requestMessage);
        }

        void IWorker_NoReply.Reentrant(System.Int32 id)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IWorker_PayloadTable.Reentrant_Invoke { id = id }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.Tests.ISubjectObserver

namespace Akka.Interfaced.Tests
{
    public static class ISubjectObserver_PayloadTable
    {
        public class Event_Invoke : IInvokable
        {
            public System.String eventName;
            public void Invoke(object __target)
            {
                ((ISubjectObserver)__target).Event(eventName);
            }
        }
    }

    public class SubjectObserver : InterfacedObserver, ISubjectObserver
    {
        public SubjectObserver(IActorRef target, int observerId)
            : base(new ActorNotificationChannel(target), observerId)
        {
        }

        public SubjectObserver(INotificationChannel channel, int observerId)
            : base(channel, observerId)
        {
        }

        public void Event(System.String eventName)
        {
            var payload = new ISubjectObserver_PayloadTable.Event_Invoke { eventName = eventName };
            Notify(payload);
        }
    }
}

#endregion
