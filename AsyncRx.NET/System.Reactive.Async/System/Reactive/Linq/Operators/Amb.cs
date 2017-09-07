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
        public static IAsyncObservable<TSource> Amb<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create<TSource>(async observer =>
            {
                var firstSubscription = new SingleAssignmentAsyncDisposable();
                var secondSubscription = new SingleAssignmentAsyncDisposable();

                var (firstObserver, secondObserver) = AsyncObserver.Amb(observer, firstSubscription, secondSubscription);

                var firstTask = first.SubscribeAsync(firstObserver).ContinueWith(d => firstSubscription.AssignAsync(d.Result));
                var secondTask = second.SubscribeAsync(secondObserver).ContinueWith(d => secondSubscription.AssignAsync(d.Result));

                await Task.WhenAll(firstTask, secondTask).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSource>) Amb<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable first, IAsyncDisposable second)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            var gate = new AsyncLock();

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
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
                            }
                        }
                    ),
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
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
                            }
                        }
                    )
                );
        }
    }
}
