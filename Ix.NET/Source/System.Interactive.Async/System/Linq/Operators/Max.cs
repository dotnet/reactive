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
        public static ValueTask<TSource> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, comparer, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> _source, IComparer<TSource> _comparer, CancellationToken _cancellationToken)
            {
                if (_comparer == null)
                {
                    _comparer = Comparer<TSource>.Default;
                }

                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // TODO: Switch to `await using` in preview 3 (https://github.com/dotnet/roslyn/pull/32731)
                {
                    if (!await e.MoveNextAsync())
                        throw Error.NoElements();

                    var max = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        var cur = e.Current;

                        if (_comparer.Compare(cur, max) > 0)
                        {
                            max = cur;
                        }
                    }

                    return max;
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
        }
    }
}
