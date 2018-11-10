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
        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return ToListCore(source, CancellationToken.None);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return ToListCore(source, cancellationToken);
        }

        private static Task<List<TSource>> ToListCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is IAsyncIListProvider<TSource> listProvider)
                return listProvider.ToListAsync(cancellationToken);

            return Core();

            async Task<List<TSource>> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    var list = new List<TSource>();

                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        list.Add(e.Current);
                    }

                    return list;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
