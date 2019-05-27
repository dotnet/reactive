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
        /// Returns a specified number of contiguous elements from the end of the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">The number of elements to take from the end of the sequence.</param>
        /// <returns>Sequence with the specified number of elements counting from the end of the source sequence.</returns>
        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return TakeLastCore(source, count);
        }
#endif

        private static IEnumerable<TSource> TakeLastCore<TSource>(IEnumerable<TSource> source, int count)
        {
            if (count == 0)
            {
                yield break;
            }

            var q = new Queue<TSource>(count);

            foreach (var item in source)
            {
                if (q.Count >= count)
                {
                    q.Dequeue();
                }

                q.Enqueue(item);
            }

            while (q.Count > 0)
            {
                yield return q.Dequeue();
            }
        }
    }
}
