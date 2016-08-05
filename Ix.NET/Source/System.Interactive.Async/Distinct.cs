// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Defer(() =>
                         {
                             var set = new Set<TKey>(comparer);
                             return source.Where(item => set.Add(keySelector(item)));
                         });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Distinct(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Defer(() =>
                         {
                             var set = new HashSet<TSource>(comparer);
                             return source.Where(set.Add);
                         });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Distinct(EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.DistinctUntilChanged_(x => x, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.DistinctUntilChanged_(x => x, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctUntilChanged_(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.DistinctUntilChanged_(keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChanged_<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var currentKey = default(TKey);
                    var hasCurrentKey = false;
                    var current = default(TSource);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (await e.MoveNext(ct)
                                       .ConfigureAwait(false))
                            {
                                var item = e.Current;
                                var key = default(TKey);
                                var comparerEquals = false;

                                key = keySelector(item);

                                if (hasCurrentKey)
                                {
                                    comparerEquals = comparer.Equals(currentKey, key);
                                }

                                if (!hasCurrentKey || !comparerEquals)
                                {
                                    hasCurrentKey = true;
                                    currentKey = key;

                                    current = item;
                                    return true;
                                }
                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            return false;
                        };

                    return CreateEnumerator(
                        f,
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }
    }
}