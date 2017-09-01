// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                return Empty<TSource>();
            }

            return new TakeLastAsyncIterator<TSource>(source, count);
        }

        private sealed class TakeLastAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private bool isDone;
            private Queue<TSource> queue;

            public TakeLastAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.count = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TakeLastAsyncIterator<TSource>(source, count);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                queue = null; // release the memory

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        queue = new Queue<TSource>();
                        isDone = false;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!isDone)
                            {
                                if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (count > 0)
                                    {
                                        var item = enumerator.Current;
                                        if (queue.Count >= count)
                                        {
                                            queue.Dequeue();
                                        }
                                        queue.Enqueue(item);
                                    }
                                }
                                else
                                {
                                    isDone = true;
                                    // Dispose early here as we can
                                    await enumerator.DisposeAsync().ConfigureAwait(false);
                                    enumerator = null;
                                }

                                continue; // loop until queue is drained
                            }

                            if (queue.Count > 0)
                            {
                                current = queue.Dequeue();
                                return true;
                            }

                            break; // while
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
