// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static Task<TSource> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core();

            async Task<TSource> Core()
            {
                if (comparer == null)
                {
                    comparer = Comparer<TSource>.Default;
                }

#if CSHARP8
                await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                        throw Error.NoElements();

                    var max = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;

                        if (comparer.Compare(cur, max) > 0)
                        {
                            max = cur;
                        }
                    }

                    return max;
                }
#else
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                        throw Error.NoElements();

                    var max = e.Current;

                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var cur = e.Current;

                        if (comparer.Compare(cur, max) > 0)
                        {
                            max = cur;
                        }
                    }

                    return max;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }
    }
}
