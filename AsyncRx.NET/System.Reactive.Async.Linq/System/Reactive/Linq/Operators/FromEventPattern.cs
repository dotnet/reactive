// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler) => FromEventPattern(addHandler, removeHandler, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEvent<EventHandler, object, EventArgs>(action => new EventHandler((o, e) => action(o, e)), addHandler, removeHandler, scheduler).Select(t => new EventPattern<object>(t.arg1, t.arg2));
        }

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler) => FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEvent<TDelegate, object, TEventArgs>(action => ConvertDelegate<Action<object, TEventArgs>, TDelegate>(action), addHandler, removeHandler, scheduler).Select(t => new EventPattern<TEventArgs>(t.arg1, t.arg2));
        }

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) => FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEvent<TDelegate, object, TEventArgs>(action => conversion(new EventHandler<TEventArgs>((o, e) => action(o, e))), addHandler, removeHandler, scheduler).Select(t => new EventPattern<TEventArgs>(t.arg1, t.arg2));
        }

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler) => FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEvent<TDelegate, TSender, TEventArgs>(action => ConvertDelegate<Action<TSender, TEventArgs>, TDelegate>(action), addHandler, removeHandler, scheduler).Select(t => new EventPattern<TSender, TEventArgs>(t.arg1, t.arg2));
        }

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler) => FromEventPattern<TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEvent<EventHandler<TEventArgs>, object, TEventArgs>(action => ConvertDelegate<Action<object, TEventArgs>, EventHandler<TEventArgs>>(action), addHandler, removeHandler, scheduler).Select(t => new EventPattern<TEventArgs>(t.arg1, t.arg2));
        }

        public static IAsyncObservable<EventPattern<object>> FromEventPattern(object target, string eventName) => FromEventPattern(target, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<object>> FromEventPattern(object target, string eventName, IAsyncScheduler scheduler)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<object, object, EventPattern<object>>(target.GetType(), target, eventName, scheduler, (o, e) => new EventPattern<object>(o, e));
        }

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName) => FromEventPattern<TEventArgs>(target, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName, IAsyncScheduler scheduler)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<object, TEventArgs, EventPattern<TEventArgs>>(target.GetType(), target, eventName, scheduler, (o, e) => new EventPattern<TEventArgs>(o, e));
        }

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName) => FromEventPattern<TSender, TEventArgs>(target, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName, IAsyncScheduler scheduler)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(target.GetType(), target, eventName, scheduler, (o, e) => new EventPattern<TSender, TEventArgs>(o, e));
        }

        public static IAsyncObservable<EventPattern<object>> FromEventPattern(Type type, string eventName) => FromEventPattern(type, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<object>> FromEventPattern(Type type, string eventName, IAsyncScheduler scheduler)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<object, object, EventPattern<object>>(type, null, eventName, scheduler, (o, e) => new EventPattern<object>(o, e));
        }

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName) => FromEventPattern<TEventArgs>(type, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName, IAsyncScheduler scheduler)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<object, TEventArgs, EventPattern<TEventArgs>>(type, null, eventName, scheduler, (o, e) => new EventPattern<TEventArgs>(o, e));
        }

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName) => FromEventPattern<TSender, TEventArgs>(type, eventName, GetSchedulerForCurrentContext());

        public static IAsyncObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName, IAsyncScheduler scheduler)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventPatternCore<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(type, null, eventName, scheduler, (o, e) => new EventPattern<TSender, TEventArgs>(o, e));
        }

        private static IAsyncObservable<TResult> FromEventPatternCore<TSender, TEventArgs, TResult>(Type type, object target, string eventName, IAsyncScheduler scheduler, Func<TSender, TEventArgs, TResult> resultSelector)
        {
            var (addMethod, removeMethod, delegateType, isWinRT) = GetEventMethods<object, object>(target.GetType(), target, eventName);

            var res = default(IAsyncObservable<TResult>);

            if (isWinRT)
            {
                res = Create<TResult>(observer =>
                {
                    var onNext = new Action<TSender, TEventArgs>((o, e) =>
                    {
                        observer.OnNextAsync(resultSelector(o, e)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
                    });

                    var d = Delegate.CreateDelegate(delegateType, onNext, typeof(Action<TSender, TEventArgs>).GetMethod("Invoke"));

                    var token = default(object);

                    try
                    {
                        token = addMethod.Invoke(target, new object[] { d });
                    }
                    catch (TargetInvocationException tie)
                    {
                        ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                    }

                    var dispose = AsyncDisposable.Create(() =>
                    {
                        try
                        {
                            removeMethod.Invoke(target, new object[] { token });
                        }
                        catch (TargetInvocationException tie)
                        {
                            ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                        }

                        return Task.CompletedTask;
                    });

                    return Task.FromResult(dispose);
                });
            }
            else
            {
                res = Create<TResult>(observer =>
                {
                    var onNext = new Action<TSender, TEventArgs>((o, e) =>
                    {
                        observer.OnNextAsync(resultSelector(o, e)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
                    });

                    var d = Delegate.CreateDelegate(delegateType, onNext, typeof(Action<TSender, TEventArgs>).GetMethod("Invoke"));

                    try
                    {
                        addMethod.Invoke(target, new object[] { d });
                    }
                    catch (TargetInvocationException tie)
                    {
                        ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                    }

                    var dispose = AsyncDisposable.Create(() =>
                    {
                        try
                        {
                            removeMethod.Invoke(target, new object[] { d });
                        }
                        catch (TargetInvocationException tie)
                        {
                            ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                        }

                        return Task.CompletedTask;
                    });

                    return Task.FromResult(dispose);
                });
            }

            return SynchronizeEvents(res, scheduler);
        }

        private static (MethodInfo addMethod, MethodInfo removeMethod, Type delegateType, bool isWinRT) GetEventMethods<TSender, TEventArgs>(Type targetType, object target, string eventName)
        {
            var e = default(EventInfo);

            if (target == null)
            {
                e = targetType.GetEventEx(eventName, isStatic: true);
                if (e == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Could not find static event '{0}' on type '{1}'.", eventName, targetType.FullName));
            }
            else
            {
                e = targetType.GetEventEx(eventName, isStatic: false);
                if (e == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Could not find instance event '{0}' on type '{1}'.", eventName, targetType.FullName));
            }

            var addMethod = e.GetAddMethod();
            var removeMethod = e.GetRemoveMethod();

            if (addMethod == null)
                throw new InvalidOperationException("Missing add method on event.");
            if (removeMethod == null)
                throw new InvalidOperationException("Missing remove method on event.");

            var psa = addMethod.GetParameters();
            if (psa.Length != 1)
                throw new InvalidOperationException("The add method of an event should take one parameter.");

            var psr = removeMethod.GetParameters();
            if (psr.Length != 1)
                throw new InvalidOperationException("The remove method of an event should take one parameter.");

            var isWinRT = false;

            if (addMethod.ReturnType != typeof(void))
            {
                isWinRT = true;

                var pet = psr[0];
                if (pet.ParameterType != addMethod.ReturnType)
                    throw new InvalidOperationException("An event should either have add and remove methods that return void or an add method that returns a type compatible with the parameter type of the remove method.");
            }

            var delegateType = psa[0].ParameterType;

            var invokeMethod = delegateType.GetMethod("Invoke");

            var parameters = invokeMethod.GetParameters();

            if (parameters.Length != 2)
                throw new InvalidOperationException("The delegate type for an event conforming to the traditional event pattern should take two parameters.");

            if (!typeof(TSender).IsAssignableFrom(parameters[0].ParameterType))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The sender parameter of the event is not assignable to '{0}'.", typeof(TSender).FullName));

            if (!typeof(TEventArgs).IsAssignableFrom(parameters[1].ParameterType))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The event arguments parameter of the event is not assignable to '{0}'.", typeof(TEventArgs).FullName));

            if (invokeMethod.ReturnType != typeof(void))
                throw new InvalidOperationException("The return type of an event delegate should be void.");

            return (addMethod, removeMethod, delegateType, isWinRT);
        }

        public static EventInfo GetEventEx(this Type type, string name, bool isStatic)
        {
            return type.GetEvent(name, isStatic ? BindingFlags.Public | BindingFlags.Static : BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
