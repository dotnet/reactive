// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Projects each element of a sequence to an given sequence and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">First source sequence element type.</typeparam>
        /// <typeparam name="TOther">Second source sequence element type.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="other">Inner sequence each source sequence element is projected onto.</param>
        /// <returns>Sequence flattening the sequences that result from projecting elements in the source sequence.</returns>
        public static IEnumerable<TOther> SelectMany<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return source.SelectMany(_ => other);
        }
    }
}
