// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TAccumulate">Accumulation type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Accumulator seed value.</param>
        /// <param name="accumulator">
        /// Accumulation function to apply to the current accumulation value and each element of the
        /// sequence.
        /// </param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return ScanCore(source, seed, accumulator);
        }

        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="accumulator">
        /// Accumulation function to apply to the current accumulation value and each element of the
        /// sequence.
        /// </param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return ScanCore(source, accumulator);
        }

        private static IEnumerable<TAccumulate> ScanCore<TSource, TAccumulate>(IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            var acc = seed;

            foreach (var item in source)
            {
                acc = accumulator(acc, item);

                yield return acc;
            }
        }

        private static IEnumerable<TSource> ScanCore<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            var hasSeed = false;
            var acc = default(TSource)!;

            foreach (var item in source)
            {
                if (!hasSeed)
                {
                    hasSeed = true;
                    acc = item;
                    continue;
                }

                acc = accumulator(acc, item);

                yield return acc;
            }
        }
    }
}
