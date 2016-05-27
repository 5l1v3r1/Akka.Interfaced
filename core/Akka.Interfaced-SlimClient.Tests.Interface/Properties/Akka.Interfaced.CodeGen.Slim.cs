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

#region Akka.Interfaced.SlimClient.Tests.IBasic

namespace Akka.Interfaced.SlimClient.Tests
{
    [PayloadTable(typeof(IBasic), PayloadTableKind.Request)]
    public static class IBasic_PayloadTable
    {
        public static Type[,] GetPayloadTypes()
        {
            return new Type[,] {
                { typeof(Call_Invoke), null },
                { typeof(CallWithParameter_Invoke), null },
                { typeof(CallWithParameterAndReturn_Invoke), typeof(CallWithParameterAndReturn_Return) },
                { typeof(CallWithReturn_Invoke), typeof(CallWithReturn_Return) },
                { typeof(GetSelf_Invoke), typeof(GetSelf_Return) },
                { typeof(ThrowException_Invoke), typeof(ThrowException_Return) },
            };
        }

        public class Call_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class CallWithParameter_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 value;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class CallWithParameterAndReturn_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Int32 value;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class CallWithParameterAndReturn_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public object Value
            {
                get { return v; }
            }
        }

        public class CallWithReturn_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class CallWithReturn_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public object Value
            {
                get { return v; }
            }
        }

        public class GetSelf_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class GetSelf_Return
            : IInterfacedPayload, IValueGetable, IPayloadActorRefUpdatable
        {
            public Akka.Interfaced.SlimClient.Tests.IBasic v;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public object Value
            {
                get { return v; }
            }

            void IPayloadActorRefUpdatable.Update(Action<object> updater)
            {
                if (v != null)
                {
                    updater(v); 
                }
            }
        }

        public class ThrowException_Invoke
            : IInterfacedPayload, IAsyncInvokable
        {
            public System.Boolean throwException;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class ThrowException_Return
            : IInterfacedPayload, IValueGetable
        {
            public System.Int32 v;

            public Type GetInterfaceType()
            {
                return typeof(IBasic);
            }

            public object Value
            {
                get { return v; }
            }
        }
    }

    public interface IBasic_NoReply
    {
        void Call();
        void CallWithParameter(System.Int32 value);
        void CallWithParameterAndReturn(System.Int32 value);
        void CallWithReturn();
        void GetSelf();
        void ThrowException(System.Boolean throwException);
    }

    public class BasicRef : InterfacedActorRef, IBasic, IBasic_NoReply
    {
        public BasicRef() : base(null)
        {
        }

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

        public Task<Akka.Interfaced.SlimClient.Tests.IBasic> GetSelf()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.GetSelf_Invoke {  }
            };
            return SendRequestAndReceive<Akka.Interfaced.SlimClient.Tests.IBasic>(requestMessage);
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

        void IBasic_NoReply.GetSelf()
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new IBasic_PayloadTable.GetSelf_Invoke {  }
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
#region Akka.Interfaced.SlimClient.Tests.ISubject

namespace Akka.Interfaced.SlimClient.Tests
{
    [PayloadTable(typeof(ISubject), PayloadTableKind.Request)]
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

            public Type GetInterfaceType()
            {
                return typeof(ISubject);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }
        }

        public class Subscribe_Invoke
            : IInterfacedPayload, IAsyncInvokable, IPayloadObserverUpdatable
        {
            public Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer;

            public Type GetInterfaceType()
            {
                return typeof(ISubject);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }

            void IPayloadObserverUpdatable.Update(Action<IInterfacedObserver> updater)
            {
                if (observer != null)
                {
                    updater(observer);
                }
            }
        }

        public class Unsubscribe_Invoke
            : IInterfacedPayload, IAsyncInvokable, IPayloadObserverUpdatable
        {
            public Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer;

            public Type GetInterfaceType()
            {
                return typeof(ISubject);
            }

            public Task<IValueGetable> InvokeAsync(object __target)
            {
                return null;
            }

            void IPayloadObserverUpdatable.Update(Action<IInterfacedObserver> updater)
            {
                if (observer != null)
                {
                    updater(observer);
                }
            }
        }
    }

    public interface ISubject_NoReply
    {
        void MakeEvent(System.String eventName);
        void Subscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer);
        void Unsubscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer);
    }

    public class SubjectRef : InterfacedActorRef, ISubject, ISubject_NoReply
    {
        public SubjectRef() : base(null)
        {
        }

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

        public Task Subscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Subscribe_Invoke { observer = observer }
            };
            return SendRequestAndWait(requestMessage);
        }

        public Task Unsubscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Unsubscribe_Invoke { observer = observer }
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

        void ISubject_NoReply.Subscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Subscribe_Invoke { observer = observer }
            };
            SendRequest(requestMessage);
        }

        void ISubject_NoReply.Unsubscribe(Akka.Interfaced.SlimClient.Tests.ISubjectObserver observer)
        {
            var requestMessage = new RequestMessage {
                InvokePayload = new ISubject_PayloadTable.Unsubscribe_Invoke { observer = observer }
            };
            SendRequest(requestMessage);
        }
    }
}

#endregion
#region Akka.Interfaced.SlimClient.Tests.ISubjectObserver

namespace Akka.Interfaced.SlimClient.Tests
{
    [PayloadTable(typeof(ISubjectObserver), PayloadTableKind.Notification)]
    public static class ISubjectObserver_PayloadTable
    {
        public static Type[] GetPayloadTypes()
        {
            return new Type[] {
                typeof(Event_Invoke),
            };
        }

        public class Event_Invoke : IInterfacedPayload, IInvokable
        {
            public System.String eventName;

            public Type GetInterfaceType()
            {
                return typeof(ISubjectObserver);
            }

            public void Invoke(object __target)
            {
                ((ISubjectObserver)__target).Event(eventName);
            }
        }
    }

    public class SubjectObserver : InterfacedObserver, ISubjectObserver
    {
        public SubjectObserver()
            : base(null, 0)
        {
        }

        public SubjectObserver(INotificationChannel channel, int observerId = 0)
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
