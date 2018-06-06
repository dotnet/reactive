// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return Create<ILookup<TKey, TValue>>(observer => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector, valueSelector)));
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

            return Create<ILookup<TKey, TValue>>(observer => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector, valueSelector, comparer)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TValue>> valueSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return Create<ILookup<TKey, TValue>>(observer => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector, valueSelector)));
        }

        public static IAsyncObservable<ILookup<TKey, TValue>> ToLookup<TSource, TKey, TValue>(this IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<ILookup<TKey, TValue>>(observer => source.SubscribeSafeAsync(AsyncObserver.ToLookup(observer, keySelector, valueSelector, comparer)));
        }
    }

    partial class AsyncObserver
    {
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

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TValue>> valueSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            return ToLookup(observer, keySelector, valueSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> ToLookup<TSource, TKey, TValue>(IAsyncObserver<ILookup<TKey, TValue>> observer, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TValue>> valueSelector, IEqualityComparer<TKey> comparer)
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
                d => Task.FromResult<ILookup<TKey, TValue>>(d)
            );
        }
    }
}
