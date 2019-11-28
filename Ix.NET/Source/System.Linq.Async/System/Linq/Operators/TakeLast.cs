// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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
        /// Returns a specified number of contiguous elements from the end of an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to take from the end of the source sequence.</param>
        /// <returns>An async-enumerable sequence containing the specified number of elements from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements <paramref name="count"/> elements. Upon completion of
        /// the source sequence, this buffer is drained on the result sequence. This causes the elements to be delayed.
        /// </remarks>
        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (count <= 0)
            {
                return Empty<TSource>();
            }

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                Queue<TSource> queue;

                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    queue = new Queue<TSource>();
                    queue.Enqueue(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        if (queue.Count < count)
                        {
                            queue.Enqueue(e.Current);
                        }
                        else
                        {
                            do
                            {
                                queue.Dequeue();
                                queue.Enqueue(e.Current);
                            }
                            while (await e.MoveNextAsync());
                            break;
                        }
                    }
                }

                Debug.Assert(queue.Count <= count);
                do
                {
                    yield return queue.Dequeue();
                }
                while (queue.Count > 0);
            }
        }
    }
}
