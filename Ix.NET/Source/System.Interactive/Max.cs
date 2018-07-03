// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        ///     Returns the maximum value in the enumerable sequence by using the specified comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to determine the maximum value.</param>
        /// <returns>Maximum value in the sequence.</returns>
        public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return Extrema(source, x => x, comparer, 1);
        }

        /// <summary>
        ///     Returns the elements with the maximum key value by using the default comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        public static IList<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return MaxBy(source, keySelector, Comparer<TKey>.Default);
        }

        /// <summary>
        ///     Returns the elements with the minimum key value by using the specified comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <param name="comparer">Comparer used to determine the maximum key value.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        public static IList<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return ExtremaBy(source, keySelector, comparer, 1);
        }

        private static TSource Extrema<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> compare, int direction)
        {
            var result = default(TSource);

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence doesn't contain any elements.");
                }

                var current = e.Current;
                var resKey = keySelector(current);
                result = current;

                while (e.MoveNext())
                {
                    var cur = e.Current;
                    var key = keySelector(cur);

                    var cmp = compare.Compare(key, resKey) * direction;
                    if (cmp == 0)
                    {
                        result = cur;
                    }
                    else if (cmp > 0)
                    {
                        result = cur;
                        resKey = key;
                    }
                }
            }

            return result;
        }

        private static IList<TSource> ExtremaBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> compare, int direction)
        {
            var result = new List<TSource>();

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence doesn't contain any elements.");
                }

                var current = e.Current;
                var resKey = keySelector(current);
                result.Add(current);

                while (e.MoveNext())
                {
                    var cur = e.Current;
                    var key = keySelector(cur);

                    var cmp = compare.Compare(key, resKey) * direction;
                    if (cmp == 0)
                    {
                        result.Add(cur);
                    }
                    else if (cmp > 0)
                    {
                        result = new List<TSource>
                        {
                            cur
                        };
                        resKey = key;
                    }
                }
            }

            return result;
        }
    }
}
