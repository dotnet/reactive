// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return CreateAsyncObservable<TSource>.From(
                source,
                dueTime,
                static async (source, dueTime, observer) =>
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
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, scheduler),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.scheduler).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, other),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.other).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncObservable<TSource> other, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, other, scheduler),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.other, state.scheduler).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }
        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return CreateAsyncObservable<TSource>.From(
                source,
                dueTime,
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, scheduler),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.scheduler).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime, IAsyncObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, other),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.other).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime, IAsyncObservable<TSource> other, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            return CreateAsyncObservable<TSource>.From(
                source,
                (dueTime, other, scheduler),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.dueTime, state.other, state.scheduler).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }

        public static IAsyncObservable<TSource> Timeout<TSource, TTimeout>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TTimeout>> timeoutDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));

            // As in Rx.NET: no first timeout, and a TimeoutException when a timeout occurs.
            return Timeout(source, Never<TTimeout>(), timeoutDurationSelector, Throw<TSource>(new TimeoutException()));
        }

        public static IAsyncObservable<TSource> Timeout<TSource, TTimeout>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TTimeout>> timeoutDurationSelector, IAsyncObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Timeout(source, Never<TTimeout>(), timeoutDurationSelector, other);
        }

        public static IAsyncObservable<TSource> Timeout<TSource, TTimeout>(this IAsyncObservable<TSource> source, IAsyncObservable<TTimeout> firstTimeout, Func<TSource, IAsyncObservable<TTimeout>> timeoutDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (firstTimeout == null)
                throw new ArgumentNullException(nameof(firstTimeout));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));

            return Timeout(source, firstTimeout, timeoutDurationSelector, Throw<TSource>(new TimeoutException()));
        }

        public static IAsyncObservable<TSource> Timeout<TSource, TTimeout>(this IAsyncObservable<TSource> source, IAsyncObservable<TTimeout> firstTimeout, Func<TSource, IAsyncObservable<TTimeout>> timeoutDurationSelector, IAsyncObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (firstTimeout == null)
                throw new ArgumentNullException(nameof(firstTimeout));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return CreateAsyncObservable<TSource>.From(
                source,
                (firstTimeout, timeoutDurationSelector, other),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sink, disposable) = await AsyncObserver.Timeout(observer, sourceSubscription, state.firstTimeout, state.timeoutDurationSelector, state.other).ConfigureAwait(false);

                    var sourceSubscriptionInner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(sourceSubscriptionInner).ConfigureAwait(false);

                    return disposable;
                });
        }
    }

    public partial class AsyncObserver
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
                var gate = new AsyncGate();

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

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, DateTimeOffset dueTime)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));

            return Timeout(observer, sourceSubscription, dueTime, AsyncObservable.Throw<TSource>(new TimeoutException()), TaskPoolAsyncScheduler.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Timeout(observer, sourceSubscription, dueTime, AsyncObservable.Throw<TSource>(new TimeoutException()), scheduler);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, DateTimeOffset dueTime, IAsyncObservable<TSource> other)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Timeout(observer, sourceSubscription, dueTime, other, TaskPoolAsyncScheduler.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, DateTimeOffset dueTime, IAsyncObservable<TSource> other, IAsyncScheduler scheduler)
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
                // A port of Rx.NET's absolute Timeout: one timer for the whole subscription, and
                // the first of the timer and the source's terminal notification wins. The gate is
                // held while a value is delivered, so a timer that fires meanwhile switches to the
                // other sequence once the delivery has finished, as in Rx.NET.
                var gate = new AsyncGate();
                var switched = false;

                var subscription = new SerialAsyncDisposable();
                await subscription.AssignAsync(sourceSubscription).ConfigureAwait(false);

                var timer = await scheduler.ScheduleAsync(async ct =>
                {
                    bool won;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        won = !switched;
                        switched = true;
                    }

                    if (won)
                    {
                        var otherSubscription = await other.SubscribeSafeAsync(observer).RendezVous(scheduler, ct);
                        await subscription.AssignAsync(otherSubscription).RendezVous(scheduler, ct);
                    }
                }, dueTime).ConfigureAwait(false);

                var sink = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!switched)
                            {
                                await observer.OnNextAsync(x).ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!switched)
                            {
                                switched = true;
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!switched)
                            {
                                switched = true;
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

                return (sink, StableCompositeAsyncDisposable.Create(timer, subscription));
            }
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Timeout<TSource, TTimeout>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, IAsyncObservable<TTimeout> firstTimeout, Func<TSource, IAsyncObservable<TTimeout>> timeoutDurationSelector, IAsyncObservable<TSource> other)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (firstTimeout == null)
                throw new ArgumentNullException(nameof(firstTimeout));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                // A port of Rx.NET's Timeout with a duration selector. Each element starts a numbered
                // timeout sequence; only the sequence with the current number can time the source
                // out (a value or a completion from it), or fail it (an error from it), and the
                // number is retired once the outcome is decided.
                var gate = new AsyncGate();
                var index = 0L;

                var timer = new SerialAsyncDisposable();
                var subscription = new SerialAsyncDisposable();
                await subscription.AssignAsync(sourceSubscription).ConfigureAwait(false);

                async ValueTask TimeoutAsync(long idx)
                {
                    bool won;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        won = index == idx;

                        if (won)
                        {
                            index = long.MaxValue;
                        }
                    }

                    if (won)
                    {
                        await timer.DisposeAsync().ConfigureAwait(false);

                        var otherSubscription = await other.SubscribeSafeAsync(observer).ConfigureAwait(false);
                        await subscription.AssignAsync(otherSubscription).ConfigureAwait(false);
                    }
                }

                async ValueTask TimeoutErrorAsync(long idx, Exception error)
                {
                    bool won;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        won = index == idx;

                        if (won)
                        {
                            index = long.MaxValue;
                        }
                    }

                    if (won)
                    {
                        await observer.OnErrorAsync(error).ConfigureAwait(false);
                    }
                }

                async Task SetTimerAsync(IAsyncObservable<TTimeout> timeout, long idx)
                {
                    var timeoutObserver = Create<TTimeout>(
                        _ => TimeoutAsync(idx),
                        error => TimeoutErrorAsync(idx, error),
                        () => TimeoutAsync(idx));

                    var timeoutSubscription = await timeout.SubscribeSafeAsync(timeoutObserver).ConfigureAwait(false);
                    await timer.AssignAsync(timeoutSubscription).ConfigureAwait(false);
                }

                var sink = Create<TSource>(
                    async x =>
                    {
                        long idx;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            idx = index;

                            if (idx == long.MaxValue)
                            {
                                return;
                            }

                            index = idx + 1;
                        }

                        await observer.OnNextAsync(x).ConfigureAwait(false);

                        IAsyncObservable<TTimeout> timeout;

                        try
                        {
                            timeout = timeoutDurationSelector(x);
                        }
                        catch (Exception ex)
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                index = long.MaxValue;
                            }

                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await SetTimerAsync(timeout, idx + 1).ConfigureAwait(false);
                    },
                    async ex =>
                    {
                        bool won;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            won = index != long.MaxValue;
                            index = long.MaxValue;
                        }

                        if (won)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        bool won;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            won = index != long.MaxValue;
                            index = long.MaxValue;
                        }

                        if (won)
                        {
                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    }
                );

                await SetTimerAsync(firstTimeout, 0L).ConfigureAwait(false);

                return (sink, StableCompositeAsyncDisposable.Create(timer, subscription));
            }
        }
    }
}
