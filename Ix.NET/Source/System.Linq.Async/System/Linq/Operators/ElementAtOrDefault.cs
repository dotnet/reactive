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
        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ElementAtOrDefault(source, index, CancellationToken.None);
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ElementAtOrDefaultCore(source, index, cancellationToken);
        }

        private static async Task<TSource> ElementAtOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source is IList<TSource> list)
            {
                if (index < list.Count)
                {
                    return list[index];
                }
            }
            else if (source is IAsyncPartition<TSource> p)
            {
                var first = await p.TryGetElementAsync(index, cancellationToken).ConfigureAwait(false);

                if (first.HasValue)
                {
                    return first.Value;
                }
            }
            else
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

            return default;
        }
    }
}
