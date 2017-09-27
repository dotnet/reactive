// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<(T1 arg1, T2 arg2)> FromEvent<T1, T2>(Action<Action<T1, T2>> addHandler, Action<Action<T1, T2>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2>, T1, T2>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2)> FromEvent<T1, T2>(Action<Action<T1, T2>> addHandler, Action<Action<T1, T2>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2>, T1, T2>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2)> FromEvent<TDelegate, T1, T2>(Func<Action<T1, T2>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2)> FromEvent<TDelegate, T1, T2>(Func<Action<T1, T2>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2)> FromEventCore<TDelegate, T1, T2>(Func<Action<T1, T2>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2)>(observer =>
                    {
                        var handler = new Action<T1, T2>((arg1, arg2) =>
                        {
                            observer.OnNextAsync((arg1, arg2)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3)> FromEvent<T1, T2, T3>(Action<Action<T1, T2, T3>> addHandler, Action<Action<T1, T2, T3>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3>, T1, T2, T3>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3)> FromEvent<T1, T2, T3>(Action<Action<T1, T2, T3>> addHandler, Action<Action<T1, T2, T3>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3>, T1, T2, T3>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3)> FromEvent<TDelegate, T1, T2, T3>(Func<Action<T1, T2, T3>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3)> FromEvent<TDelegate, T1, T2, T3>(Func<Action<T1, T2, T3>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3)> FromEventCore<TDelegate, T1, T2, T3>(Func<Action<T1, T2, T3>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3>((arg1, arg2, arg3) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)> FromEvent<T1, T2, T3, T4>(Action<Action<T1, T2, T3, T4>> addHandler, Action<Action<T1, T2, T3, T4>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4>, T1, T2, T3, T4>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)> FromEvent<T1, T2, T3, T4>(Action<Action<T1, T2, T3, T4>> addHandler, Action<Action<T1, T2, T3, T4>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4>, T1, T2, T3, T4>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)> FromEvent<TDelegate, T1, T2, T3, T4>(Func<Action<T1, T2, T3, T4>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)> FromEvent<TDelegate, T1, T2, T3, T4>(Func<Action<T1, T2, T3, T4>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)> FromEventCore<TDelegate, T1, T2, T3, T4>(Func<Action<T1, T2, T3, T4>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4>((arg1, arg2, arg3, arg4) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)> FromEvent<T1, T2, T3, T4, T5>(Action<Action<T1, T2, T3, T4, T5>> addHandler, Action<Action<T1, T2, T3, T4, T5>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5>, T1, T2, T3, T4, T5>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)> FromEvent<T1, T2, T3, T4, T5>(Action<Action<T1, T2, T3, T4, T5>> addHandler, Action<Action<T1, T2, T3, T4, T5>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5>, T1, T2, T3, T4, T5>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)> FromEvent<TDelegate, T1, T2, T3, T4, T5>(Func<Action<T1, T2, T3, T4, T5>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)> FromEvent<TDelegate, T1, T2, T3, T4, T5>(Func<Action<T1, T2, T3, T4, T5>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)> FromEventCore<TDelegate, T1, T2, T3, T4, T5>(Func<Action<T1, T2, T3, T4, T5>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5>((arg1, arg2, arg3, arg4, arg5) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)> FromEvent<T1, T2, T3, T4, T5, T6>(Action<Action<T1, T2, T3, T4, T5, T6>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6>, T1, T2, T3, T4, T5, T6>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)> FromEvent<T1, T2, T3, T4, T5, T6>(Action<Action<T1, T2, T3, T4, T5, T6>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6>, T1, T2, T3, T4, T5, T6>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6>(Func<Action<T1, T2, T3, T4, T5, T6>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6>(Func<Action<T1, T2, T3, T4, T5, T6>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6>(Func<Action<T1, T2, T3, T4, T5, T6>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6>((arg1, arg2, arg3, arg4, arg5, arg6) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)> FromEvent<T1, T2, T3, T4, T5, T6, T7>(Action<Action<T1, T2, T3, T4, T5, T6, T7>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7>, T1, T2, T3, T4, T5, T6, T7>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)> FromEvent<T1, T2, T3, T4, T5, T6, T7>(Action<Action<T1, T2, T3, T4, T5, T6, T7>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7>, T1, T2, T3, T4, T5, T6, T7>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7>(Func<Action<T1, T2, T3, T4, T5, T6, T7>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7>(Func<Action<T1, T2, T3, T4, T5, T6, T7>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7>(Func<Action<T1, T2, T3, T4, T5, T6, T7>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8>, T1, T2, T3, T4, T5, T6, T7, T8>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8>, T1, T2, T3, T4, T5, T6, T7, T8>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, T1, T2, T3, T4, T5, T6, T7, T8, T9>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, T1, T2, T3, T4, T5, T6, T7, T8, T9>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(h => h, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)> FromEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> addHandler, Action<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> removeHandler, IAsyncScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(h => h, addHandler, removeHandler, scheduler);
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(conversion, addHandler, removeHandler, GetSchedulerForCurrentContext());
        }

        public static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)> FromEvent<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException(nameof(conversion));
            if (addHandler == null)
                throw new ArgumentNullException(nameof(addHandler));
            if (removeHandler == null)
                throw new ArgumentNullException(nameof(removeHandler));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(conversion, addHandler, removeHandler, scheduler);
        }

        private static IAsyncObservable<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)> FromEventCore<TDelegate, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IAsyncScheduler scheduler)
        {
            return
                SynchronizeEvents(
                    Create<(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)>(observer =>
                    {
                        var handler = new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
                        {
                            observer.OnNextAsync((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16)); // REVIEW: Fire-and-forget can lead to out of order processing, and observers may reject these calls as "busy".
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

    }
}
