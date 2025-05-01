﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Threading;

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
                    var (sink, cancel) = AsyncObserver.Switch(observer);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, cancel);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<IAsyncObservable<TSource>>, IAsyncDisposable) Switch<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = AsyncGate.Create();

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

                            var inner = await xs.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                            await disposable.AssignAsync(inner).ConfigureAwait(false);
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
