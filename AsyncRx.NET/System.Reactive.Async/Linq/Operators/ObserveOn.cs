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
        public static IAsyncObservable<TSource> ObserveOn<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                scheduler,
                static async (source, scheduler, observer) =>
                {
                    var (sink, drain) = AsyncObserver.ObserveOn(observer, scheduler);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, drain);
                });
        }

        public static IAsyncObservable<TSource> ObserveOn<TSource>(this IAsyncObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return ObserveOn(source, new SynchronizationContextAsyncScheduler(context));
        }
    }

    public partial class AsyncObserver
    {
        /// <summary>
        /// Creates an observer that delivers the notifications it receives to
        /// <paramref name="observer"/> on <paramref name="scheduler"/>.
        /// </summary>
        /// <param name="observer">The observer to deliver to.</param>
        /// <param name="scheduler">Scheduler to notify the observer on.</param>
        /// <remarks>
        /// As Rx.NET's scheduled observer: notifications are queued, and a drain is scheduled
        /// whenever one is queued and none is running; the drain delivers the queued values in
        /// order, then the error or completion once the values are gone, and ends when the queue
        /// is empty. An exception thrown by the observer escapes from the scheduled drain to the
        /// scheduler, as in Rx.NET. Disposing the returned disposable stops the current drain and
        /// discards anything queued. The earlier form kept one drain alive for the life of the
        /// subscription, awaiting a <see cref="SemaphoreSlim"/> and rendezvousing back on the
        /// scheduler after each release. That delivered correctly, but the semaphore completes
        /// its waiters on the thread pool, so every notification cost a pool hop before the
        /// rendezvous; this form, like Rx.NET's, never leaves the scheduler.
        /// </remarks>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) ObserveOn<TSource>(this IAsyncObserver<TSource> observer, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new AsyncGate();

            var queue = new Queue<TSource>();
            var error = default(Exception);
            var isDone = false;
            var isDraining = false;
            var isDisposed = false;

            var drain = new SerialAsyncDisposable();

            async ValueTask DrainAsync(CancellationToken ct)
            {
                while (!ct.IsCancellationRequested)
                {
                    var next = default(TSource);
                    var hasNext = false;
                    var terminal = default(Exception);
                    var completed = false;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (isDisposed)
                        {
                            isDraining = false;
                            return;
                        }

                        if (queue.Count > 0)
                        {
                            next = queue.Dequeue();
                            hasNext = true;
                        }
                        else if (error != null)
                        {
                            terminal = error;
                        }
                        else if (isDone)
                        {
                            completed = true;
                        }
                        else
                        {
                            isDraining = false;
                            return;
                        }
                    }

                    if (hasNext)
                    {
                        await observer.OnNextAsync(next).RendezVous(scheduler, ct);
                    }
                    else if (terminal != null)
                    {
                        await observer.OnErrorAsync(terminal).RendezVous(scheduler, ct);
                        return;
                    }
                    else if (completed)
                    {
                        await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                        return;
                    }
                }
            }

            async ValueTask EnsureDrainingAsync()
            {
                var start = false;

                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (!isDraining && !isDisposed)
                    {
                        isDraining = true;
                        start = true;
                    }
                }

                if (start)
                {
                    var scheduled = await scheduler.ScheduleAsync(DrainAsync).ConfigureAwait(false);

                    await drain.AssignAsync(scheduled).ConfigureAwait(false);
                }
            }

            var sink = Create<TSource>(
                async x =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        queue.Enqueue(x);
                    }

                    await EnsureDrainingAsync().ConfigureAwait(false);
                },
                async ex =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        error = ex;
                    }

                    await EnsureDrainingAsync().ConfigureAwait(false);
                },
                async () =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        isDone = true;
                    }

                    await EnsureDrainingAsync().ConfigureAwait(false);
                }
            );

            var dispose = AsyncDisposable.Create(async () =>
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    isDisposed = true;
                    queue.Clear();
                }

                await drain.DisposeAsync().ConfigureAwait(false);
            });

            return (sink, dispose);
        }
    }
}
