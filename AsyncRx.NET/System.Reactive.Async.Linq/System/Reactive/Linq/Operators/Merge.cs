// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        // TODO: Add Merge with max concurrency and IEnumerable<T>-based overloads.

        public static IAsyncObservable<TSource> Merge<TSource>(this IAsyncObservable<IAsyncObservable<TSource>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var (sink, cancel) = AsyncObserver.Merge(observer);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, cancel);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable) Merge<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var count = 1;

            var disposable = new CompositeAsyncDisposable();

            async Task OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            };

            async Task OnCompletedAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (--count == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
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
                        OnCompletedAsync
                    ),
                    disposable
                );
        }
    }
}
