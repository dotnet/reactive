// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // REVIEW: This is really an n-ary Prepend. Should we add n-ary overloads of Append and Prepend as well?
        //         If so, likely in Ix rather than System.Linq.Async.

        /// <summary>
        /// Prepends a sequence of values to an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend values to.</param>
        /// <param name="values">Values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="values"/> is null.</exception>
        public static IAsyncEnumerable<TSource> StartWith<TSource>(this IAsyncEnumerable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (values == null)
                throw Error.ArgumentNull(nameof(values));

            return values.ToAsyncEnumerable().Concat(source);
        }
    }
}
