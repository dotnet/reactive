// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<ILookup<TKey, TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector)));
        }

        public static IAsyncObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<ILookup<TKey, TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return CreateAsyncObservable<ILookup<TKey, TSource>>.From(
                source,
                keySelector,
                static (source, keySelector, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector)));
        }

        public static IAsyncObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<ILookup<TKey, TSource>>.From(
                source,
                (keySelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.comparer)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return CreateAsyncObservable<ILookup<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.valueSelector)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<ILookup<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.valueSelector, state.comparer)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return CreateAsyncObservable<ILookup<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.valueSelector)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<ILookup<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, state.keySelector, state.valueSelector, state.comparer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ToLookup<TSource, TKey>(IAsyncObserver<ILookup<TKey, TSource>> observer, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return ToLookup(observer, keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey>(IAsyncObserver<ILookup<TKey, TSource>> observer, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToLookup(observer, keySelector, x => x, comparer);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey>(IAsyncObserver<ILookup<TKey, TSource>> observer, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return ToLookup(observer, keySelector, x => new ValueTask<TSource>(x), EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey>(IAsyncObserver<ILookup<TKey, TSource>> observer, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToLookup(observer, keySelector, x => new ValueTask<TSource>(x), comparer);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return ToLookup(observer, keySelector, valueSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Aggregate<TSource, Lookup<TKey, TValue>, ILookup<TKey, TValue>>(
                observer,
                new Lookup<TKey, TValue>(comparer),
                (d, x) =>
                {
                    var key = keySelector(x);
                    var value = valueSelector(x);

                    d.Add(key, value);

                    return d;
                },
                d => d
            );
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return ToLookup(observer, keySelector, valueSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Aggregate<TSource, Lookup<TKey, TValue>, ILookup<TKey, TValue>>(
                observer,
                new Lookup<TKey, TValue>(comparer),
                async (d, x) =>
                {
                    var key = await keySelector(x).ConfigureAwait(false);
                    var value = await valueSelector(x).ConfigureAwait(false);

                    d.Add(key, value);

                    return d;
                },
                d => new ValueTask<ILookup<TKey, TValue>>(d)
            );
        }
    }
}
