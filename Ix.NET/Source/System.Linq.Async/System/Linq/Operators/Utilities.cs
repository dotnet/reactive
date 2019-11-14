// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal static class Utilities
    {
        public static async ValueTask AddRangeAsync<T>(this List<T> list, IAsyncEnumerable<T> collection, CancellationToken cancellationToken)
        {
            if (collection is IEnumerable<T> enumerable)
            {
                list.AddRange(enumerable);
                return;
            }

            if (collection is IAsyncIListProvider<T> listProvider)
            {
                var count = await listProvider.GetCountAsync(onlyIfCheap: true, cancellationToken).ConfigureAwait(false);

                if (count == 0)
                {
                    return;
                }

                if (count > 0)
                {
                    var newCount = list.Count + count;

                    if (list.Capacity < newCount)
                    {
                        list.Capacity = newCount;
                    }
                }
            }

            await foreach (var item in collection.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                list.Add(item);
            }
        }
    }
}
