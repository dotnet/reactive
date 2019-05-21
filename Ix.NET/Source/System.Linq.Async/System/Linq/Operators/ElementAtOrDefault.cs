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
        public static ValueTask<TSource> ElementAtOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, index, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
            {
                if (source is IAsyncPartition<TSource> p)
                {
                    var first = await p.TryGetElementAtAsync(index, cancellationToken).ConfigureAwait(false);

                    if (first.HasValue)
                    {
                        return first.Value;
                    }
                }

                if (index >= 0)
                {
                    if (source is IList<TSource> list)
                    {
                        if (index < list.Count)
                        {
                            return list[index];
                        }
                    }
                    else
                    {
                        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                        {
                            if (index == 0)
                            {
                                return item;
                            }

                            index--;
                        }
                    }
                }

                return default;
            }
        }
    }
}
