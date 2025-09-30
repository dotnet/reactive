﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.take?view=net-9.0-pp#system-linq-asyncenumerable-take-1(system-collections-generic-iasyncenumerable((-0))-system-int32)

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>An async-enumerable sequence that contains the specified number of elements from the start of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (count <= 0)
            {
                return Empty<TSource>();
            }
            else if (source is IAsyncPartition<TSource> partition)
            {
                return partition.Take(count);
            }
            else if (source is IList<TSource> list)
            {
                return new AsyncListPartition<TSource>(list, 0, count - 1);
            }

            return new AsyncEnumerablePartition<TSource>(source, 0, count - 1);
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
    }
}
