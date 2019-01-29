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
        public static Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (source is IAsyncIListProvider<TSource> listProvider)
                return listProvider.ToListAsync(cancellationToken).AsTask();

            return Core(source, cancellationToken);

            static async Task<List<TSource>> Core(IAsyncEnumerable<TSource> _source, CancellationToken _cancellationToken)
            {
                var list = new List<TSource>();

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    list.Add(item);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        list.Add(e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return list;
            }
        }
    }
}
