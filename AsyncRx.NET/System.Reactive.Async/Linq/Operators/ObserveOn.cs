// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
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
        /// discards anything queued. The producer's path takes no lock: it enqueues and counts,
        /// and the drain trampolines on that count (Rx.NET's "missed" pattern), so a producer
        /// never waits for the drain and never runs on its thread. An earlier form guarded both
        /// paths with an <see cref="AsyncGate"/>, whose release hands the lock to the next waiter
        /// and runs its continuation inline; a producer waiting on the gate then continued on
        /// the drain's thread, before the drain delivered, and a producer that went on to block
        /// until that delivery deadlocked with it.
        /// </remarks>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) ObserveOn<TSource>(this IAsyncObserver<TSource> observer, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var queue = new ConcurrentQueue<TSource>();
            var error = default(Exception);
            var isDone = false;
            var isDisposed = false;
            var pending = 0;
            var drain = new SerialAsyncDisposable();

            async ValueTask DrainAsync(CancellationToken ct)
            {
                var missed = 1;

                while (true)
                {
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            return;
                        }

                        if (queue.TryDequeue(out var next))
                        {
                            await observer.OnNextAsync(next).RendezVous(scheduler, ct);
                            continue;
                        }

                        if (Volatile.Read(ref isDone))
                        {
                            var terminal = Volatile.Read(ref error);

                            if (terminal != null)
                            {
                                await observer.OnErrorAsync(terminal).RendezVous(scheduler, ct);
                            }
                            else
                            {
                                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            }

                            return;
                        }

                        break;
                    }

                    missed = Interlocked.Add(ref pending, -missed);

                    if (missed == 0)
                    {
                        return;
                    }
                }
            }

            async ValueTask SignalAsync()
            {
                if (Interlocked.Increment(ref pending) != 1)
                {
                    return;
                }

                if (Volatile.Read(ref isDisposed))
                {
                    return;
                }

                var scheduled = await scheduler.ScheduleAsync(DrainAsync).ConfigureAwait(false);

                await drain.AssignAsync(scheduled).ConfigureAwait(false);
            }

            var sink = Create<TSource>(
                x =>
                {
                    queue.Enqueue(x);

                    return SignalAsync();
                },
                ex =>
                {
                    Volatile.Write(ref error, ex);
                    Volatile.Write(ref isDone, true);

                    return SignalAsync();
                },
                () =>
                {
                    Volatile.Write(ref isDone, true);

                    return SignalAsync();
                }
            );

            var dispose = AsyncDisposable.Create(async () =>
            {
                Volatile.Write(ref isDisposed, true);

                while (queue.TryDequeue(out _))
                {
                }

                await drain.DisposeAsync().ConfigureAwait(false);
            });

            return (sink, dispose);
        }
    }
}
