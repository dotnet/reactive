// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        /// <summary>
        /// Split the elements of an async-enumerable sequence into chunks of size at most <paramref name="size"/>.
        /// </summary>
        /// <remarks>
        /// Every chunk except the last will be of size <paramref name="size"/>.
        /// The last chunk will contain the remaining elements and may be of a smaller size.
        /// </remarks>
        /// <param name="source">
        /// An <see cref="IAsyncEnumerable{T}"/> whose elements to chunk.
        /// </param>
        /// <param name="size">
        /// Maximum size of each chunk.
        /// </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size"/> is below 1.
        /// </exception>
        public static async IAsyncEnumerable<TSource[]> ChunkAsync<TSource>(this IAsyncEnumerable<TSource> source, int size, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (size < 1)
                throw Error.ArgumentOutOfRange(nameof(size));

            await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

            // Before allocating anything, make sure there's at least one element.
            if (await e.MoveNextAsync())
            {
                // Now that we know we have at least one item, allocate an initial storage array. This is not
                // the array we'll yield.  It starts out small in order to avoid significantly overallocating
                // when the source has many fewer elements than the chunk size.
                var arraySize = Math.Min(size, 4);
                int i;
                do
                {
                    var array = new TSource[arraySize];

                    // Store the first item.
                    array[0] = e.Current;
                    i = 1;

                    if (size != array.Length)
                    {
                        // This is the first chunk. As we fill the array, grow it as needed.
                        for (; i < size && await e.MoveNextAsync(); i++)
                        {
                            if (i >= array.Length)
                            {
                                arraySize = (int)Math.Min((uint)size, 2 * (uint)array.Length);
                                Array.Resize(ref array, arraySize);
                            }

                            array[i] = e.Current;
                        }
                    }
                    else
                    {
                        // For all but the first chunk, the array will already be correctly sized.
                        // We can just store into it until either it's full or MoveNext returns false.
                        var local = array; // avoid bounds checks by using cached local (`array` is lifted to iterator object as a field)
                        Debug.Assert(local.Length == size);
                        for (; (uint)i < (uint)local.Length && await e.MoveNextAsync(); i++)
                        {
                            local[i] = e.Current;
                        }
                    }

                    if (i != array.Length)
                    {
                        Array.Resize(ref array, i);
                    }

                    yield return array;
                }
                while (i >= size && await e.MoveNextAsync());
            }
        }
    }
}
