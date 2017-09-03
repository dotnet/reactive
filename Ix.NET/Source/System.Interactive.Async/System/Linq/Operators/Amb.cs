// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Amb<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new AmbAsyncIterator<TSource>(first, second);
        }

        private sealed class AmbAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> first;
            private readonly IAsyncEnumerable<TSource> second;

            private IAsyncEnumerator<TSource> enumerator;

            public AmbAsyncIterator(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);

                this.first = first;
                this.second = second;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AmbAsyncIterator<TSource>(first, second);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        var firstEnumerator = first.GetAsyncEnumerator();
                        var secondEnumerator = second.GetAsyncEnumerator();

                        var firstMoveNext = firstEnumerator.MoveNextAsync();
                        var secondMoveNext = secondEnumerator.MoveNextAsync();

                        var winner = await Task.WhenAny(firstMoveNext, secondMoveNext).ConfigureAwait(false);

                        //
                        // REVIEW: An alternative option is to call DisposeAsync on the other and await it, but this has two drawbacks:
                        //
                        // 1. Concurrent DisposeAsync while a MoveNextAsync is in flight.
                        // 2. The winner elected by Amb is blocked to yield results until the loser unblocks.
                        //
                        // The approach below has one drawback, namely that exceptions raised by loser are dropped on the floor.
                        //

                        if (winner == firstMoveNext)
                        {
                            enumerator = firstEnumerator;

                            var ignored = secondMoveNext.ContinueWith(_ =>
                            {
                                secondEnumerator.DisposeAsync();
                            });
                        }
                        else
                        {
                            enumerator = secondEnumerator;

                            var ignored = firstMoveNext.ContinueWith(_ =>
                            {
                                firstEnumerator.DisposeAsync();
                            });
                        }

                        state = AsyncIteratorState.Iterating;

                        if (await winner.ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
