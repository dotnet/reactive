// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<TEventArgs>, TEventArgs>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<TEventArgs>, TEventArgs>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, TEventArgs>(h => ConvertDelegate<Action<TEventArgs>, TDelegate>(h), addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, TEventArgs>(h => ConvertDelegate<Action<TEventArgs>, TDelegate>(h), addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action, Unit>(h => () => h(Unit.Default), addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action, Unit>(h => () => h(Unit.Default), addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<TEventArgs> FromEventCore<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<TEventArgs>(observer =>
                    {
                        var handler = new Action<TEventArgs>(async e =>
                        {
                            await observer.OnNextAsync(e).ConfigureAwait(false);
                        });

                        var converted = conversion(handler);

                        addHandler(converted);

                        return Task.FromResult(AsyncDisposable.Create(() =>
                        {
                            removeHandler(converted);

                            return Task.CompletedTask;
                        }));
                    }),
                    scheduler
                );
        }

        private static IAsyncScheduler GetSchedulerForCurrentContext()
        {
            var context = SynchronizationContext.Current;

            if (context != null)
            {
                return new SynchronizationContextAsyncScheduler(context);
            }

            return ImmediateAsyncScheduler.Instance;
        }

        private static IAsyncObservable<T> SynchronizeEvents<T>(this IAsyncObservable<T> source, IAsyncScheduler scheduler)
        {
            return source.SubscribeOn(scheduler).Publish().RefCount();
        }

        private static TTo ConvertDelegate<TFrom, TTo>(TFrom o)
        {
            var invokeMethod = typeof(TFrom).GetMethod("Invoke");
            return (TTo)(object)Delegate.CreateDelegate(typeof(TTo), o, invokeMethod);
        }
    }
}
