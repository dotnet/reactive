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
        public static IAsyncEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var end = (long)start + count - 1L;
            if (count < 0 || end > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Empty<int>();

            return new RangeAsyncIterator(start, count);
        }

        private sealed class RangeAsyncIterator : AsyncIterator<int>, IAsyncIListProvider<int>
        {
            private readonly int start;
            private readonly int end;

            public RangeAsyncIterator(int start, int count)
            {
                Debug.Assert(count > 0);

                this.start = start;
                this.end = start + count;
            }

            public override AsyncIterator<int> Clone() => new RangeAsyncIterator(start, end - start);

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => Task.FromResult(end - start);

            public Task<int[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var res = new int[end - start];

                var value = start;

                for (var i = 0; i < res.Length; i++)
                {
                    res[i] = value++;
                }

                return Task.FromResult(res);
            }

            public Task<List<int>> ToListAsync(CancellationToken cancellationToken)
            {
                var res = new List<int>(end - start);

                for (var value = start; value < end; value++)
                {
                    res.Add(value);
                }

                return Task.FromResult(res);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        current = start;

                        state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        current++;

                        if (current != end)
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
