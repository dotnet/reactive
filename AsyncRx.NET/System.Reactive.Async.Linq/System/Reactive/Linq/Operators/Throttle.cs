// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Throttle<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, throttler) = AsyncObserver.Throttle(observer, dueTime);

                await d.AddAsync(throttler).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(sourceSubscription).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TSource> Throttle<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, throttler) = AsyncObserver.Throttle(observer, dueTime, scheduler);

                await d.AddAsync(throttler).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(sourceSubscription).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TSource> Throttle<TSource, TThrottle>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TThrottle>> throttleSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (throttleSelector == null)
                throw new ArgumentNullException(nameof(throttleSelector));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, throttler) = AsyncObserver.Throttle(observer, throttleSelector);

                await d.AddAsync(throttler).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(sourceSubscription).ConfigureAwait(false);

                return d;
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Throttle<TSource>(IAsyncObserver<TSource> observer, TimeSpan dueTime) => Throttle(observer, dueTime, TaskPoolAsyncScheduler.Default);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Throttle<TSource>(IAsyncObserver<TSource> observer, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new AsyncLock();

            var timer = new SerialAsyncDisposable();

            var hasValue = false;
            var value = default(TSource);
            var id = 0UL;

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            var myId = default(ulong);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasValue = true;
                                value = x;
                                myId = ++id;
                            }

                            var d = new SingleAssignmentAsyncDisposable();
                            await timer.AssignAsync(d).ConfigureAwait(false);

                            var t = await scheduler.ScheduleAsync(async ct =>
                            {
                                if (!ct.IsCancellationRequested)
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        if (hasValue && id == myId)
                                        {
                                            await observer.OnNextAsync(value).ConfigureAwait(false);
                                        }

                                        hasValue = false;
                                    }
                                }
                            }).ConfigureAwait(false);
                            await d.AssignAsync(t).ConfigureAwait(false);
                        },
                        async ex =>
                        {
                            await timer.DisposeAsync().ConfigureAwait(false);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);

                                hasValue = false;
                                id++;
                            }
                        },
                        async () =>
                        {
                            await timer.DisposeAsync().ConfigureAwait(false);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (hasValue)
                                {
                                    await observer.OnNextAsync(value).ConfigureAwait(false);
                                }

                                await observer.OnCompletedAsync().ConfigureAwait(false);

                                hasValue = false;
                                id++;
                            }
                        }
                    ),
                    timer
                );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Throttle<TSource, TThrottle>(IAsyncObserver<TSource> observer, Func<TSource, IAsyncObservable<TThrottle>> throttleSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (throttleSelector == null)
                throw new ArgumentNullException(nameof(throttleSelector));

            var gate = new AsyncLock();

            var throttler = new SerialAsyncDisposable();

            var hasValue = false;
            var value = default(TSource);
            var id = 0UL;

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            var throttleSource = default(IAsyncObservable<TThrottle>);

                            try
                            {
                                throttleSource = throttleSelector(x); // REVIEW: Do we need an async variant?
                            }
                            catch (Exception ex)
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }

                                return;
                            }

                            var myId = default(ulong);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasValue = true;
                                value = x;
                                myId = ++id;
                            }

                            var d = new SingleAssignmentAsyncDisposable();
                            await throttler.AssignAsync(d).ConfigureAwait(false);

                            var throttleObserver = Create<TThrottle>(
                                    async y =>
                                    {
                                        using (await gate.LockAsync().ConfigureAwait(false))
                                        {
                                            if (hasValue && myId == id)
                                            {
                                                await observer.OnNextAsync(x).ConfigureAwait(false);
                                            }

                                            hasValue = false;

                                            await d.DisposeAsync().ConfigureAwait(false);
                                        }
                                    },
                                    async ex =>
                                    {
                                        using (await gate.LockAsync().ConfigureAwait(false))
                                        {
                                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                        }
                                    },
                                    async () =>
                                    {
                                        using (await gate.LockAsync().ConfigureAwait(false))
                                        {
                                            if (hasValue && myId == id)
                                            {
                                                await observer.OnNextAsync(x).ConfigureAwait(false);
                                            }

                                            hasValue = false;

                                            await d.DisposeAsync().ConfigureAwait(false);
                                        }
                                    }
                                );

                            var t = await throttleSource.SubscribeSafeAsync(throttleObserver).ConfigureAwait(false);
                            await d.AssignAsync(t).ConfigureAwait(false);
                        },
                        async ex =>
                        {
                            await throttler.DisposeAsync().ConfigureAwait(false);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);

                                hasValue = false;
                                id++;
                            }
                        },
                        async () =>
                        {
                            await throttler.DisposeAsync().ConfigureAwait(false);

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (hasValue)
                                {
                                    await observer.OnNextAsync(value).ConfigureAwait(false);
                                }

                                await observer.OnCompletedAsync().ConfigureAwait(false);

                                hasValue = false;
                                id++;
                            }
                        }
                    ),
                    throttler
                );
        }
    }
}
