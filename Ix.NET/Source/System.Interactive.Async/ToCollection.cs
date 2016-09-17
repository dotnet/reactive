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
        public static Task<TSource[]> ToArray<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToArray(source, CancellationToken.None);
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var arrayProvider = source as IIListProvider<TSource>;
            if (arrayProvider != null)
                return arrayProvider.ToArrayAsync(cancellationToken);

            return AsyncEnumerableHelpers.ToArray(source, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToDictionary(source, keySelector, elementSelector, comparer, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return ToDictionary(source, keySelector, elementSelector, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToDictionary(source, keySelector, comparer, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return ToDictionary(source, keySelector, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Aggregate(new Dictionary<TKey, TElement>(comparer), (d, x) =>
                                                                              {
                                                                                  d.Add(keySelector(x), elementSelector(x));
                                                                                  return d;
                                                                              }, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.ToDictionary(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.ToDictionary(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ToDictionary(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToList(source, CancellationToken.None);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var listProvider = source as IIListProvider<TSource>;
            if (listProvider != null)
                return listProvider.ToListAsync(cancellationToken);

            return source.Aggregate(new List<TSource>(), (list, x) =>
                                                         {
                                                             list.Add(x);
                                                             return list;
                                                         }, cancellationToken);
        }
    }
}