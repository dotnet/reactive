// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(async observer =>
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

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = AsyncObserver.TakeLast(observer, count, scheduler);

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

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(async observer =>
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

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = AsyncObserver.TakeLast(observer, duration, clock);

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

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = AsyncObserver.TakeLast(observer, duration, clock, scheduler);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, drain);
            });
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler) => TakeLast(source, duration, scheduler, scheduler);
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, int count) => TakeLast(observer, count, TaskPoolAsyncScheduler.Default);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
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

                            return Task.CompletedTask;
                        },
                        observer.OnErrorAsync,
                        async () =>
                        {
                            var drain = await scheduler.ScheduleAsync(async ct =>
                            {
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

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => TakeLast(observer, duration, Clock.Default, TaskPoolAsyncScheduler.Default);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler) => TakeLast(observer, duration, scheduler, scheduler);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock) => TakeLast(observer, duration, clock, TaskPoolAsyncScheduler.Default);

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

                            return Task.CompletedTask;
                        },
                        observer.OnErrorAsync,
                        async () =>
                        {
                            Trim(clock.Now);

                            var drain = await scheduler.ScheduleAsync(async ct =>
                            {
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
