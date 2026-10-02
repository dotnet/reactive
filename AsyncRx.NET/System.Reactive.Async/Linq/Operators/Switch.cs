// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Switch<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<IAsyncObservable<TSource>, TSource>(
                source,
                async static (source, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, cancel) = AsyncObserver.Switch(observer, subscription);

                    var inner = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(inner).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, cancel);
                });
        }
    }

    public partial class AsyncObserver
    {
        /// <summary>
        /// Creates an observer that forwards the most recent inner sequence it receives to
        /// <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to forward the latest inner sequence to.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence. The returned observer disposes it when the
        /// outer sequence completes, so that the outer source is released as soon as nothing
        /// more can come from it, as Rx.NET does.
        /// </param>
        public static (IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable) Switch<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable subscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var gate = new AsyncGate();

            var isStopped = false;
            var hasLatest = false;
            var latest = 0UL;

            var disposable = new SerialAsyncDisposable();

            return
                (
                    Create<IAsyncObservable<TSource>>(
                        async xs =>
                        {
                            ulong id;

                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasLatest = true;
                                id = unchecked(++latest);
                            }

                            // Holds this inner subscription so that it can be released as soon as
                            // the inner sequence completes, as Rx.NET does, rather than when the
                            // next inner sequence replaces it.
                            var inner = new SingleAssignmentAsyncDisposable();

                            await disposable.AssignAsync(inner).ConfigureAwait(false);

                            var innerObserver = Create<TSource>(
                                async x =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        if (latest == id)
                                        {
                                            await observer.OnNextAsync(x).ConfigureAwait(false);
                                        }
                                    }
                                },
                                async ex =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        if (latest == id)
                                        {
                                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                        }
                                    }
                                },
                                async () =>
                                {
                                    using (await gate.LockAsync().ConfigureAwait(false))
                                    {
                                        await inner.DisposeAsync().ConfigureAwait(false);

                                        if (latest == id)
                                        {
                                            hasLatest = false;

                                            if (isStopped)
                                            {
                                                await observer.OnCompletedAsync().ConfigureAwait(false);
                                            }
                                        }
                                    }
                                }
                            );

                            var innerSubscription = await xs.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                            await inner.AssignAsync(innerSubscription).ConfigureAwait(false);
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
                                // Nothing more can arrive from the outer source; release it now
                                // rather than when the result completes.
                                await subscription.DisposeAsync().ConfigureAwait(false);

                                isStopped = true;

                                if (!hasLatest)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    ),
                    disposable
                );
        }
    }
}
