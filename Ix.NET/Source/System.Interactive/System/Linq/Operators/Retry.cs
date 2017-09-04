// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new[] { source }.Repeat().Catch();
        }

        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs, with the specified
        /// maximum number of retries.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="retryCount">Maximum number of retries.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            return new[] { source }.Repeat(retryCount).Catch();
        }
    }
}
