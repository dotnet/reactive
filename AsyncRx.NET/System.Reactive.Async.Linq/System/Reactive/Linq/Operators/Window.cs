// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<IAsyncObservable<TSource>>(observer => WindowCore(source, observer, (o, d) => AsyncObserver.Window(o, d, count)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            return Create<IAsyncObservable<TSource>>(observer => WindowCore(source, observer, (o, d) => AsyncObserver.Window(o, d, count, skip)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan, scheduler)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan, timeShift)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan, timeShift, scheduler)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan, count)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IAsyncObservable<TSource>>(observer => WindowAsyncCore(source, observer, (o, d) => AsyncObserver.Window(o, d, timeSpan, count, scheduler)));
        }

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource, TWindowBoundary>(this IAsyncObservable<TSource> source, IAsyncObservable<TWindowBoundary> windowBoundaries)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (windowBoundaries == null)
                throw new ArgumentNullException(nameof(windowBoundaries));

            return Create<IAsyncObservable<TSource>>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sourceObserver, boundariesObserver, subscription) = await AsyncObserver.Window<TSource, TWindowBoundary>(observer, d).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);
                await d.AddAsync(sourceSubscription).ConfigureAwait(false);

                var boundariesSubscription = await windowBoundaries.SubscribeSafeAsync(boundariesObserver).ConfigureAwait(false);
                await d.AddAsync(boundariesSubscription).ConfigureAwait(false);

                return subscription;
            });
        }

        // REVIEW: This overload is inherited from Rx but arguably a bit esoteric as it doesn't provide context to the closing selector.

        public static IAsyncObservable<IAsyncObservable<TSource>> Window<TSource, TWindowClosing>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TWindowClosing>> windowClosingSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (windowClosingSelector == null)
                throw new ArgumentNullException(nameof(windowClosingSelector));

            return Create<IAsyncObservable<TSource>>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sourceObserver, closingSubscription, subscription) = await AsyncObserver.Window<TSource, TWindowClosing>(observer, windowClosingSelector, d).ConfigureAwait(false);

                await d.AddAsync(closingSubscription).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);
                await d.AddAsync(sourceSubscription).ConfigureAwait(false);

                return subscription;
            });
        }

        private static async Task<IAsyncDisposable> WindowCore<TSource>(IAsyncObservable<TSource> source, IAsyncObserver<IAsyncObservable<TSource>> observer, Func<IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable, (IAsyncObserver<TSource>, IAsyncDisposable)> createObserver)
        {
            var d = new SingleAssignmentAsyncDisposable();

            var (sink, subscription) = createObserver(observer, d);

            var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
            await d.AssignAsync(inner).ConfigureAwait(false);

            return subscription;
        }

        private static async Task<IAsyncDisposable> WindowAsyncCore<TSource>(IAsyncObservable<TSource> source, IAsyncObserver<IAsyncObservable<TSource>> observer, Func<IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable, Task<(IAsyncObserver<TSource>, IAsyncDisposable)>> createObserverAsync)
        {
            var d = new SingleAssignmentAsyncDisposable();

            var (sink, subscription) = await createObserverAsync(observer, d).ConfigureAwait(false);

            var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
            await d.AssignAsync(inner).ConfigureAwait(false);

            return subscription;
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, int count) => Window(observer, subscription, count, count);

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, int count, int skip)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            var refCount = new RefCountAsyncDisposable(subscription);

            var queue = new Queue<IAsyncSubject<TSource>>();
            var n = 0;

            return
                (
                    Create<TSource>
                    (
                        async x =>
                        {
                            foreach (var window in queue)
                            {
                                await window.OnNextAsync(x).ConfigureAwait(false);
                            }

                            var i = n - count + 1;
                            if (i >= 0 && i % skip == 0)
                            {
                                await queue.Dequeue().OnCompletedAsync().ConfigureAwait(false);
                            }

                            n++;

                            if (n % skip == 0)
                            {
                                var window = new SequentialSimpleAsyncSubject<TSource>();
                                queue.Enqueue(window);

                                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);

                                await observer.OnNextAsync(wrapper).ConfigureAwait(false);
                            }
                        },
                        async ex =>
                        {
                            while (queue.Count > 0)
                            {
                                await queue.Dequeue().OnErrorAsync(ex).ConfigureAwait(false);
                            }

                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        },
                        async () =>
                        {
                            while (queue.Count > 0)
                            {
                                await queue.Dequeue().OnCompletedAsync().ConfigureAwait(false);
                            }

                            await observer.OnCompletedAsync().ConfigureAwait(false);
                        }
                    ),
                    refCount
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan) => Window(observer, subscription, timeSpan, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new AsyncLock();

            var window = default(IAsyncSubject<TSource>);
            var d = new CompositeAsyncDisposable();
            var refCount = new RefCountAsyncDisposable(d);

            async Task CreateWindowAsync()
            {
                window = new SequentialSimpleAsyncSubject<TSource>();

                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);

                await observer.OnNextAsync(wrapper).ConfigureAwait(false);
            }

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                await d.AddAsync(subscription).ConfigureAwait(false);

                await CreateWindowAsync().ConfigureAwait(false);

                var timer = await scheduler.ScheduleAsync(async ct =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await window.OnCompletedAsync().ConfigureAwait(false);
                            await CreateWindowAsync().ConfigureAwait(false);
                        }

                        await scheduler.Delay(timeSpan, ct).RendezVous(scheduler, ct);
                    }
                }, timeSpan).ConfigureAwait(false);

                await d.AddAsync(timer).ConfigureAwait(false);

                return
                    (
                        Create<TSource>(
                            async x =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnNextAsync(x).ConfigureAwait(false);
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnErrorAsync(ex).ConfigureAwait(false);
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnCompletedAsync().ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        refCount
                    );
            }

            return CoreAsync();
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan, TimeSpan timeShift) => Window(observer, subscription, timeSpan, timeShift, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new AsyncLock();

            var d = new CompositeAsyncDisposable();
            var timer = new SerialAsyncDisposable();
            var refCount = new RefCountAsyncDisposable(d);

            var queue = new Queue<IAsyncSubject<TSource>>();

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

            async Task CreateWindowAsync()
            {
                var window = new SequentialSimpleAsyncSubject<TSource>();
                queue.Enqueue(window);

                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);

                await observer.OnNextAsync(wrapper).ConfigureAwait(false);
            }

            async Task CreateTimer()
            {
                var inner = new SingleAssignmentAsyncDisposable();
                await timer.AssignAsync(inner).ConfigureAwait(false);

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (isClose)
                            {
                                await queue.Dequeue().OnCompletedAsync().RendezVous(scheduler, ct);
                            }

                            if (isOpen)
                            {
                                await CreateWindowAsync().RendezVous(scheduler, ct);
                            }
                        }

                        await scheduler.Delay(GetNextDue(), ct).RendezVous(scheduler, ct);
                    }
                }, GetNextDue()).ConfigureAwait(false);

                await inner.AssignAsync(task).ConfigureAwait(false);
            }

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                await d.AddAsync(subscription).ConfigureAwait(false);
                await d.AddAsync(timer).ConfigureAwait(false);

                await CreateWindowAsync().ConfigureAwait(false);
                await CreateTimer().ConfigureAwait(false);

                return
                    (
                        Create<TSource>(
                            async x =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    foreach (var window in queue)
                                    {
                                        await window.OnNextAsync(x).ConfigureAwait(false);
                                    }
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    while (queue.Count > 0)
                                    {
                                        await queue.Dequeue().OnErrorAsync(ex).ConfigureAwait(false);
                                    }

                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    while (queue.Count > 0)
                                    {
                                        await queue.Dequeue().OnCompletedAsync().ConfigureAwait(false);
                                    }

                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        refCount
                    );
            }

            return CoreAsync();
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan, int count) => Window(observer, subscription, timeSpan, count, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Window<TSource>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var gate = new AsyncLock();

            var n = 0;
            var window = default(IAsyncSubject<TSource>);

            var d = new CompositeAsyncDisposable();
            var timer = new SerialAsyncDisposable();
            var refCount = new RefCountAsyncDisposable(d);

            async Task CreateTimer(IAsyncSubject<TSource> currentWindow)
            {
                var inner = new SingleAssignmentAsyncDisposable();
                await timer.AssignAsync(inner).ConfigureAwait(false);

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    var newWindow = default(IAsyncSubject<TSource>);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (window != currentWindow)
                        {
                            return;
                        }

                        n = 0;
                        newWindow = await CreateWindowAsync().RendezVous(scheduler, ct);
                    }

                    await CreateTimer(newWindow).RendezVous(scheduler, ct);
                }, timeSpan).ConfigureAwait(false);

                await inner.AssignAsync(task).ConfigureAwait(false);
            }

            async Task<IAsyncSubject<TSource>> CreateWindowAsync()
            {
                window = new SequentialSimpleAsyncSubject<TSource>();

                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);
                await observer.OnNextAsync(wrapper).ConfigureAwait(false);

                return window;
            }

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                await d.AddAsync(subscription).ConfigureAwait(false);
                await d.AddAsync(timer).ConfigureAwait(false);

                var w = await CreateWindowAsync().ConfigureAwait(false);
                await CreateTimer(w).ConfigureAwait(false);

                return
                    (
                        Create<TSource>(
                            async x =>
                            {
                                var newWindow = default(IAsyncSubject<TSource>);

                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnNextAsync(x).ConfigureAwait(false);

                                    n++;

                                    if (n == count)
                                    {
                                        await window.OnCompletedAsync().ConfigureAwait(false);

                                        n = 0;
                                        newWindow = await CreateWindowAsync().ConfigureAwait(false);
                                    }
                                }

                                if (newWindow != null)
                                {
                                    await CreateTimer(newWindow).ConfigureAwait(false);
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnErrorAsync(ex).ConfigureAwait(false);
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnCompletedAsync().ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        refCount
                    );
            }

            return CoreAsync();
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncObserver<TWindowBoundary>, IAsyncDisposable)> Window<TSource, TWindowBoundary>(IAsyncObserver<IAsyncObservable<TSource>> observer, IAsyncDisposable subscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var gate = new AsyncLock();

            var refCount = new RefCountAsyncDisposable(subscription);
            var window = default(IAsyncSubject<TSource>);

            async Task CreateWindowAsync()
            {
                window = new SequentialSimpleAsyncSubject<TSource>();
                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);
                await observer.OnNextAsync(wrapper).ConfigureAwait(false);
            }

            async Task<(IAsyncObserver<TSource>, IAsyncObserver<TWindowBoundary>, IAsyncDisposable)> CoreAsync()
            {
                await CreateWindowAsync().ConfigureAwait(false);

                return
                    (
                        Create<TSource>(
                            async x =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnNextAsync(x).ConfigureAwait(false);
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnErrorAsync(ex).ConfigureAwait(false);
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnCompletedAsync().ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        Create<TWindowBoundary>(
                            async x =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnCompletedAsync().ConfigureAwait(false);
                                    await CreateWindowAsync().ConfigureAwait(false);
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnErrorAsync(ex).ConfigureAwait(false);
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await window.OnCompletedAsync().ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        refCount
                    );
            }

            return CoreAsync();
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable, IAsyncDisposable)> Window<TSource, TWindowClosing>(IAsyncObserver<IAsyncObservable<TSource>> observer, Func<IAsyncObservable<TWindowClosing>> windowClosingSelector, IAsyncDisposable subscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (windowClosingSelector == null)
                throw new ArgumentNullException(nameof(windowClosingSelector));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var closeSubscription = new SerialAsyncDisposable();

            var gate = new AsyncLock();
            var queueLock = new AsyncQueueLock();

            var refCount = new RefCountAsyncDisposable(subscription);

            var window = default(IAsyncSubject<TSource>);

            async Task CreateWindowAsync()
            {
                window = new SequentialSimpleAsyncSubject<TSource>();
                var wrapper = new WindowAsyncObservable<TSource>(window, refCount);
                await observer.OnNextAsync(wrapper).ConfigureAwait(false);
            }

            async Task CreateWindowCloseAsync()
            {
                var closing = default(IAsyncObservable<TWindowClosing>);

                try
                {
                    closing = windowClosingSelector(); // REVIEW: Do we need an async variant?
                }
                catch (Exception ex)
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }

                    return;
                }

                var closingSubscription = new SingleAssignmentAsyncDisposable();
                await closeSubscription.AssignAsync(closingSubscription).ConfigureAwait(false);

                async Task CloseWindowAsync()
                {
                    await closingSubscription.DisposeAsync().ConfigureAwait(false);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        await window.OnCompletedAsync().ConfigureAwait(false);
                        await CreateWindowAsync().ConfigureAwait(false);
                    }

                    await queueLock.WaitAsync(CreateWindowCloseAsync).ConfigureAwait(false);
                }

                var closingObserver =
                    Create<TWindowClosing>(
                        x => CloseWindowAsync(),
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await window.OnErrorAsync(ex).ConfigureAwait(false);
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        CloseWindowAsync
                    );

                var closingSubscriptionInner = await closing.SubscribeSafeAsync(closingObserver).ConfigureAwait(false);
                await closingSubscription.AssignAsync(closingSubscriptionInner).ConfigureAwait(false);
            }

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable, IAsyncDisposable)> CoreAsync()
            {
                await CreateWindowAsync().ConfigureAwait(false);

                var sink =
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await window.OnNextAsync(x).ConfigureAwait(false);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await window.OnErrorAsync(ex).ConfigureAwait(false);
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await window.OnCompletedAsync().ConfigureAwait(false);
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    );

                await queueLock.WaitAsync(CreateWindowCloseAsync).ConfigureAwait(false);

                return (sink, closeSubscription, refCount);
            }

            return CoreAsync();
        }
    }
}
