// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
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
        public static async IAsyncEnumerable<TSource[]> ChunkAsync<TSource>(this IAsyncEnumerable<TSource> source, int size)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (size < 1)
                throw Error.ArgumentOutOfRange(nameof(size));

            await using IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();

            if (await e.MoveNextAsync())
            {
                List<TSource> chunkBuilder = new();
                while (true)
                {
                    do
                    {
                        chunkBuilder.Add(e.Current);
                    }
                    while (chunkBuilder.Count < size && await e.MoveNextAsync());

                    yield return chunkBuilder.ToArray();

                    if (chunkBuilder.Count < size || !await e.MoveNextAsync())
                    {
                        yield break;
                    }
                    chunkBuilder.Clear();
                }
            }
        }
    }
}
