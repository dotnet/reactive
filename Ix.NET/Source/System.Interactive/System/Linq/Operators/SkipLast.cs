// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
#if !(REFERENCE_ASSEMBLY && (NETCOREAPP2_0 || NETSTANDARD2_1))
        /// <summary>
        /// Bypasses a specified number of contiguous elements from the end of the sequence and returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">
        /// The number of elements to skip from the end of the sequence before returning the remaining
        /// elements.
        /// </param>
        /// <returns>Sequence bypassing the specified number of elements counting from the end of the source sequence.</returns>
        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return SkipLastCore(source, count);
        }
#endif

        private static IEnumerable<TSource> SkipLastCore<TSource>(this IEnumerable<TSource> source, int count)
        {
            var q = new Queue<TSource>();

            foreach (var x in source)
            {
                q.Enqueue(x);

                if (q.Count > count)
                {
                    yield return q.Dequeue();
                }
            }
        }
    }
}
