// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns consecutive distinct elements by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DistinctUntilChangedCore(source, x => x, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Returns consecutive distinct elements by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to compare values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(source, x => x, comparer);
        }

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var currentKey = default(TKey)!;
            var hasCurrentKey = false;

            foreach (var item in source)
            {
                var key = keySelector(item);

                var comparerEquals = false;
                if (hasCurrentKey)
                {
                    comparerEquals = comparer.Equals(currentKey, key);
                }

                if (!hasCurrentKey || !comparerEquals)
                {
                    hasCurrentKey = true;
                    currentKey = key;

                    yield return item;
                }
            }
        }

    }
}
