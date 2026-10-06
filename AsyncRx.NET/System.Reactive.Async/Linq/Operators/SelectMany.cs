// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TOther> SelectMany<TSource, TOther>(this IAsyncObservable<TSource> source, IAsyncObservable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return SelectMany(source, _ => other);
        }

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, selector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, selector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.collectionSelector, state.resultSelector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.collectionSelector, state.resultSelector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, IAsyncObservable<TResult>> onNext, Func<Exception, IAsyncObservable<TResult>> onError, Func<IAsyncObservable<TResult>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return CreateAsyncObservable<TResult>.From(
                source,
                (onNext, onError, onCompleted),
                static async (source, state, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.onNext, state.onError, state.onCompleted);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, IAsyncObservable<TResult>> onNext, Func<Exception, IAsyncObservable<TResult>> onError, Func<IAsyncObservable<TResult>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return CreateAsyncObservable<TResult>.From(
                source,
                (onNext, onError, onCompleted),
                static async (source, state, observer) =>
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.onNext, state.onError, state.onCompleted);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.SelectMany(observer, state)));
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.SelectMany(observer, state)));
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
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
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector)));
        }

        public static IAsyncObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
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
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.SelectMany(observer, state.collectionSelector, state.resultSelector)));
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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, selector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, selector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.collectionSelector, state.resultSelector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

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
                    var subscription = new SingleAssignmentAsyncDisposable();

                    var (sink, inner) = AsyncObserver.SelectMany(observer, subscription, state.collectionSelector, state.resultSelector);

                    var outer = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);
                    await subscription.AssignAsync(outer).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, inner);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, IAsyncObservable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, subscription, x => new ValueTask<IAsyncObservable<TResult>>(selector(x)), (x, y) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, subscription, selector, (x, y) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return SelectMany<TSource, TCollection, TResult>(observer, subscription, x => new ValueTask<IAsyncObservable<TCollection>>(collectionSelector(x)), (x, y) => new ValueTask<TResult>(resultSelector(x, y)));
        }

        /// <summary>
        /// Creates an observer that projects each element it receives to an inner sequence and
        /// merges those sequences into <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to merge the projected sequences into.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence. The returned observer disposes it when the
        /// outer sequence completes while inner sequences are still active, so that the outer
        /// source is released as soon as nothing more can come from it, as Rx.NET does.
        /// </param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
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
                    OnOuterCompletedAsync
                ),
                disposable
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, int, IAsyncObservable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, subscription, (x, i) => new ValueTask<IAsyncObservable<TResult>>(selector(x, i)), (x, i, y, j) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, int, ValueTask<IAsyncObservable<TResult>>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, subscription, selector, (x, i, y, j) => new ValueTask<TResult>(y));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, int, IAsyncObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return SelectMany<TSource, TCollection, TResult>(observer, subscription, (x, i) => new ValueTask<IAsyncObservable<TCollection>>(collectionSelector(x, i)), (x, i, y, j) => new ValueTask<TResult>(resultSelector(x, i, y, j)));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, int, ValueTask<IAsyncObservable<TCollection>>> collectionSelector, Func<TSource, int, TCollection, int, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            async ValueTask<IAsyncObservable<(TCollection item, int i)>> collectionSelectorWithIndex((TSource item, int i) t) => (await collectionSelector(t.item, t.i).ConfigureAwait(false)).Select((item, i) => (item, i));
            ValueTask<TResult> resultSelectorWithIndex((TSource item, int i) outer, (TCollection item, int i) inner) => resultSelector(outer.item, outer.i, inner.item, inner.i);

            var (outerObserverWithIndex, disposable) = SelectMany(observer, subscription, collectionSelectorWithIndex, (Func<(TSource item, int i), (TCollection item, int i), ValueTask<TResult>>)resultSelectorWithIndex);

            var outerObserver = Select<TSource, (TSource item, int i)>(outerObserverWithIndex, (item, i) => (item, i));

            return (outerObserver, disposable);
        }

        /// <summary>
        /// Creates an observer that projects each element, the error, and the completion it
        /// receives to an inner sequence through the respective selector and merges those
        /// sequences into <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to merge the projected sequences into.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence. The returned observer disposes it when the
        /// outer sequence terminates while inner sequences are still active, so that the outer
        /// source is released as soon as nothing more can come from it, as Rx.NET does.
        /// </param>
        /// <param name="onNext">A transform function to apply to each element.</param>
        /// <param name="onError">
        /// A transform function to apply when an error occurs in the source sequence.
        /// </param>
        /// <param name="onCompleted">
        /// A transform function to apply when the end of the source sequence is reached.
        /// </param>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, IAsyncObservable<TResult>> onNext, Func<Exception, IAsyncObservable<TResult>> onError, Func<IAsyncObservable<TResult>> onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

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

            async ValueTask OnInnerCompletedAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (--count == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            };

            async ValueTask OnOuterTerminatedAsync()
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

            async ValueTask SubscribeInnerAsync(IAsyncObservable<TResult> collection)
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    count++;
                }

                var inner = new SingleAssignmentAsyncDisposable();

                await disposable.AddAsync(inner).ConfigureAwait(false);

                var innerObserver = Create<TResult>(
                    async y =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnNextAsync(y).ConfigureAwait(false);
                        }
                    },
                    OnErrorAsync,
                    async () =>
                    {
                        await OnInnerCompletedAsync().ConfigureAwait(false);

                        await disposable.RemoveAsync(inner).ConfigureAwait(false);
                    }
                );

                var innerSubscription = await collection.SubscribeSafeAsync(innerObserver).ConfigureAwait(false);

                await inner.AssignAsync(innerSubscription).ConfigureAwait(false);
            }

            // Each selector is applied outside the gate, and a selector that throws fails the
            // result; the outer sequence's error and completion both select an inner and then
            // count as the outer's termination.
            return
            (
                Create<TSource>(
                    async x =>
                    {
                        var collection = default(IAsyncObservable<TResult>);

                        try
                        {
                            collection = onNext(x);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await SubscribeInnerAsync(collection).ConfigureAwait(false);
                    },
                    async error =>
                    {
                        var collection = default(IAsyncObservable<TResult>);

                        try
                        {
                            collection = onError(error);
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await SubscribeInnerAsync(collection).ConfigureAwait(false);
                        await OnOuterTerminatedAsync().ConfigureAwait(false);
                    },
                    async () =>
                    {
                        var collection = default(IAsyncObservable<TResult>);

                        try
                        {
                            collection = onCompleted();
                        }
                        catch (Exception ex)
                        {
                            await OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }

                        await SubscribeInnerAsync(collection).ConfigureAwait(false);
                        await OnOuterTerminatedAsync().ConfigureAwait(false);
                    }
                ),
                disposable
            );
        }

        /// <summary>
        /// Creates an observer that projects each element with its index, the error, and the
        /// completion it receives to an inner sequence through the respective selector and
        /// merges those sequences into <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to merge the projected sequences into.</param>
        /// <param name="subscription">
        /// The subscription to the outer sequence, disposed when the outer sequence terminates
        /// while inner sequences are still active.
        /// </param>
        /// <param name="onNext">
        /// A transform function to apply to each element; the second parameter of the function
        /// represents the index of the source element.
        /// </param>
        /// <param name="onError">
        /// A transform function to apply when an error occurs in the source sequence.
        /// </param>
        /// <param name="onCompleted">
        /// A transform function to apply when the end of the source sequence is reached.
        /// </param>
        public static (IAsyncObserver<TSource>, IAsyncDisposable) SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, IAsyncDisposable subscription, Func<TSource, int, IAsyncObservable<TResult>> onNext, Func<Exception, IAsyncObservable<TResult>> onError, Func<IAsyncObservable<TResult>> onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            var index = -1;

            return SelectMany<TSource, TResult>(observer, subscription, x => onNext(x, checked(++index)), onError, onCompleted);
        }

        /// <summary>
        /// Creates an observer that projects each element it receives to an enumerable sequence
        /// and forwards that sequence's elements to <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to forward the projected elements to.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        public static IAsyncObserver<TSource> SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SelectMany<TSource, TResult, TResult>(observer, selector, (x, y) => y);
        }

        /// <summary>
        /// Creates an observer that projects each element it receives, with its index, to an
        /// enumerable sequence and forwards that sequence's elements to
        /// <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to forward the projected elements to.</param>
        /// <param name="selector">
        /// A transform function to apply to each element; the second parameter of the function
        /// represents the index of the source element.
        /// </param>
        public static IAsyncObserver<TSource> SelectMany<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var index = -1;

            return SelectMany<TSource, TResult, TResult>(observer, x => selector(x, checked(++index)), (x, y) => y);
        }

        /// <summary>
        /// Creates an observer that projects each element it receives, with its index, to an
        /// enumerable sequence and forwards each of that sequence's elements, with its index,
        /// through a result selector to <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to forward the projected elements to.</param>
        /// <param name="collectionSelector">
        /// A transform function to apply to each element; the second parameter of the function
        /// represents the index of the source element.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence; the
        /// second parameter of the function represents the index of the source element and the
        /// fourth parameter represents the index of the intermediate element.
        /// </param>
        public static IAsyncObserver<TSource> SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var index = -1;

            return SelectMany<TSource, (TCollection item, int j, int outerIndex), TResult>(
                observer,
                x =>
                {
                    var i = checked(++index);
                    return WithIndex(collectionSelector(x, i), i);
                },
                (x, y) => resultSelector(x, y.outerIndex, y.item, y.j));

            static IEnumerable<(TCollection item, int j, int outerIndex)> WithIndex(IEnumerable<TCollection> source, int outerIndex)
            {
                var j = 0;

                foreach (var item in source)
                {
                    yield return (item, checked(j++), outerIndex);
                }
            }
        }

        /// <summary>
        /// Creates an observer that projects each element it receives to an enumerable sequence
        /// and forwards each of that sequence's elements through a result selector to
        /// <paramref name="observer"/>.
        /// </summary>
        /// <param name="observer">The observer to forward the projected elements to.</param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        /// <remarks>
        /// The enumeration happens synchronously within the delivery of the source element, as
        /// in Rx.NET: a selector, <c>GetEnumerator</c>, <c>MoveNext</c>, <c>Current</c> or result
        /// selector that throws ends the result with that error.
        /// </remarks>
        public static IAsyncObserver<TSource> SelectMany<TSource, TCollection, TResult>(IAsyncObserver<TResult> observer, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create<TSource>(
                async x =>
                {
                    var enumerator = default(IEnumerator<TCollection>);

                    try
                    {
                        enumerator = collectionSelector(x).GetEnumerator();
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    using (enumerator)
                    {
                        while (true)
                        {
                            var result = default(TResult);

                            try
                            {
                                if (!enumerator.MoveNext())
                                {
                                    break;
                                }

                                result = resultSelector(x, enumerator.Current);
                            }
                            catch (Exception ex)
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                return;
                            }

                            await observer.OnNextAsync(result).ConfigureAwait(false);
                        }
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
