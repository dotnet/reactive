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
        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                count,
                static (source, count, observer) => source.SubscribeSafeAsync(AsyncObserver.Buffer(observer, count)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (skip <= 0)
                throw new ArgumentNullException(nameof(skip));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (count, skip),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.Buffer(observer, state.count, state.skip)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                timeSpan,
                static async (source, timeSpan, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, timeSpan).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (timeSpan, scheduler),
                static async (source, state, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, state.timeSpan, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (timeSpan, timeShift),
                static async (source, state, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, state.timeSpan, state.timeShift).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (timeSpan, timeShift, scheduler),
                static async (source, state, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, state.timeSpan, state.timeShift, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (timeSpan, count),
                static async (source, state, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, state.timeSpan, state.count).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (timeSpan, count, scheduler),
                static async (source, state, observer) =>
                {
                    var (sink, timer) = await AsyncObserver.Buffer(observer, state.timeSpan, state.count, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(this IAsyncObservable<TSource> source, IAsyncObservable<TBufferBoundary> bufferBoundaries)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferBoundaries == null)
                throw new ArgumentNullException(nameof(bufferBoundaries));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                bufferBoundaries,
                static async (source, bufferBoundaries, observer) =>
                {
                    var (sourceObserver, boundariesObserver) = AsyncObserver.Buffer<TSource, TBufferBoundary>(observer);

                    var sourceSubscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);
                    var boundariesSubscription = await bufferBoundaries.SubscribeSafeAsync(boundariesObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, boundariesSubscription);
                });
        }

        // REVIEW: This overload is inherited from Rx but arguably a bit esoteric as it doesn't provide context to the closing selector.

        public static IAsyncObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TBufferClosing>> bufferClosingSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferClosingSelector == null)
                throw new ArgumentNullException(nameof(bufferClosingSelector));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                bufferClosingSelector,
                static async (source, bufferClosingSelector, observer) =>
                {
                    var (sourceObserver, closingDisposable) = await AsyncObserver.Buffer<TSource, TBufferClosing>(observer, bufferClosingSelector).ConfigureAwait(false);

                    var sourceSubscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, closingDisposable);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count) => Buffer(observer, count, count);

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count, int skip)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (skip <= 0)
                throw new ArgumentNullException(nameof(skip));

            var queue = new Queue<IList<TSource>>();
            var n = 0;

            void CreateBuffer() => queue.Enqueue(new List<TSource>());

            CreateBuffer();

            return Create<TSource>(
                async x =>
                {
                    foreach (var buffer in queue)
                    {
                        buffer.Add(x);
                    }

                    var c = n - count + 1;

                    if (c >= 0 && c % skip == 0)
                    {
                        var buffer = queue.Dequeue();

                        if (buffer.Count > 0)
                        {
                            await observer.OnNextAsync(buffer).ConfigureAwait(false);
                        }
                    }

                    n++;

                    if (n % skip == 0)
                    {
                        CreateBuffer();
                    }
                },
                ex =>
                {
                    while (queue.Count > 0)
                    {
                        queue.Dequeue().Clear();
                    }

                    return observer.OnErrorAsync(ex);
                },
                async () =>
                {
                    while (queue.Count > 0)
                    {
                        var buffer = queue.Dequeue();

                        if (buffer.Count > 0)
                        {
                            await observer.OnNextAsync(buffer).ConfigureAwait(false);
                        }
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan) => Buffer(observer, timeSpan, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var gate = new AsyncGate();

                var buffer = new List<TSource>();

                var sink = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            buffer.Add(x);
                        }
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
                            if (buffer.Count > 0)
                            {
                                await observer.OnNextAsync(buffer).ConfigureAwait(false);
                            }

                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    }
                );

                var timer = await scheduler.ScheduleAsync(async ct =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (buffer.Count > 0)
                            {
                                await observer.OnNextAsync(buffer).ConfigureAwait(false);

                                buffer = new List<TSource>();
                            }
                        }

                        await scheduler.Delay(timeSpan, ct).RendezVous(scheduler, ct);
                    }
                }, timeSpan).ConfigureAwait(false);

                return (sink, timer);
            };
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, TimeSpan timeShift) => Buffer(observer, timeSpan, timeShift, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var gate = new AsyncGate();

                var queue = new Queue<List<TSource>>();

                queue.Enqueue(new List<TSource>());

                var sink = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            foreach (var buffer in queue)
                            {
                                buffer.Add(x);
                            }
                        }
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
                            while (queue.Count > 0)
                            {
                                var buffer = queue.Dequeue();

                                if (buffer.Count > 0)
                                {
                                    await observer.OnNextAsync(buffer).ConfigureAwait(false);
                                }
                            }

                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    }
                );

                var nextOpen = timeShift;
                var nextClose = timeSpan;
                var totalTime = TimeSpan.Zero;

                var isOpen = false;
                var isClose = false;

                TimeSpan GetNextDue()
                {
                    if (nextOpen == nextClose)
                    {
                        isOpen = isClose = true;
                    }
                    else if (nextClose < nextOpen)
                    {
                        isClose = true;
                        isOpen = false;
                    }
                    else
                    {
                        isOpen = true;
                        isClose = false;
                    }

                    var newTotalTime = isClose ? nextClose : nextOpen;
                    var due = newTotalTime - totalTime;
                    totalTime = newTotalTime;

                    if (isOpen)
                    {
                        nextOpen += timeShift;
                    }

                    if (isClose)
                    {
                        nextClose += timeShift;
                    }

                    return due;
                }

                var timer = await scheduler.ScheduleAsync(async ct =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (isClose)
                            {
                                var buffer = queue.Dequeue();

                                if (buffer.Count > 0)
                                {
                                    await observer.OnNextAsync(buffer).RendezVous(scheduler, ct);
                                }
                            }

                            if (isOpen)
                            {
                                queue.Enqueue(new List<TSource>());
                            }
                        }

                        await scheduler.Delay(GetNextDue(), ct).RendezVous(scheduler, ct);
                    }
                }, GetNextDue()).ConfigureAwait(false);

                return (sink, timer);
            };
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, int count) => Buffer(observer, timeSpan, count, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var gate = new AsyncGate();

                var timer = new SerialAsyncDisposable();

                var buffer = new List<TSource>();
                var n = 0;
                var id = 0;

                async Task CreateTimerAsync(int timerId)
                {
                    var d = await scheduler.ScheduleAsync(async ct =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (timerId == id)
                            {
                                await FlushAsync().ConfigureAwait(false);
                            }
                        }
                    }, timeSpan).ConfigureAwait(false);

                    await timer.AssignAsync(d).ConfigureAwait(false);
                }

                async Task FlushAsync()
                {
                    n = 0;
                    ++id;

                    var res = buffer;
                    buffer = new List<TSource>();
                    await observer.OnNextAsync(buffer).ConfigureAwait(false);

                    await CreateTimerAsync(id).ConfigureAwait(false);
                }

                var sink = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            buffer.Add(x);

                            if (++n == count)
                            {
                                await FlushAsync().ConfigureAwait(false);
                            }
                        }
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
                            await observer.OnNextAsync(buffer).ConfigureAwait(false); // NB: We don't check for non-empty in sync Rx either.
                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    }
                );

                await CreateTimerAsync(0).ConfigureAwait(false);

                return (sink, timer);
            };
        }

        public static (IAsyncObserver<TSource>, IAsyncObserver<TBufferBoundary>) Buffer<TSource, TBufferBoundary>(IAsyncObserver<IList<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncGate();

            var buffer = new List<TSource>();

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                buffer.Add(x);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                buffer.Clear();
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnNextAsync(buffer).ConfigureAwait(false);
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    ),
                    Create<TBufferBoundary>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                var oldBuffer = buffer;
                                buffer = new List<TSource>();
                                await observer.OnNextAsync(oldBuffer).ConfigureAwait(false);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                buffer.Clear();
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnNextAsync(buffer).ConfigureAwait(false);
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Buffer<TSource, TBufferClosing>(IAsyncObserver<IList<TSource>> observer, Func<IAsyncObservable<TBufferClosing>> bufferClosingSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (bufferClosingSelector == null)
                throw new ArgumentNullException(nameof(bufferClosingSelector));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var closeSubscription = new SerialAsyncDisposable();

                var gate = new AsyncGate();
                var queueLock = new AsyncQueueLock();

                var buffer = new List<TSource>();

                async ValueTask CreateBufferCloseAsync()
                {
                    var closing = default(IAsyncObservable<TBufferClosing>);

                    try
                    {
                        closing = bufferClosingSelector(); // REVIEW: Do we need an async variant?
                    }
                    catch (Exception ex)
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            buffer.Clear();
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }

                        return;
                    }

                    var closingSubscription = new SingleAssignmentAsyncDisposable();
                    await closeSubscription.AssignAsync(closingSubscription).ConfigureAwait(false);

                    async ValueTask CloseBufferAsync()
                    {
                        await closingSubscription.DisposeAsync().ConfigureAwait(false);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            var oldBuffer = buffer;
                            buffer = new List<TSource>();
                            await observer.OnNextAsync(oldBuffer).ConfigureAwait(false);
                        }

                        await queueLock.WaitAsync(CreateBufferCloseAsync).ConfigureAwait(false);
                    }

                    var closingObserver =
                        Create<TBufferClosing>(
                            x => CloseBufferAsync(),
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    buffer.Clear();
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            CloseBufferAsync
                        );

                    var closingSubscriptionInner = await closing.SubscribeSafeAsync(closingObserver).ConfigureAwait(false);
                    await closingSubscription.AssignAsync(closingSubscriptionInner).ConfigureAwait(false);
                }

                var sink =
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                buffer.Add(x);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                buffer.Clear();
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnNextAsync(buffer).ConfigureAwait(false);
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    );

                await queueLock.WaitAsync(CreateBufferCloseAsync).ConfigureAwait(false);

                return (sink, closeSubscription);
            }
        }
    }
}
