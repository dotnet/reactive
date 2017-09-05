// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new RepeatAsyncIterator<TResult>(element, count);
        }

        private sealed class RepeatAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly TResult element;
            private readonly int count;
            private int remaining;

            public RepeatAsyncIterator(TResult element, int count)
            {
                this.element = element;
                this.count = count;
            }

            public override AsyncIterator<TResult> Clone() => new RepeatAsyncIterator<TResult>(element, count);

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        remaining = count;

                        if (remaining > 0)
                        {
                            current = element;
                        }

                        state = AsyncIteratorState.Iterating;

                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (remaining-- != 0)
                        {
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
