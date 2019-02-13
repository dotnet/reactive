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

            return Core(source, index, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, int _index, CancellationToken _cancellationToken)
            {
                if (_source is IAsyncPartition<TSource> p)
                {
                    var first = await p.TryGetElementAtAsync(_index, _cancellationToken).ConfigureAwait(false);

                    if (first.HasValue)
                    {
                        return first.Value;
                    }
                }
                else
                {
                    if (_source is IList<TSource> list)
                    {
                        return list[_index];
                    }

                    if (_index >= 0)
                    {
                        await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                        {
                            if (_index == 0)
                            {
                                return item;
                            }

                            _index--;
                        }
                    }
                }

                // NB: Even though index is captured, no closure is created.
                //     The nameof expression is lowered to a literal prior to creating closures.

                throw Error.ArgumentOutOfRange(nameof(index));
            }
        }
    }
}
