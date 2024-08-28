// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
#if !(REFERENCE_ASSEMBLY && NET6_0_OR_GREATER)
        /// <summary>
        /// Returns the elements with the maximum key value by using the default comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        [Obsolete("Use MaxByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static IList<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MaxBy(source, keySelector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the elements with the minimum key value by using the specified comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <param name="comparer">Comparer used to determine the maximum key value.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        [Obsolete("Use MaxByWithTies to maintain same behavior with .NET 6 and later", false)]
        public static IList<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue));
        }
#endif
    }
}
