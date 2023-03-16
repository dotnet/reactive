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
        public static IAsyncObservable<(TFirst first, TSecond second)> WithLatestFrom<TFirst, TSecond>(this IAsyncObservable<TFirst> first, IAsyncObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return CreateAsyncObservable<(TFirst first, TSecond second)>.From(
                first,
                second,
                static async (first, second, observer) =>
                {
                    var (firstObserver, secondObserver) = AsyncObserver.WithLatestFrom(observer);

                    // REVIEW: Consider concurrent subscriptions.

                    var firstSubscription = await first.SubscribeSafeAsync(firstObserver).ConfigureAwait(false);
                    var secondSubscription = await second.SubscribeSafeAsync(secondObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
                });
        }

        public static IAsyncObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(this IAsyncObservable<TFirst> first, IAsyncObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                first,
                (second, resultSelector),
                static async (first, state, observer) =>
                {
                    var (firstObserver, secondObserver) = AsyncObserver.WithLatestFrom(observer, state.resultSelector);

                    // REVIEW: Consider concurrent subscriptions.

                    var firstSubscription = await first.SubscribeSafeAsync(firstObserver).ConfigureAwait(false);
                    var secondSubscription = await state.second.SubscribeSafeAsync(secondObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
                });
        }

        public static IAsyncObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(this IAsyncObservable<TFirst> first, IAsyncObservable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                first,
                (second, resultSelector),
                static async (first, state, observer) =>
                {
                    var (firstObserver, secondObserver) = AsyncObserver.WithLatestFrom(observer, state.resultSelector);

                    // REVIEW: Consider concurrent subscriptions.

                    var firstSubscription = await first.SubscribeSafeAsync(firstObserver).ConfigureAwait(false);
                    var secondSubscription = await state.second.SubscribeSafeAsync(secondObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TFirst>, IAsyncObserver<TSecond>) WithLatestFrom<TFirst, TSecond, TResult>(IAsyncObserver<TResult> observer, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return WithLatestFrom<TFirst, TSecond, TResult>(observer, (x, y) => new ValueTask<TResult>(resultSelector(x, y)));
        }

        public static (IAsyncObserver<TFirst>, IAsyncObserver<TSecond>) WithLatestFrom<TFirst, TSecond, TResult>(IAsyncObserver<TResult> observer, Func<TFirst, TSecond, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var gate = new AsyncGate();

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            }

            var hasLatest = false;
            var latest = default(TSecond);

            return
                (
                    Create<TFirst>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (hasLatest)
                                {
                                    var res = default(TResult);

                                    try
                                    {
                                        res = await resultSelector(x, latest).ConfigureAwait(false);
                                    }
                                    catch (Exception ex)
                                    {
                                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                        return;
                                    }

                                    await observer.OnNextAsync(res).ConfigureAwait(false);
                                }
                            }
                        },
                        OnErrorAsync,
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    ),
                    Create<TSecond>(
                        async y =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasLatest = true;
                                latest = y;
                            }
                        },
                        OnErrorAsync,
                        () => default
                    )
                );
        }

        public static (IAsyncObserver<TFirst>, IAsyncObserver<TSecond>) WithLatestFrom<TFirst, TSecond>(IAsyncObserver<(TFirst first, TSecond second)> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncGate();

            async ValueTask OnErrorAsync(Exception ex)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                }
            }

            var hasLatest = false;
            var latest = default(TSecond);

            return
                (
                    Create<TFirst>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (hasLatest)
                                {
                                    await observer.OnNextAsync((first: x, second: latest)).ConfigureAwait(false);
                                }
                            }
                        },
                        OnErrorAsync,
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    ),
                    Create<TSecond>(
                        async y =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasLatest = true;
                                latest = y;
                            }
                        },
                        OnErrorAsync,
                        () => default
                    )
                );
        }
    }
}
