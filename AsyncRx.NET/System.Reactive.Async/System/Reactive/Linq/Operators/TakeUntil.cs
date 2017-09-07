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
        public static IAsyncObservable<TSource> TakeUntil<TSource, TUntil>(this IAsyncObservable<TSource> source, IAsyncObservable<TUntil> until)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (until == null)
                throw new ArgumentNullException(nameof(until));

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, untilObserver) = AsyncObserver.TakeUntil<TSource, TUntil>(observer);

                var d1 = await source.SubscribeAsync(sourceObserver).ConfigureAwait(false);
                var d2 = await until.SubscribeAsync(untilObserver).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(d1, d2);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TUntil>) TakeUntil<TSource, TUntil>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnNextAsync(x).ConfigureAwait(false);
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
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    ),
                    Create<TUntil>(
                        async y =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        () => Task.CompletedTask
                    )
                );
        }
    }
}
