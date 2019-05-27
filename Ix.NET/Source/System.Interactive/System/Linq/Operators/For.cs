// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Generates a sequence by enumerating a source sequence, mapping its elements on result sequences, and concatenating
        /// those sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="resultSelector">Result selector to evaluate for each iteration over the source.</param>
        /// <returns>
        /// Sequence concatenating the inner sequences that result from evaluating the result selector on elements from
        /// the source.
        /// </returns>
        public static IEnumerable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(resultSelector).Concat();
        }
    }
}
