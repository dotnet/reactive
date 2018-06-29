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
        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new ExpandAsyncIterator<TSource>(source, selector);
        }

        private sealed class ExpandAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, IAsyncEnumerable<TSource>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            private Queue<IAsyncEnumerable<TSource>> queue;

            public ExpandAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ExpandAsyncIterator<TSource>(source, selector);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                queue = null;

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        queue = new Queue<IAsyncEnumerable<TSource>>();
                        queue.Enqueue(source);

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (enumerator == null)
                            {
                                if (queue.Count > 0)
                                {
                                    var src = queue.Dequeue();

                                    enumerator?.Dispose();
                                    enumerator = src.GetEnumerator();

                                    continue; // loop
                                }

                                break; // while
                            }

                            if (await enumerator.MoveNext(cancellationToken)
                                                .ConfigureAwait(false))
                            {
                                var item = enumerator.Current;
                                var next = selector(item);
                                queue.Enqueue(next);
                                current = item;
                                return true;
                            }
                            enumerator.Dispose();
                            enumerator = null;
                        }

                        break; // case
                }

                Dispose();
                return false;
            }
        }
    }
}