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
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new ZipAsyncIterator<TFirst, TSecond, TResult>(first, second, selector);
        }

        private sealed class ZipAsyncIterator<TFirst, TSecond, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TFirst> first;
            private readonly IAsyncEnumerable<TSecond> second;
            private readonly Func<TFirst, TSecond, TResult> selector;

            private IAsyncEnumerator<TFirst> firstEnumerator;
            private IAsyncEnumerator<TSecond> secondEnumerator;

            public ZipAsyncIterator(IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);
                Debug.Assert(selector != null);

                this.first = first;
                this.second = second;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new ZipAsyncIterator<TFirst, TSecond, TResult>(first, second, selector);
            }

            public override async Task DisposeAsync()
            {
                if (firstEnumerator != null)
                {
                    await firstEnumerator.DisposeAsync().ConfigureAwait(false);
                    firstEnumerator = null;
                }
                if (secondEnumerator != null)
                {
                    await secondEnumerator.DisposeAsync().ConfigureAwait(false);
                    secondEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        firstEnumerator = first.GetAsyncEnumerator();
                        secondEnumerator = second.GetAsyncEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:

                        // We kick these off and join so they can potentially run in parallel
                        var ft = firstEnumerator.MoveNextAsync();
                        var st = secondEnumerator.MoveNextAsync();
                        await Task.WhenAll(ft, st)
                                  .ConfigureAwait(false);

                        if (ft.Result && st.Result)
                        {
                            current = selector(firstEnumerator.Current, secondEnumerator.Current);
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}