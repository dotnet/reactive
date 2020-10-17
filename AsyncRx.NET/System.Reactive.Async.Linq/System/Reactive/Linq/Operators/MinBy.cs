// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<IList<TSource>> MinBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.MinBy(observer, keySelector)));
        }

        public static IAsyncObservable<IList<TSource>> MinBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.MinBy(observer, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<IList<TSource>> MinBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.MinBy(observer, keySelector)));
        }

        public static IAsyncObservable<IList<TSource>> MinBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IList<TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.MinBy(observer, state.keySelector, state.comparer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> MinBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MinBy(observer, x => new ValueTask<TKey>(keySelector(x)), Comparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> MinBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return MinBy(observer, x => new ValueTask<TKey>(keySelector(x)), comparer);
        }

        public static IAsyncObserver<TSource> MinBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MinBy(observer, keySelector, Comparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> MinBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var hasValue = false;
            var lastKey = default(TKey);
            var list = new List<TSource>();

            return Create<TSource>(
                async x =>
                {
                    var key = default(TKey);
                    try
                    {
                        key = await keySelector(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    var comparison = 0;

                    if (!hasValue)
                    {
                        hasValue = true;
                        lastKey = key;
                    }
                    else
                    {
                        try
                        {
                            comparison = comparer.Compare(key, lastKey);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (comparison < 0)
                    {
                        lastKey = key;
                        list.Clear();
                    }

                    if (comparison <= 0)
                    {
                        list.Add(x);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(list).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
