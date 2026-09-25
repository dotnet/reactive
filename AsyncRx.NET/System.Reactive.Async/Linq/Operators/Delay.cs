// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{

    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Delay(source, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create(
                source,
                (dueTime, scheduler),
                static async (source, state, observer) =>
                {
                    // The sink owns the source subscription so it can release it as soon as the
                    // source terminates (as Rx.NET's sink does), while the delayed notifications
                    // are still being drained.
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, drain) = await AsyncObserver.Delay(observer, subscription, state.dueTime, state.scheduler).ConfigureAwait(false);

                    await subscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Delay(source, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create(
                source,
                (dueTime, scheduler),
                static async (source, state, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, drain) = await AsyncObserver.Delay(observer, subscription, state.dueTime, state.scheduler).ConfigureAwait(false);

                    await subscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> Delay<TSource, TDelay>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TDelay>> delayDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (delayDurationSelector == null)
                throw new ArgumentNullException(nameof(delayDurationSelector));

            return Create(
                source,
                delayDurationSelector,
                static async (source, delayDurationSelector, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, delays) = AsyncObserver.Delay(observer, subscription, delayDurationSelector);

                    await subscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, delays);
                });
        }

        public static IAsyncObservable<TSource> Delay<TSource, TDelay>(this IAsyncObservable<TSource> source, IAsyncObservable<TDelay> subscriptionDelay, Func<TSource, IAsyncObservable<TDelay>> delayDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subscriptionDelay == null)
                throw new ArgumentNullException(nameof(subscriptionDelay));
            if (delayDurationSelector == null)
                throw new ArgumentNullException(nameof(delayDurationSelector));

            return Create(
                source,
                (subscriptionDelay, delayDurationSelector),
                static async (source, state, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, delays) = AsyncObserver.Delay(observer, subscription, state.delayDurationSelector);

                    // The source is subscribed once the subscription delay produces its first
                    // element or completes; that subscription is then released.
                    var delaySubscription = new SingleAssignmentAsyncDisposable();
                    var triggered = false;

                    async ValueTask SubscribeSourceAsync()
                    {
                        if (triggered)
                        {
                            return;
                        }

                        triggered = true;

                        await subscription.AssignAsync(await source.SubscribeSafeAsync(sink).ConfigureAwait(false)).ConfigureAwait(false);
                        await delaySubscription.DisposeAsync().ConfigureAwait(false);
                    }

                    var delayObserver = AsyncObserver.Create<TDelay>(
                        _ => SubscribeSourceAsync(),
                        observer.OnErrorAsync,
                        SubscribeSourceAsync);

                    await delaySubscription.AssignAsync(await state.subscriptionDelay.SubscribeSafeAsync(delayObserver).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(delaySubscription, subscription, delays);
                });
        }
    }

    public partial class AsyncObserver
    {
        // subscription: the subscription to the source, disposed as soon as the source terminates.

        public static ValueTask<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, IAsyncDisposable subscription, TimeSpan dueTime)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            return Delay(observer, subscription, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static ValueTask<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, IAsyncDisposable subscription, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return DelayCore(observer, subscription, scheduler, dueTime, null);
        }

        public static ValueTask<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, IAsyncDisposable subscription, DateTimeOffset dueTime)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            return Delay(observer, subscription, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static ValueTask<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, IAsyncDisposable subscription, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return DelayCore(observer, subscription, scheduler, null, dueTime);
        }

        // Mirrors Rx.NET's Delay sinks. Values are queued with their arrival time and delivered
        // by a drain the scheduler runs when the head is due; completion is delayed by the same
        // amount; errors are forwarded immediately and drop what is queued. For a relative due
        // time the delay is known from the start; for an absolute one nothing is released until
        // that time, when the delay becomes "how long since subscription" and buffered values
        // are released with their original spacing. All asynchrony comes from the scheduler, so
        // the operator behaves the same on a real scheduler and under virtual time.
        private static async ValueTask<(IAsyncObserver<TSource>, IAsyncDisposable)> DelayCore<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable subscription, IAsyncScheduler scheduler, TimeSpan? relativeDueTime, DateTimeOffset? absoluteDueTime)
        {
            var gate = new AsyncGate();
            var queue = new Queue<(TSource Value, DateTimeOffset Arrival)>();
            var drain = new SerialAsyncDisposable();

            var ready = relativeDueTime.HasValue;   // absolute: not until the due time is reached
            var delay = relativeDueTime.GetValueOrDefault();
            var active = false;                     // a drain is scheduled or running
            var running = false;                    // a drain is currently delivering
            var hasCompleted = false;
            var completedAt = default(DateTimeOffset);
            var hasFailed = false;
            var error = default(Exception);

            async ValueTask ScheduleDrainAsync(TimeSpan after)
            {
                var d = await scheduler.ScheduleAsync(DrainAsync, after).ConfigureAwait(false);
                await drain.AssignAsync(d).ConfigureAwait(false);
            }

            async ValueTask DrainAsync(CancellationToken ct)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (hasFailed)
                    {
                        return;
                    }

                    running = true;
                }

                while (!ct.IsCancellationRequested)
                {
                    var hasValue = false;
                    var value = default(TSource);
                    var shouldComplete = false;
                    var shouldFail = false;
                    var shouldReschedule = false;
                    var rescheduleAfter = default(TimeSpan);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (hasFailed)
                        {
                            shouldFail = true;
                            running = false;
                        }
                        else
                        {
                            var now = scheduler.Now;

                            if (queue.Count > 0)
                            {
                                var (nextValue, arrival) = queue.Peek();
                                var due = arrival + delay;

                                if (due <= now)
                                {
                                    queue.Dequeue();
                                    value = nextValue;
                                    hasValue = true;
                                }
                                else
                                {
                                    shouldReschedule = true;
                                    rescheduleAfter = due - now;
                                    running = false;
                                }
                            }
                            else if (hasCompleted)
                            {
                                var due = completedAt + delay;

                                if (due <= now)
                                {
                                    shouldComplete = true;
                                }
                                else
                                {
                                    shouldReschedule = true;
                                    rescheduleAfter = due - now;
                                    running = false;
                                }
                            }
                            else
                            {
                                running = false;
                                active = false;
                            }
                        }
                    }

                    if (hasValue)
                    {
                        await observer.OnNextAsync(value).ConfigureAwait(false);
                        continue;
                    }

                    if (shouldComplete)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                    else if (shouldFail)
                    {
                        await observer.OnErrorAsync(error).ConfigureAwait(false);
                    }
                    else if (shouldReschedule)
                    {
                        await ScheduleDrainAsync(rescheduleAfter).ConfigureAwait(false);
                    }

                    return;
                }
            }

            var sink = Create<TSource>(
                async x =>
                {
                    var shouldRun = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        queue.Enqueue((x, scheduler.Now));
                        shouldRun = ready && !active;
                        active |= shouldRun;
                    }

                    if (shouldRun)
                    {
                        await ScheduleDrainAsync(delay).ConfigureAwait(false);
                    }
                },
                async ex =>
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);

                    var shouldRun = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        queue.Clear();
                        error = ex;
                        hasFailed = true;
                        shouldRun = !running;
                    }

                    if (shouldRun)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                async () =>
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);

                    var shouldRun = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        completedAt = scheduler.Now;
                        hasCompleted = true;
                        shouldRun = ready && !active;
                        active |= shouldRun;
                    }

                    if (shouldRun)
                    {
                        await ScheduleDrainAsync(delay).ConfigureAwait(false);
                    }
                }
            );

            if (!absoluteDueTime.HasValue)
            {
                return (sink, drain);
            }

            var subscribedAt = scheduler.Now;

            var start = await scheduler.ScheduleAsync(
                async ct =>
                {
                    var shouldRun = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        delay = scheduler.Now - subscribedAt;
                        ready = true;
                        shouldRun = (queue.Count > 0 || hasCompleted) && !active;
                        active |= shouldRun;
                    }

                    if (shouldRun)
                    {
                        await ScheduleDrainAsync(TimeSpan.Zero).ConfigureAwait(false);
                    }
                },
                absoluteDueTime.Value).ConfigureAwait(false);

            return (sink, StableCompositeAsyncDisposable.Create(start, drain));
        }

        // Mirrors Rx.NET's selector-based Delay sink: each value is held until the sequence the
        // selector returns for it produces its first element or completes; the source
        // subscription is released when the source completes; the operator completes once the
        // source has completed and no value is still held; any error, from the source, a
        // selector, or a delay sequence, is forwarded immediately.
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Delay<TSource, TDelay>(this IAsyncObserver<TSource> observer, IAsyncDisposable subscription, Func<TSource, IAsyncObservable<TDelay>> delayDurationSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (delayDurationSelector == null)
                throw new ArgumentNullException(nameof(delayDurationSelector));

            var gate = new AsyncGate();
            var delays = new CompositeAsyncDisposable();
            var pending = 0;
            var atEnd = false;

            async ValueTask CheckDoneAsync()
            {
                if (atEnd && pending == 0)
                {
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            }

            async ValueTask OnErrorAsync(Exception error)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(error).ConfigureAwait(false);
                }
            }

            var sink = Create<TSource>(
                async x =>
                {
                    IAsyncObservable<TDelay> delay;

                    try
                    {
                        delay = delayDurationSelector(x);
                    }
                    catch (Exception error)
                    {
                        await OnErrorAsync(error).ConfigureAwait(false);
                        return;
                    }

                    var delaySubscription = new SingleAssignmentAsyncDisposable();
                    var once = false;

                    async ValueTask ReleaseAsync()
                    {
                        if (once)
                        {
                            return;
                        }

                        once = true;

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(x).ConfigureAwait(false);
                            pending--;
                            await delays.RemoveAsync(delaySubscription).ConfigureAwait(false);
                            await CheckDoneAsync().ConfigureAwait(false);
                        }
                    }

                    var delayObserver = Create<TDelay>(
                        _ => ReleaseAsync(),
                        OnErrorAsync,
                        ReleaseAsync);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        pending++;
                    }

                    await delays.AddAsync(delaySubscription).ConfigureAwait(false);
                    await delaySubscription.AssignAsync(await delay.SubscribeSafeAsync(delayObserver).ConfigureAwait(false)).ConfigureAwait(false);
                },
                OnErrorAsync,
                async () =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        atEnd = true;
                        await subscription.DisposeAsync().ConfigureAwait(false);
                        await CheckDoneAsync().ConfigureAwait(false);
                    }
                }
            );

            return (sink, delays);
        }
    }
}
