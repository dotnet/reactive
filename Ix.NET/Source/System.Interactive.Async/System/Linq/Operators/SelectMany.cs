// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Projects each element of the source observable sequence to the other observable sequence and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence and the elements in the result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="other">An observable sequence to project each element from the source sequence onto.</param>
        /// <returns>An observable sequence whose elements are the result of projecting each source element onto the other sequence and merging all the resulting sequences together.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        public static IAsyncEnumerable<TOther> SelectMany<TSource, TOther>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (other == null)
                throw Error.ArgumentNull(nameof(other));

            return source.SelectMany(_ => other);
        }
    }
}
