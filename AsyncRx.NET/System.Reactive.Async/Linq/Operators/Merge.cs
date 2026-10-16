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
        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Create<TSource>(async observer =>
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var (sink, cancel) = AsyncObserver.Merge(observer, subscription);

                var inner = await sources.SubscribeSafeAsync(sink).ConfigureAwait(false);
                await subscription.AssignAsync(inner).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, cancel);
            });
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<Task<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Create<TSource>(async observer =>
            {
                var (sink, cancel) = AsyncObserver.Merge(observer);

                var subscription = await sources.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, cancel);
            });
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> sources, int maxConcurrent)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (maxConcurrent <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));

            return Create<TSource>(async observer =>
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var (sink, cancel) = AsyncObserver.Merge(observer, subscription, maxConcurrent);

                var inner = await sources.SubscribeSafeAsync(sink).ConfigureAwait(false);
                await subscription.AssignAsync(inner).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, cancel);
            });
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources, int maxConcurrent)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (maxConcurrent <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));

            return sources.ToAsyncObservable(ImmediateAsyncScheduler.Instance).Merge(maxConcurrent);
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources, int maxConcurrent, IAsyncScheduler scheduler)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (maxConcurrent <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return sources.ToAsyncObservable(scheduler).Merge(maxConcurrent);
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new[] { first, second }.ToAsyncObservable(ImmediateAsyncScheduler.Instance).Merge();
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second, IAsyncScheduler scheduler)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new[] { first, second }.ToAsyncObservable(scheduler).Merge();
        }

        public static IAsyncObservable<TSource> Merge<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ToAsyncObservable(ImmediateAsyncScheduler.Instance).Merge();
        }

        public static IAsyncObservable<TSource> Merge<TSource>(IAsyncScheduler scheduler, params IAsyncObservable<TSource>[] sources)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ToAsyncObservable(scheduler).Merge();
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ToAsyncObservable(ImmediateAsyncScheduler.Instance).Merge();
        }

        public static IAsyncObservable<TSource> Merge<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources, IAsyncScheduler scheduler)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return sources.ToAsyncObservable(scheduler).Merge();
        }
    }

    public partial class AsyncObserver
    {
        /// <summary>
        /// Creates an observer that merges the inner sequences it receives into
        /// <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to merge the inner sequences into.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence. The returned observer disposes it when the
        /// outer sequence completes while inner sequences are still active, so that the outer
        /// source is released as soon as nothing more can come from it, as Rx.NET does.
        /// </param>
        public static (IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable) Merge<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable subscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var gate = new AsyncGate();

            var count = 1;

            var disposable = new CompositeAsyncDisposable();

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            };

            async ValueTask OnCompletedAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (--count == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            };

            async ValueTask OnOuterCompletedAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (--count == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        // Inner sequences are still running. Nothing more can arrive from the
                        // outer source, so release it now rather than when the result completes.
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                }
            };

            return
                (
                    Create<IAsyncObservable<TSource>>(
                        async xs =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                count++;
                            }

                            var inner = new SingleAssignmentAsyncDisposable();

                            await disposable.AddAsync(inner).ConfigureAwait(false);

                            var innerObserver = Create<TSource>(
                                async x =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        await observer.OnNextAsync(x).ConfigureAwait(false);
                                    }
                                },
                                OnErrorAsync,
                                async () =>
                                {
                                    await OnCompletedAsync().ConfigureAwait(false);

                                    await disposable.RemoveAsync(inner).ConfigureAwait(false);
                                }
                            );

                            var innerSubscription = await xs.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                            await inner.AssignAsync(innerSubscription).ConfigureAwait(false);
                        },
                        OnErrorAsync,
                        OnOuterCompletedAsync
                    ),
                    disposable
                );
        }

        /// <summary>
        /// Creates an observer that merges the inner sequences it receives into
        /// <paramref name="observer"/>, subscribing to at most <paramref name="maxConcurrent"/>
        /// of them at a time and queueing the rest.
        /// </summary>
        /// <param name="observer">The observer to merge the inner sequences into.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence, disposed when the outer sequence completes
        /// while inner sequences are still active or queued, as Rx.NET does.
        /// </param>
        /// <param name="maxConcurrent">The number of inner sequences subscribed to at once.</param>
        public static (IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable) Merge<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable subscription, int maxConcurrent)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (maxConcurrent <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));

            var gate = new AsyncGate();

            var isStopped = false;
            var activeCount = 0;
            var queue = new Queue<IAsyncObservable<TSource>>();

            var disposable = new CompositeAsyncDisposable();

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            }

            async ValueTask SubscribeInnerAsync(IAsyncObservable<TSource> xs)
            {
                var inner = new SingleAssignmentAsyncDisposable();

                await disposable.AddAsync(inner).ConfigureAwait(false);

                var innerObserver = Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(x).ConfigureAwait(false);
                        }
                    },
                    OnErrorAsync,
                    async () =>
                    {
                        await disposable.RemoveAsync(inner).ConfigureAwait(false);

                        var next = default(IAsyncObservable<TSource>);

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (queue.Count > 0)
                            {
                                // The slot passes straight to the next queued sequence.
                                next = queue.Dequeue();
                            }
                            else
                            {
                                activeCount--;

                                if (isStopped && activeCount == 0)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        }

                        if (next != null)
                        {
                            await SubscribeInnerAsync(next).ConfigureAwait(false);
                        }
                    }
                );

                var innerSubscription = await xs.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                await inner.AssignAsync(innerSubscription).ConfigureAwait(false);
            }

            return
                (
                    Create<IAsyncObservable<TSource>>(
                        async xs =>
                        {
                            bool subscribe;

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                subscribe = activeCount < maxConcurrent;

                                if (subscribe)
                                {
                                    activeCount++;
                                }
                                else
                                {
                                    queue.Enqueue(xs);
                                }
                            }

                            if (subscribe)
                            {
                                await SubscribeInnerAsync(xs).ConfigureAwait(false);
                            }
                        },
                        OnErrorAsync,
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                isStopped = true;

                                if (activeCount == 0)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                                else
                                {
                                    // Inner sequences are still running or queued. Nothing more can
                                    // arrive from the outer source, so release it now.
                                    await subscription.DisposeAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    ),
                    disposable
                );
        }

        /// <summary>
        /// Creates an observer that merges the results of the tasks it receives into
        /// <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to merge the task results into.</param>
        /// <remarks>
        /// As Rx.NET's <c>Merge(IObservable&lt;Task&lt;T&gt;&gt;)</c>: each task's result is
        /// forwarded when it completes, a faulted task's single exception (or its
        /// <see cref="AggregateException"/> when there are several) is forwarded as an error, and a
        /// cancelled task is forwarded as a <see cref="TaskCanceledException"/> for that task. The
        /// returned disposable cancels the continuations of tasks still pending.
        /// </remarks>
        public static (IAsyncObserver<Task<TSource>>, IAsyncDisposable) Merge<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncGate();

            var count = 1;

            var cts = new CancellationTokenSource();

            async ValueTask OnCompletedAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (--count == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            }

            async ValueTask OnTaskCompletedAsync(Task<TSource> task)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(task.Result).ConfigureAwait(false);
                        }

                        await OnCompletedAsync().ConfigureAwait(false);
                        break;

                    case TaskStatus.Faulted:
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            var exceptions = task.Exception.InnerExceptions;
                            await observer.OnErrorAsync(exceptions.Count == 1 ? exceptions[0] : task.Exception).ConfigureAwait(false);
                        }

                        break;

                    case TaskStatus.Canceled:
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(new TaskCanceledException(task)).ConfigureAwait(false);
                        }

                        break;
                }
            }

            return
                (
                    Create<Task<TSource>>(
                        async task =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                count++;
                            }

                            if (task.IsCompleted)
                            {
                                await OnTaskCompletedAsync(task).ConfigureAwait(false);
                            }
                            else
                            {
                                _ = task.ContinueWith(
                                    static (t, state) => ((Func<Task<TSource>, ValueTask>)state)(t).AsTask(),
                                    (Func<Task<TSource>, ValueTask>)OnTaskCompletedAsync,
                                    cts.Token,
                                    TaskContinuationOptions.ExecuteSynchronously,
                                    TaskScheduler.Default).Unwrap();
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        OnCompletedAsync
                    ),
                    AsyncDisposable.Create(() =>
                    {
                        cts.Cancel();
                        return default;
                    })
                );
        }
    }
}
