// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Distinct<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource>(this IAsyncObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(
                source,
                comparer,
                static (source, comparer, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, comparer)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, keySelector)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, keySelector)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, state.keySelector, state.comparer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Distinct<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Distinct(observer, EqualityComparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> Distinct<TSource>(IAsyncObserver<TSource> observer, IEqualityComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var set = new HashSet<TSource>(comparer);

            return Create<TSource>(
                async x =>
                {
                    var added = false;

                    try
                    {
                        added = set.Add(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (added)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        // The key-selector forms, as in Rx.NET: the key is computed and added to the set in one
        // step, so that a selector or comparer that throws ends the result at that element.
        public static IAsyncObserver<TSource> Distinct<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Distinct(observer, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> Distinct<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Distinct(observer, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> Distinct<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Distinct(observer, x => new ValueTask<TKey>(keySelector(x)), comparer);
        }

        public static IAsyncObserver<TSource> Distinct<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var set = new HashSet<TKey>(comparer);

            return Create<TSource>(
                async x =>
                {
                    var added = false;

                    try
                    {
                        var key = await keySelector(x).ConfigureAwait(false);

                        added = set.Add(key);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (added)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
