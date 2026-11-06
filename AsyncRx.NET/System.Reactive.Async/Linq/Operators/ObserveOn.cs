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
        /// discards anything queued. The queue and its flags are guarded by a monitor, never held
        /// across an await: a producer enqueues and, if no drain is running, marks one as running
        /// and schedules it outside the lock; the drain marks itself finished only under the lock
        /// and only on finding the queue empty, so a producer that enqueues at that moment
        /// starts the next drain. A monitor release runs nobody else's code, so a producer never
        /// waits for the drain and never continues on its thread, and the drain never completes
        /// a producer's task. An <see cref="AsyncGate"/> would not do here: its release hands the
        /// lock to the next waiter inline, so a producer waiting on it would continue on the
        /// drain's thread before the drain delivered, and one that then blocked for that delivery
        /// would deadlock with it.
        /// </remarks>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) ObserveOn<TSource>(this IAsyncObserver<TSource> observer, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new object();
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

                    lock (gate)
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

            async ValueTask StartDrainAsync()
            {
                var scheduled = await scheduler.ScheduleAsync(DrainAsync).ConfigureAwait(false);

                await drain.AssignAsync(scheduled).ConfigureAwait(false);
            }

            var sink = Create<TSource>(
                x =>
                {
                    bool start;

                    lock (gate)
                    {
                        queue.Enqueue(x);

                        start = !isDraining && !isDisposed;

                        if (start)
                        {
                            isDraining = true;
                        }
                    }

                    return start ? StartDrainAsync() : default;
                },
                ex =>
                {
                    bool start;

                    lock (gate)
                    {
                        error = ex;

                        start = !isDraining && !isDisposed;

                        if (start)
                        {
                            isDraining = true;
                        }
                    }

                    return start ? StartDrainAsync() : default;
                },
                () =>
                {
                    bool start;

                    lock (gate)
                    {
                        isDone = true;

                        start = !isDraining && !isDisposed;

                        if (start)
                        {
                            isDraining = true;
                        }
                    }

                    return start ? StartDrainAsync() : default;
                }
            );

            var dispose = AsyncDisposable.Create(async () =>
            {
                lock (gate)
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
