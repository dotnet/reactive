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
