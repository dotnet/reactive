// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;

#if HAS_WINRT
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using ObservableImpl;
#endif

    //
    // BREAKING CHANGE v2 > v1.x - FromEvent[Pattern] now has an implicit SubscribeOn and Publish operation.
    //
    // See FromEvent.cs for more information.
    //
    internal partial class QueryLanguage
    {
        #region + FromEventPattern +

        #region Strongly typed

        #region Action<EventHandler>

        public virtual IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler)
        {
            return FromEventPattern_(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IScheduler scheduler)
        {
            return FromEventPattern_(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<object>> FromEventPattern_(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEventPattern.Impl<EventHandler, object>(e => new EventHandler(e), addHandler, removeHandler, scheduler);
#else
            var res = Observable.FromEventPattern<EventHandler, object>(e => new EventHandler(e), addHandler, removeHandler);
            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        #endregion

        #region Action<TDelegate>

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            return FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEventPattern.Impl<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
#else
            var res = new AnonymousObservable<EventPattern<TEventArgs>>(observer =>
            {
                Action<object, TEventArgs> handler = (sender, eventArgs) => observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
                var d = ReflectionUtils.CreateDelegate<TDelegate>(handler, typeof(Action<object, TEventArgs>).GetMethod(nameof(Action<object, TEventArgs>.Invoke)));
                addHandler(d);
                return Disposable.Create(() => removeHandler(d));
            });
            
            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            return FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEventPattern.Impl<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
#else
            var res = new AnonymousObservable<EventPattern<TEventArgs>>(observer =>
            {
                var handler = conversion((sender, eventArgs) => observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs)));
                addHandler(handler);
                return Disposable.Create(() => removeHandler(handler));
            });

            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            return FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEventPattern.Impl<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
#else
            var res = new AnonymousObservable<EventPattern<TSender, TEventArgs>>(observer =>
            {
                Action<TSender, TEventArgs> handler = (sender, eventArgs) => observer.OnNext(new EventPattern<TSender, TEventArgs>(sender, eventArgs));
                var d = ReflectionUtils.CreateDelegate<TDelegate>(handler, typeof(Action<TSender, TEventArgs>).GetMethod(nameof(Action<TSender, TEventArgs>.Invoke)));
                addHandler(d);
                return Disposable.Create(() => removeHandler(d));
            });

            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        #endregion

        #region Action<EventHandler<TEventArgs>>

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler)
        {
            return FromEventPattern_<TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler, IScheduler scheduler)
        {
            return FromEventPattern_<TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEventPattern.Impl<EventHandler<TEventArgs>, TEventArgs>(handler => handler, addHandler, removeHandler, scheduler);
#else
            var res = Observable.FromEventPattern<EventHandler<TEventArgs>, TEventArgs>(handler => handler, addHandler, removeHandler);
            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        #endregion

        #endregion

        #region Reflection

        #region Instance events

        public virtual IObservable<EventPattern<object>> FromEventPattern(object target, string eventName)
        {
            return FromEventPattern_(target, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<object>> FromEventPattern(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_(target, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<object>> FromEventPattern_(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<object, object, EventPattern<object>>(target.GetType(), target, eventName, (sender, args) => new EventPattern<object>(sender, args), scheduler);
        }

        #endregion

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName)
        {
            return FromEventPattern_<TEventArgs>(target, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TEventArgs>(target, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(target.GetType(), target, eventName, (sender, args) => new EventPattern<TEventArgs>(sender, args), scheduler);
        }

        #endregion

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName)
        {
            return FromEventPattern_<TSender, TEventArgs>(target, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TSender, TEventArgs>(target, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(object target, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(target.GetType(), target, eventName, (sender, args) => new EventPattern<TSender, TEventArgs>(sender, args), scheduler);
        }

        #endregion

        #endregion

        #region Static events

        public virtual IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName)
        {
            return FromEventPattern_(type, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_(type, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<object>> FromEventPattern_(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<object, object, EventPattern<object>>(type, null, eventName, (sender, args) => new EventPattern<object>(sender, args), scheduler);
        }

        #endregion

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName)
        {
            return FromEventPattern_<TEventArgs>(type, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TEventArgs>(type, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(type, null, eventName, (sender, args) => new EventPattern<TEventArgs>(sender, args), scheduler);
        }

        #endregion

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName)
        {
            return FromEventPattern_<TSender, TEventArgs>(type, eventName, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TSender, TEventArgs>(type, eventName, scheduler);
        }

        #region Implementation

        private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(Type type, string eventName, IScheduler scheduler)
        {
            return FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(type, null, eventName, (sender, args) => new EventPattern<TSender, TEventArgs>(sender, args), scheduler);
        }

        #endregion

        #endregion

        #region Helper methods

        private static IObservable<TResult> FromEventPattern_<TSender, TEventArgs, TResult>(Type targetType, object target, string eventName, Func<TSender, TEventArgs, TResult> getResult, IScheduler scheduler)
        {
            var addMethod = default(MethodInfo);
            var removeMethod = default(MethodInfo);
            var delegateType = default(Type);
            var isWinRT = default(bool);
            ReflectionUtils.GetEventMethods<TSender, TEventArgs>(targetType, target, eventName, out addMethod, out removeMethod, out delegateType, out isWinRT);

#if HAS_WINRT
            if (isWinRT)
            {
#if !NO_PERF
                return new FromEventPattern.Handler<TSender, TEventArgs, TResult>(target, delegateType, addMethod, removeMethod, getResult, true, scheduler);
#else
                return new AnonymousObservable<TResult>(observer =>
                {
                    Action<TSender, TEventArgs> handler = (sender, eventArgs) => observer.OnNext(getResult(sender, eventArgs));
                    var d = ReflectionUtils.CreateDelegate(delegateType, handler, typeof(Action<TSender, TEventArgs>).GetMethod(nameof(Action<TSender, TEventArgs>.Invoke)));
                    var token = addMethod.Invoke(target, new object[] { d });
                    return Disposable.Create(() => removeMethod.Invoke(target, new object[] { token }));
                });
#endif
            }
#endif

#if !NO_PERF
            return new FromEventPattern.Handler<TSender, TEventArgs, TResult>(target, delegateType, addMethod, removeMethod, getResult, false, scheduler);
#else
            var res = new AnonymousObservable<TResult>(observer =>
            {
                Action<TSender, TEventArgs> handler = (sender, eventArgs) => observer.OnNext(getResult(sender, eventArgs));
                var d = ReflectionUtils.CreateDelegate(delegateType, handler, typeof(Action<TSender, TEventArgs>).GetMethod(nameof(Action<TSender, TEventArgs>.Invoke)));
                addMethod.Invoke(target, new object[] { d });
                return Disposable.Create(() => removeMethod.Invoke(target, new object[] { d }));
            });

            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        #endregion

        #endregion

        #region FromEvent

        public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            return FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
#else
            var res = new AnonymousObservable<TEventArgs>(observer =>
            {
                var handler = conversion(observer.OnNext);
                addHandler(handler);
                return Disposable.Create(() => removeHandler(handler));
            });

            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            return FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            return FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
#else
            var res = new AnonymousObservable<TEventArgs>(observer =>
            {
                Action<TEventArgs> handler = observer.OnNext;
                var d = ReflectionUtils.CreateDelegate<TDelegate>(handler, typeof(Action<TEventArgs>).GetMethod(nameof(Action<TEventArgs>.Invoke)));
                addHandler(d);
                return Disposable.Create(() => removeHandler(d));
            });

            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler)
        {
            return FromEvent_<TEventArgs>(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler, IScheduler scheduler)
        {
            return FromEvent_<TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<TEventArgs> FromEvent_<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEvent<Action<TEventArgs>, TEventArgs>(h => h, addHandler, removeHandler, scheduler);
#else
            var res = Observable.FromEvent<Action<TEventArgs>, TEventArgs>(h => h, addHandler, removeHandler);
            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        public virtual IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
        {
            return FromEvent_(addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public virtual IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler, IScheduler scheduler)
        {
            return FromEvent_(addHandler, removeHandler, scheduler);
        }

        #region Implementation

        private static IObservable<Unit> FromEvent_(Action<Action> addHandler, Action<Action> removeHandler, IScheduler scheduler)
        {
#if !NO_PERF
            return new FromEvent<Action, Unit>(h => new Action(() => h(new Unit())), addHandler, removeHandler, scheduler);
#else
            var res = Observable.FromEvent<Action, Unit>(h => new Action(() => h(new Unit())), addHandler, removeHandler);
            return SynchronizeEvents(res, scheduler);
#endif
        }

        #endregion

        #endregion

        #region Helpers

        private static IScheduler GetSchedulerForCurrentContext()
        {
            var context = SynchronizationContext.Current;

            if (context != null)
                return new SynchronizationContextScheduler(context, false);
            else
                return SchedulerDefaults.ConstantTimeOperations;
        }

#if NO_PERF

        private static IObservable<T> SynchronizeEvents<T>(IObservable<T> source, IScheduler scheduler)
        {
            return source.SubscribeOn(scheduler).Publish().RefCount();
        }

#endif

        #endregion
    }
}
