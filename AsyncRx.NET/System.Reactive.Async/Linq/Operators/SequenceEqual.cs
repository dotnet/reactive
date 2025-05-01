﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Threading;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        // TODO: Add SequenceEqual<T>(IAsyncObservable<T>, IAsyncEnumerable<T>).

        public static IAsyncObservable<bool> SequenceEqual<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return CreateAsyncObservable<bool>.From(
                first,
                second,
                static async (first, second, observer) =>
                {
                    var (firstObserver, secondObserver) = AsyncObserver.SequenceEqual<TSource>(observer);

                    var firstTask = first.SubscribeSafeAsync(firstObserver);
                    var secondTask = second.SubscribeSafeAsync(secondObserver);

                    // REVIEW: Consider concurrent subscriptions.

                    var d1 = await firstTask.ConfigureAwait(false);
                    var d2 = await secondTask.ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(d1, d2);
                });
        }

        public static IAsyncObservable<bool> SequenceEqual<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<bool>.From(
                first,
                (second, comparer),
                static async (first, state, observer) =>
                {
                    var (firstObserver, secondObserver) = AsyncObserver.SequenceEqual(observer, state.comparer);

                    var firstTask = first.SubscribeSafeAsync(firstObserver);
                    var secondTask = state.second.SubscribeSafeAsync(secondObserver);

                    // REVIEW: Consider concurrent subscriptions.

                    var d1 = await firstTask.ConfigureAwait(false);
                    var d2 = await secondTask.ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(d1, d2);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSource>) SequenceEqual<TSource>(IAsyncObserver<bool> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return SequenceEqual(observer, EqualityComparer<TSource>.Default);
        }

        public static (IAsyncObserver<TSource>, IAsyncObserver<TSource>) SequenceEqual<TSource>(IAsyncObserver<bool> observer, IEqualityComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var gate = AsyncGate.Create();

            var queueLeft = new Queue<TSource>();
            var queueRight = new Queue<TSource>();
            var doneLeft = false;
            var doneRight = false;

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (queueRight.Count > 0)
                                {
                                    var v = queueRight.Dequeue();

                                    var equal = false;
                                    try
                                    {
                                        equal = comparer.Equals(x, v);
                                    }
                                    catch (Exception ex)
                                    {
                                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    if (!equal)
                                    {
                                        await observer.OnNextAsync(false).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                }
                                else if (doneRight)
                                {
                                    await observer.OnNextAsync(false).ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                                else
                                {
                                    queueLeft.Enqueue(x);
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
                                doneLeft = true;

                                if (queueLeft.Count == 0)
                                {
                                    if (queueRight.Count > 0)
                                    {
                                        await observer.OnNextAsync(false).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                    else if (doneRight)
                                    {
                                        await observer.OnNextAsync(true).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                }
                            }
                        }
                    ),
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (queueLeft.Count > 0)
                                {
                                    var v = queueLeft.Dequeue();

                                    var equal = false;
                                    try
                                    {
                                        equal = comparer.Equals(v, x);
                                    }
                                    catch (Exception ex)
                                    {
                                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    if (!equal)
                                    {
                                        await observer.OnNextAsync(false).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                }
                                else if (doneLeft)
                                {
                                    await observer.OnNextAsync(false).ConfigureAwait(false);
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                                else
                                {
                                    queueRight.Enqueue(x);
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
                                doneRight = true;

                                if (queueRight.Count == 0)
                                {
                                    if (queueLeft.Count > 0)
                                    {
                                        await observer.OnNextAsync(false).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                    else if (doneLeft)
                                    {
                                        await observer.OnNextAsync(true).ConfigureAwait(false);
                                        await observer.OnCompletedAsync().ConfigureAwait(false);
                                    }
                                }
                            }
                        }
                    )
                );
        }
    }
}
