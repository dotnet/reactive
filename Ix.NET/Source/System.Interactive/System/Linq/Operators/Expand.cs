// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Expands the sequence by recursively applying a selector function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function to retrieve the next sequence to expand.</param>
        /// <returns>Sequence with results from the recursive expansion of the source sequence.</returns>
        public static IEnumerable<TSource> Expand<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return ExpandCore(source, selector);
        }

        private static IEnumerable<TSource> ExpandCore<TSource>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector)
        {
            var queue = new Queue<IEnumerable<TSource>>();
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var src = queue.Dequeue();

                foreach (var item in src)
                {
                    queue.Enqueue(selector(item));
                    yield return item;
                }
            }
        }
    }
}
