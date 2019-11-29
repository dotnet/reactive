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
        /// <summary>
        /// Projects each element of an async-enumerable sequence into consecutive non-overlapping buffers which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="count">Length of each buffer.</param>
        /// <returns>An async-enumerable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than or equal to zero.</exception>
        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count <= 0)
                throw Error.ArgumentOutOfRange(nameof(count));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<IList<TSource>> Core(CancellationToken cancellationToken)
            {
                var buffer = new List<TSource>(count);

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    buffer.Add(item);

                    if (buffer.Count == count)
                    {
                        yield return buffer;

                        buffer = new List<TSource>(count);
                    }
                }

                if (buffer.Count > 0)
                {
                    yield return buffer;
                }
            }
        }

        /// <summary>
        /// Projects each element of an async-enumerable sequence into zero or more buffers which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="count">Length of each buffer.</param>
        /// <param name="skip">Number of elements to skip between creation of consecutive buffers.</param>
        /// <returns>An async-enumerable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> or <paramref name="skip"/> is less than or equal to zero.</exception>
        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count <= 0)
                throw Error.ArgumentOutOfRange(nameof(count));
            if (skip <= 0)
                throw Error.ArgumentOutOfRange(nameof(skip));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<IList<TSource>> Core(CancellationToken cancellationToken)
            {
                var buffers = new Queue<IList<TSource>>();

                var index = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (index++ % skip == 0)
                    {
                        buffers.Enqueue(new List<TSource>(count));
                    }

                    foreach (var buffer in buffers)
                    {
                        buffer.Add(item);
                    }

                    if (buffers.Count > 0 && buffers.Peek().Count == count)
                    {
                        yield return buffers.Dequeue();
                    }
                }

                while (buffers.Count > 0)
                {
                    yield return buffers.Dequeue();
                }
            }
        }
    }
}
