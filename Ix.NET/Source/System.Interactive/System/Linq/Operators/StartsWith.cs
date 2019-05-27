// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns the source sequence prefixed with the specified value.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="values">Values to prefix the sequence with.</param>
        /// <returns>Sequence starting with the specified prefix value, followed by the source sequence.</returns>
        public static IEnumerable<TSource> StartWith<TSource>(this IEnumerable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return StartWithCore(source, values);
        }

        private static IEnumerable<TSource> StartWithCore<TSource>(IEnumerable<TSource> source, params TSource[] values)
        {
            foreach (var x in values)
            {
                yield return x;
            }

            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}
