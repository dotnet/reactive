// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return CreateAsyncObservable<TSource>.From(
                source,
                count,
                static async (source, count, observer) =>
                {
                    var (sink, drain) = AsyncObserver.TakeLast(observer, count);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (count, scheduler),
                static async (source, state, observer) =>
                {
                    var (sink, drain) = AsyncObserver.TakeLast(observer, state.count, state.scheduler);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return CreateAsyncObservable<TSource>.From(
                source,
                duration,
                static async (source, duration, observer) =>
                {
                    var (sink, drain) = AsyncObserver.TakeLast(observer, duration);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            return CreateAsyncObservable<TSource>.From(
                source,
                (duration, clock),
                static async (source, state, observer) =>
                {
                    var (sink, drain) = AsyncObserver.TakeLast(observer, state.duration, state.clock);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (duration, clock, scheduler),
                static async (source, state, observer) =>
                {
                    var (sink, drain) = AsyncObserver.TakeLast(observer, state.duration, state.clock, state.scheduler);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // As in Rx.NET: the scheduler runs the timer, and the loop is the immediate one.
            return TakeLast(source, duration, scheduler, ImmediateAsyncScheduler.Instance);
        }
    }

    public partial class AsyncObserver
    {
        // As in Rx.NET, whose loop scheduler defaults to its immediate one: with no scheduler given,
        // the queue drains where the source completed.
        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, int count) => TakeLast(observer, count, ImmediateAsyncScheduler.Instance);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            // As in Rx.NET: a count of zero (and a duration of zero, below) still subscribes, and
            // completes or fails when the source does, with nothing drained.
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var sad = new SingleAssignmentAsyncDisposable();

            var queue = new Queue<TSource>();

            return
                (
                    Create<TSource>(
                        x =>
                        {
                            queue.Enqueue(x);

                            if (queue.Count > count)
                            {
                                queue.Dequeue();
                            }

                            return default;
                        },
                        observer.OnErrorAsync,
                        async () =>
                        {
                            var drain = await scheduler.ScheduleAsync((observer, scheduler, queue), static async (state, ct) =>
                            {
                                var (observer, scheduler, queue) = state;

                                while (!ct.IsCancellationRequested && queue.Count > 0)
                                {
                                    await observer.OnNextAsync(queue.Dequeue()).RendezVous(scheduler, ct);
                                }

                                ct.ThrowIfCancellationRequested();

                                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            }).ConfigureAwait(false);

                            await sad.AssignAsync(drain).ConfigureAwait(false);
                        }
                    ),
                    sad
                );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => TakeLast(observer, duration, Clock.Default, ImmediateAsyncScheduler.Instance);

        // As in Rx.NET: a single scheduler is the timer scheduler, and the loop is the immediate one.
        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler) => TakeLast(observer, duration, scheduler, ImmediateAsyncScheduler.Instance);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock) => TakeLast(observer, duration, clock, ImmediateAsyncScheduler.Instance);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var sad = new SingleAssignmentAsyncDisposable();

            var queue = new Queue<Timestamped<TSource>>();

            void Trim(DateTimeOffset now)
            {
                while (queue.Count > 0 && now - queue.Peek().Timestamp >= duration)
                {
                    queue.Dequeue();
                }
            }

            return
                (
                    Create<TSource>(
                        x =>
                        {
                            var now = clock.Now;

                            queue.Enqueue(new Timestamped<TSource>(x, now));

                            Trim(now);

                            return default;
                        },
                        observer.OnErrorAsync,
                        async () =>
                        {
                            Trim(clock.Now);

                            var drain = await scheduler.ScheduleAsync((observer, scheduler, queue), static async (state, ct) =>
                            {
                                var (observer, scheduler, queue) = state;

                                while (!ct.IsCancellationRequested && queue.Count > 0)
                                {
                                    await observer.OnNextAsync(queue.Dequeue().Value).RendezVous(scheduler, ct);
                                }

                                ct.ThrowIfCancellationRequested();

                                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            }).ConfigureAwait(false);

                            await sad.AssignAsync(drain).ConfigureAwait(false);
                        }
                    ),
                    sad
                );
        }
    }
}
