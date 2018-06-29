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
        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return new BufferAsyncIterator<TSource>(source, count, count);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return new BufferAsyncIterator<TSource>(source, count, skip);
        }

        private sealed class BufferAsyncIterator<TSource> : AsyncIterator<IList<TSource>>
        {
            private readonly int count;
            private readonly int skip;
            private readonly IAsyncEnumerable<TSource> source;

            private Queue<IList<TSource>> buffers;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;
            private bool stopped;

            public BufferAsyncIterator(IAsyncEnumerable<TSource> source, int count, int skip)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.count = count;
                this.skip = skip;
            }

            public override AsyncIterator<IList<TSource>> Clone()
            {
                return new BufferAsyncIterator<TSource>(source, count, skip);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                buffers = null;

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        buffers = new Queue<IList<TSource>>();
                        index = 0;
                        stopped = false;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!stopped)
                            {
                                if (await enumerator.MoveNext(cancellationToken)
                                                    .ConfigureAwait(false))
                                {
                                    var item = enumerator.Current;
                                    if (index++ % skip == 0)
                                    {
                                        buffers.Enqueue(new List<TSource>(count));
                                    }

                                    foreach (var buffer in buffers)
                                    {
                                        buffer.Add(item);
                                    }

                                    if (buffers.Count > 0 && buffers.Peek()
                                                                    .Count == count)
                                    {
                                        current = buffers.Dequeue();
                                        return true;
                                    }

                                    continue; // loop
                                }
                                stopped = true;
                                enumerator.Dispose();
                                enumerator = null;

                                continue; // loop
                            }

                            if (buffers.Count > 0)
                            {
                                current = buffers.Dequeue();
                                return true;
                            }

                            break; // exit the while
                        }

                        break; // case
                }

                Dispose();
                return false;
            }
        }
    }
}