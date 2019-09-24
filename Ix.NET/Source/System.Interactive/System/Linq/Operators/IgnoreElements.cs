// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Ignores all elements in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Source sequence without its elements.</returns>
        public static IEnumerable<TSource> IgnoreElements<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return IgnoreElementsCore(source);
        }

        private static IEnumerable<TSource> IgnoreElementsCore<TSource>(IEnumerable<TSource> source)
        {
            foreach (var _ in source)
                ;

            yield break;
        }
    }
}
