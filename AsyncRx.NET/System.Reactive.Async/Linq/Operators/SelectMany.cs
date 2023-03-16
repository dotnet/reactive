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
        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static async (source, selector, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, selector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static async (source, selector, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, selector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                source,
                (collectionSelector, resultSelector),
                static async (source, state, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                source,
                (collectionSelector, resultSelector),
                static async (source, state, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static async (source, selector, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, selector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static async (source, selector, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, selector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                source,
                (collectionSelector, resultSelector),
                static async (source, state, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, int, TCollection, int, ValueTask<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CreateAsyncObservable<TResult>.From(
                source,
                (collectionSelector, resultSelector),
                static async (source, state, observer) =>
                {
                    var (sink, inner) = AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, IAsyncObservable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, x => new ValueTask<IAsyncObservable<TResult>>(selector(x)), (x, y) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, selector, (x, y) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return SelectMany<TSource, TCollection, TResult>(observer, x => new ValueTask<IAsyncObservable<TCollection>>(collectionSelector(x)), (x, y) => new ValueTask<TResult>(resultSelector(x, y)));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

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

            return
            (
                Create<TSource>(
                    async x =>
                    {
                        var collection = default(IAsyncObservable<TCollection>);

                        try
                        {
                            collection = await collectionSelector(x).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            count++;
                        }

                        var inner = new SingleAssignmentAsyncDisposable();

                        await disposable.AddAsync(inner).ConfigureAwait(false);

                        var innerObserver = Create<TCollection>(
                            async y =>
                            {
                                var res = default(TResult);

                                try
                                {
                                    res = await resultSelector(x, y).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await observer.OnNextAsync(res).ConfigureAwait(false);
                                }
                            },
                            OnErrorAsync,
                            async () =>
                            {
                                await OnCompletedAsync().ConfigureAwait(false);

                                await disposable.RemoveAsync(inner).ConfigureAwait(false);
                            }
                        );

                        var innerSubscription = await collection.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                        await inner.AssignAsync(innerSubscription).ConfigureAwait(false);
                    },
                    OnErrorAsync,
                    OnCompletedAsync
                ),
                disposable
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, IAsyncObservable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, (x, i) => new ValueTask<IAsyncObservable<TResult>>(selector(x, i)), (x, i, y, j) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, selector, (x, i, y, j) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return SelectMany<TSource, TCollection, TResult>(observer, (x, i) => new ValueTask<IAsyncObservable<TCollection>>(collectionSelector(x, i)), (x, i, y, j) => new ValueTask<TResult>(resultSelector(x, i, y, j)));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, int, TCollection, int, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            async ValueTask<IAsyncObservable<(TCollection item, int i)>> collectionSelectorWithIndex((TSource item, int i) t) => (await collectionSelector(t.item, t.i).ConfigureAwait(false)).Select((item, i) => (item, i));
            ValueTask<TResult> resultSelectorWithIndex((TSource item, int i) outer, (TCollection item, int i) inner) => resultSelector(outer.item, outer.i, inner.item, inner.i);

            var (outerObserverWithIndex, disposable) = SelectMany(observer, collectionSelectorWithIndex, (Func<(TSource item, int i), (TCollection item, int i), ValueTask<TResult>>)resultSelectorWithIndex);

            var outerObserver = Select<TSource, (TSource item, int i)>(outerObserverWithIndex, (item, i) => (item, i));

            return (outerObserver, disposable);
        }
    }
}
