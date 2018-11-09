// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

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
    }
}
