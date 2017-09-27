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
        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var sourceSubscription = new SingleAssignmentAsyncDisposable();

                var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, dueTime).ConfigureAwait(false);

                var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                return disposable;
            });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var sourceSubscription = new SingleAssignmentAsyncDisposable();

                var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, dueTime, scheduler).ConfigureAwait(false);

                var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                return disposable;
            });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Create<TSource>(async observer =>
            {
                var sourceSubscription = new SingleAssignmentAsyncDisposable();

                var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, dueTime, other).ConfigureAwait(false);

                var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                return disposable;
            });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncObservable<TSource> other, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var sourceSubscription = new SingleAssignmentAsyncDisposable();

                var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, dueTime, other, scheduler).ConfigureAwait(false);

                var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                return disposable;
            });
        }
    }

    partial class AsyncObserver
    {
        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan dueTime)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));

            return Timeout(observer, sourceSubscription, dueTime, AsyncObservable.Throw<TSource>(new TimeoutException()), TaskPoolAsyncScheduler.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Timeout(observer, sourceSubscription, dueTime, AsyncObservable.Throw<TSource>(new TimeoutException()), scheduler);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan dueTime, IAsyncObservable<TSource> other)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Timeout(observer, sourceSubscription, dueTime, other, TaskPoolAsyncScheduler.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan dueTime, IAsyncObservable<TSource> other, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var gate = new AsyncLock();

                var switched = false;
                var id = 0UL;

                var timer = new SerialAsyncDisposable();
                var subscription = new SerialAsyncDisposable();

                var d = StableCompositeAsyncDisposable.Create(timer, subscription);

                await subscription.AssignAsync(sourceSubscription).ConfigureAwait(false);

                async Task<bool> OnAsync()
                {
                    var hasWon = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (!switched)
                        {
                            unchecked
                            {
                                ++id;
                            }

                            hasWon = true;
                        }
                    }

                    return hasWon;
                }

                async Task CreateTimerAsync()
                {
                    var timerId = id;

                    var timeout = await scheduler.ScheduleAsync(async ct =>
                    {
                        var hasWon = false;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            hasWon = switched = timerId == id;
                        }

                        if (hasWon)
                        {
                            var otherSubscription = await other.SubscribeSafeAsync(observer).RendezVous(scheduler, ct);

                            await subscription.AssignAsync(otherSubscription).RendezVous(scheduler, ct);
                        }
                    }, dueTime).ConfigureAwait(false);

                    await timer.AssignAsync(timeout).ConfigureAwait(false);
                }

                var sink = Create<TSource>(
                    async x =>
                    {
                        if (await OnAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(x).ConfigureAwait(false);

                            await CreateTimerAsync().ConfigureAwait(false);
                        }
                    },
                    async ex =>
                    {
                        if (await OnAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        if (await OnAsync().ConfigureAwait(false))
                        {
                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    }
                );

                await CreateTimerAsync().ConfigureAwait(false);

                return (sink, d);
            }
        }
    }
}
