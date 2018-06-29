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
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                return Empty<TSource>();
            }

            return new TakeAsyncIterator<TSource>(source, count);
        }

        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                return Empty<TSource>();
            }

            return new TakeLastAsyncIterator<TSource>(source, count);
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new TakeWhileAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return new TakeWhileWithIndexAsyncIterator<TSource>(source, predicate);
        }

        private sealed class TakeAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly IAsyncEnumerable<TSource> source;

            private int currentCount;
            private IAsyncEnumerator<TSource> enumerator;

            public TakeAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.count = count;
                currentCount = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TakeAsyncIterator<TSource>(source, count);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }


            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (currentCount > 0 && await enumerator.MoveNext(cancellationToken)
                                                                .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            currentCount--;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
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

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }
                queue = null; // release the memory

                base.Dispose();
            }


            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        queue = new Queue<TSource>();
                        isDone = false;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!isDone)
                            {
                                if (await enumerator.MoveNext(cancellationToken)
                                                    .ConfigureAwait(false))
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
                                    enumerator.Dispose();
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

                Dispose();
                return false;
            }
        }

        private sealed class TakeWhileAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            public TakeWhileAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TakeWhileAsyncIterator<TSource>(source, predicate);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            if (!predicate(item))
                            {
                                break;
                            }
                            current = item;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class TakeWhileWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public TakeWhileWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TakeWhileWithIndexAsyncIterator<TSource>(source, predicate);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        index = -1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            checked
                            {
                                index++;
                            }

                            if (!predicate(item, index))
                            {
                                break;
                            }
                            current = item;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }
    }
}