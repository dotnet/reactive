// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                // Return source if not actually skipping, but only if it's a type from here, to avoid
                // issues if collections are used as keys or otherwise must not be aliased.
                if (source is AsyncIterator<TSource> || source is IAsyncPartition<TSource>)
                {
                    return source;
                }

                count = 0;
            }
            else if (source is IAsyncPartition<TSource> partition)
            {
                return partition.Skip(count);
            }
            else if (source is IList<TSource> list)
            {
                return new AsyncListPartition<TSource>(list, count, int.MaxValue);
            }

            return new AsyncEnumerablePartition<TSource>(source, count, -1);
        }
    }
}
