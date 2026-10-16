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
        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return CreateAsyncObservable<TSource>.From(
                source,
                count,
                static (source, count, observer) => source.SubscribeSafeAsync(AsyncObserver.Take(observer, count)));
        }

        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // As in Rx.NET, the scheduler only matters for the degenerate count of zero, where
            // the completion is scheduled rather than delivered immediately.
            if (count == 0)
            {
                return Empty<TSource>(scheduler);
            }

            return CreateAsyncObservable<TSource>.From(
                source,
                count,
                static (source, count, observer) => source.SubscribeSafeAsync(AsyncObserver.Take(observer, count)));
        }

        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            // No special case for a zero duration: as in Rx.NET, the source is subscribed and the
            // completion is delivered by the scheduler (observable via the source subscription).

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want Take on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                duration,
                static async (source, duration, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.Take(observer, duration).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want Take on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                (duration, scheduler),
                static async (source, state, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.Take(observer, state.duration, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Take<TSource>(IAsyncObserver<TSource> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<TSource>(
                async x =>
                {
                    var remaining = --count;

                    await observer.OnNextAsync(x).ConfigureAwait(false);

                    if (remaining == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => Take(observer, duration, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want TakeUntil on the observer?
                // DESIGN: It seems that if an observer would be an IAsyncDisposable, this could get a bit easier ("inject" the inner disposable).

                var gate = new AsyncGate();
                var done = false;

                // Once the sequence has completed (by the timer or by the source), nothing more
                // may be forwarded: a source notification that was already waiting for the gate
                // when the timer fired must be dropped, as Rx.NET's disposed sink drops it.
                //
                // NOTE: nothing here is specific to Take. This is the async counterpart of the
                // protection Rx.NET's sink base classes give every operator (a disposed sink
                // forwards to a no-op observer, so an upstream notification that races a
                // termination is swallowed rather than becoming a grammar violation).
                // AsyncRx.NET's AsyncObserverBase takes the opposite stance and throws on
                // "already terminated", so any operator that can terminate from a second
                // source of events (a timer, another sequence) while a delivery is in flight
                // needs this same "terminated under the gate => drop" logic. It should
                // eventually move somewhere shared — a gated, terminate-once sink base — rather
                // than be repeated per operator.
                var sourceObserver = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!done)
                            {
                                await observer.OnNextAsync(x).ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!done)
                            {
                                done = true;
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!done)
                            {
                                done = true;
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    });

                return
                    (
                        sourceObserver,
                        await scheduler.ScheduleAsync(async ct =>
                        {
                            if (!ct.IsCancellationRequested)
                            {
                                using (await gate.LockAsync().RendezVous(scheduler, ct))
                                {
                                    if (!done)
                                    {
                                        done = true;
                                        await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                                    }
                                }
                            }
                        }, duration).ConfigureAwait(false)
                    );
            }
        }
    }
}
