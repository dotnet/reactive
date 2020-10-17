// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<IDictionary<TKey, TValue>> ToDictionary<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return CreateAsyncObservable<IDictionary<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToDictionary(observer, state.keySelector, state.valueSelector)));
        }

        public static IAsyncObservable<IDictionary<TKey, TValue>> ToDictionary<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IDictionary<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToDictionary(observer, state.keySelector, state.valueSelector, state.comparer))); ;
        }

        public static IAsyncObservable<IDictionary<TKey, TValue>> ToDictionary<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return CreateAsyncObservable<IDictionary<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToDictionary(observer, state.keySelector, state.valueSelector)));
        }

        public static IAsyncObservable<IDictionary<TKey, TValue>> ToDictionary<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<IDictionary<TKey, TValue>>.From(
                source,
                (keySelector, valueSelector, comparer),
                static (source, state, observer) => source.SubscribeSafeAsync(AsyncObserver.ToDictionary(observer, state.keySelector, state.valueSelector, state.comparer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ToDictionary<TSource, TKey, TValue>(IAsyncObserver<IDictionary<TKey, TValue>> observer, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return ToDictionary(observer, keySelector, valueSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToDictionary<TSource, TKey, TValue>(IAsyncObserver<IDictionary<TKey, TValue>> observer, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Aggregate<TSource, Dictionary<TKey, TValue>, IDictionary<TKey, TValue>>(
                observer,
                new Dictionary<TKey, TValue>(comparer),
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

        public static IAsyncObserver<TSource> ToDictionary<TSource, TKey, TValue>(IAsyncObserver<IDictionary<TKey, TValue>> observer, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return ToDictionary(observer, keySelector, valueSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToDictionary<TSource, TKey, TValue>(IAsyncObserver<IDictionary<TKey, TValue>> observer, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Aggregate<TSource, Dictionary<TKey, TValue>, IDictionary<TKey, TValue>>(
                observer,
                new Dictionary<TKey, TValue>(comparer),
                async (d, x) =>
                {
                    var key = await keySelector(x).ConfigureAwait(false);
                    var value = await valueSelector(x).ConfigureAwait(false);

                    d.Add(key, value);

                    return d;
                },
                d => new ValueTask<IDictionary<TKey, TValue>>(d)
            );
        }
    }
}
