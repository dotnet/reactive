// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                // Return source if not actually skipping, but only if it's a type from here, to avoid
                // issues if collections are used as keys or otherwise must not be aliased.
                if (source is AsyncIterator<TSource>)
                {
                    return source;
                }

                count = 0;
            }

            return new SkipAsyncIterator<TSource>(source, count);
        }

        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                // Return source if not actually skipping, but only if it's a type from here, to avoid
                // issues if collections are used as keys or otherwise must not be aliased.
                if (source is AsyncIterator<TSource>)
                {
                    return source;
                }

                count = 0;
            }

            return new SkipLastAsyncIterator<TSource>(source, count);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
        }

        private sealed class SkipAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly IAsyncEnumerable<TSource> source;
            private int currentCount;
            private IAsyncEnumerator<TSource> enumerator;

            public SkipAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                this.source = source;
                this.count = count;
                currentCount = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipAsyncIterator<TSource>(source, count);
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

                        // skip elements as requested
                        while (currentCount > 0 && await enumerator.MoveNext(cancellationToken)
                                                                   .ConfigureAwait(false))
                        {
                            currentCount--;
                        }
                        if (currentCount <= 0)
                        {
                            state = AsyncIteratorState.Iterating;
                            goto case AsyncIteratorState.Iterating;
                        }
                        break;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SkipLastAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly IAsyncEnumerable<TSource> source;
            private IAsyncEnumerator<TSource> enumerator;
            private Queue<TSource> queue;

            public SkipLastAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                this.source = source;
                this.count = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipLastAsyncIterator<TSource>(source, count);
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

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            queue.Enqueue(item);
                            if (queue.Count > count)
                            {
                                current = queue.Dequeue();
                                return true;
                            }
                            goto case AsyncIteratorState.Iterating; // loop until either the await is false or we return an item
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SkipWhileAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;

            public SkipWhileAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileAsyncIterator<TSource>(source, predicate);
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

                        // skip elements as requested
                        while (await enumerator.MoveNext(cancellationToken)
                                               .ConfigureAwait(false))
                        {
                            var element = enumerator.Current;
                            if (!predicate(element))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }
                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }
                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }

        private sealed class SkipWhileWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> predicate;
            private readonly IAsyncEnumerable<TSource> source;

            private bool doMoveNext;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SkipWhileWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
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

                        // skip elements as requested
                        while (await enumerator.MoveNext(cancellationToken)
                                               .ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }

                            var element = enumerator.Current;
                            if (!predicate(element, index))
                            {
                                doMoveNext = false;
                                state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }
                        break;

                    case AsyncIteratorState.Iterating:
                        if (doMoveNext && await enumerator.MoveNext(cancellationToken)
                                                          .ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }
                        if (!doMoveNext)
                        {
                            current = enumerator.Current;
                            doMoveNext = true;
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