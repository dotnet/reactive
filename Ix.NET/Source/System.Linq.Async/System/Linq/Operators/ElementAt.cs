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
        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">async-enumerable sequence to return the element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence that produces the element at the specified position in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">(Asynchronous) <paramref name="index"/> is greater than or equal to the number of elements in the source sequence.</exception>
        public static ValueTask<TSource> ElementAtAsync<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken = default)
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
                else
                {
                    if (source is IList<TSource> list)
                    {
                        return list[index];
                    }

                    if (index >= 0)
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

                // NB: Even though index is captured, no closure is created.
                //     The nameof expression is lowered to a literal prior to creating closures.

                throw Error.ArgumentOutOfRange(nameof(index));
            }
        }
    }
}
