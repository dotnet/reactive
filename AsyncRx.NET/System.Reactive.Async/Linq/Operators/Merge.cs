// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        // TODO: Add Merge with max concurrency and IEnumerable<T>-based overloads.

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var (sink, cancel) = AsyncObserver.Merge(observer, subscription);

                var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                await subscription.AssignAsync(inner).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, cancel);
            });
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
    }
}
