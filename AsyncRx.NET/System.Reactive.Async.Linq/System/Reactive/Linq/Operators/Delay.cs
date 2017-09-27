// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // TODO: Add overloads with DateTimeOffset and with duration selector.

    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = await AsyncObserver.Delay(observer, dueTime).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, drain);
            });
        }

        public static IAsyncObservable<TSource> Delay<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = await AsyncObserver.Delay(observer, dueTime, scheduler).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, drain);
            });
        }
    }

    partial class AsyncObserver
    {
        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, TimeSpan dueTime)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Delay(observer, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Delay<TSource>(this IAsyncObserver<TSource> observer, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // TODO: Use stopwatch functionality.

            var start = scheduler.Now;

            var semaphore = new SemaphoreSlim(0);

            var gate = new AsyncLock();

            var queue = new Queue<TimeInterval<TSource>>();
            var isDone = false;

            var drain = await scheduler.ScheduleAsync(async ct =>
            {
                while (!ct.IsCancellationRequested)
                {
                    await semaphore.WaitAsync(ct).RendezVous(scheduler, ct);

                    if (queue.Count > 0)
                    {
                        var next = queue.Dequeue();

                        var nextDueTime = start + next.Interval + dueTime;
                        var delay = nextDueTime - scheduler.Now;

                        await scheduler.Delay(delay, ct).RendezVous(scheduler, ct);

                        await observer.OnNextAsync(next.Value).RendezVous(scheduler, ct);
                    }

                    if (queue.Count == 0 && isDone)
                    {
                        await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                        break;
                    }
                }
            }).ConfigureAwait(false);

            var sink = Create<TSource>(
                async x =>
                {
                    var time = scheduler.Now;
                    var delay = time - start;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        queue.Enqueue(new TimeInterval<TSource>(x, delay));
                    }

                    semaphore.Release(1);
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
                        isDone = true;
                    }

                    semaphore.Release(1);
                }
            );

            return (sink, drain);
        }
    }
}
