// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Concatenates the input sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence with the elements of the source sequences concatenated.</returns>
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Concat_();
        }

        /// <summary>
        /// Concatenates the input sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence with the elements of the source sequences concatenated.</returns>
        public static IEnumerable<TSource> Concat<TSource>(params IEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Concat_();
        }

        private static IEnumerable<TSource> Concat_<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            foreach (var source in sources)
                foreach (var item in source)
                    yield return item;
        }

        /// <summary>
        /// Projects each element of a sequence to an given sequence and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">First source sequence element type.</typeparam>
        /// <typeparam name="TOther">Second source sequence element type.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="other">Inner sequence each source sequenec element is projected onto.</param>
        /// <returns>Sequence flattening the sequences that result from projecting elements in the source sequence.</returns>
        public static IEnumerable<TOther> SelectMany<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (other == null)
                throw new ArgumentNullException("other");

            return source.SelectMany(_ => other);
        }

#if NO_ZIP
        /// <summary>
        /// Merges two sequences by applying the specified selector function on index-based corresponding element pairs from both sequences.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">Function to apply to each pair of elements from both sequences.</param>
        /// <returns>Sequence consisting of the result of pairwise application of the selector function over pairs of elements from the source sequences.</returns>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return Zip_(first, second, resultSelector);
        }

        private static IEnumerable<TResult> Zip_<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
                while (e1.MoveNext() && e2.MoveNext())
                    yield return resultSelector(e1.Current, e2.Current);
        }
#endif
    }
}
