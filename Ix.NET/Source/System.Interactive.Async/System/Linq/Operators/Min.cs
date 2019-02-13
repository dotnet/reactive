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
        public static Task<TSource> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, comparer, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, IComparer<TSource> _comparer, CancellationToken _cancellationToken)
            {
                if (_comparer == null)
                {
                    _comparer = Comparer<TSource>.Default;
                }

                var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                        throw Error.NoElements();

                    var min = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;

                        if (_comparer.Compare(cur, min) < 0)
                        {
                            min = cur;
                        }
                    }

                    return min;
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
        }
    }
}
