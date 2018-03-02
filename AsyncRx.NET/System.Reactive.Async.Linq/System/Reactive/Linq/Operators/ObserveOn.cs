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
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> ObserveOn<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var (sink, drain) = await AsyncObserver.ObserveOn(observer, scheduler).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, drain);
            });
        }
    }

    partial class AsyncObserver
    {
        public static async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> ObserveOn<TSource>(this IAsyncObserver<TSource> observer, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var semaphore = new SemaphoreSlim(0);

            var gate = new AsyncLock();

            var queue = new Queue<TSource>();
            var error = default(Exception);
            var isDone = false;

            var drain = await scheduler.ScheduleAsync(async ct =>
            {
                while (!ct.IsCancellationRequested)
                {
                    await semaphore.WaitAsync(ct).RendezVous(scheduler, ct);

                    if (queue.Count > 0)
                    {
                        var next = queue.Dequeue();

                        await observer.OnNextAsync(next).RendezVous(scheduler, ct);
                    }

                    if (queue.Count == 0)
                    {
                        if (isDone)
                        {
                            await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            break;
                        }

                        if (error != null)
                        {
                            await observer.OnErrorAsync(error).RendezVous(scheduler, ct);
                            break;
                        }
                    }
                }
            }).ConfigureAwait(false);

            var sink = Create<TSource>(
                async x =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        queue.Enqueue(x);
                    }

                    semaphore.Release(1);
                },
                async ex =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        error = ex;
                    }

                    semaphore.Release(1);
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
