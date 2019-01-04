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
        public static Task<TSource> ElementAtAsync<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core();

            async Task<TSource> Core()
            {
                if (source is IAsyncPartition<TSource> p)
                {
                    var first = await p.TryGetElementAtAsync(index, cancellationToken).ConfigureAwait(false);

                    if (first.HasValue)
                    {
                        return first.Value;
                    }
                }
                else
                {
                    if (source is IList<TSource> list)
                    {
                        return list[index];
                    }

                    if (index >= 0)
                    {
                        var e = source.GetAsyncEnumerator(cancellationToken);

                        try
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (index == 0)
                                {
                                    return e.Current;
                                }

                                index--;
                            }
                        }
                        finally
                        {
                            await e.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }

                throw Error.ArgumentOutOfRange(nameof(index));
            }
        }
    }
}
