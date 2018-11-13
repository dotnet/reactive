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
                throw Error.ArgumentOutOfRange(nameof(count));

            var end = (long)start + count - 1L;
            if (count < 0 || end > int.MaxValue)
                throw Error.ArgumentOutOfRange(nameof(count));

            if (count == 0)
                return Empty<int>();

            return new RangeAsyncIterator(start, count);
        }

        private sealed class RangeAsyncIterator : AsyncIterator<int>, IAsyncPartition<int>
        {
            private readonly int _start;
            private readonly int _end;

            public RangeAsyncIterator(int start, int count)
            {
                Debug.Assert(count > 0);

                _start = start;
                _end = start + count;
            }

            public override AsyncIteratorBase<int> Clone() => new RangeAsyncIterator(_start, _end - _start);

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => Task.FromResult(_end - _start);

            public IAsyncPartition<int> Skip(int count)
            {
                var n = _end - _start;

                if (count >= n)
                {
                    return EmptyAsyncIterator<int>.Instance;
                }

                return new RangeAsyncIterator(_start + count, n - count);
            }

            public IAsyncPartition<int> Take(int count)
            {
                var n = _end - _start;

                if (count >= n)
                {
                    return this;
                }

                return new RangeAsyncIterator(_start, count);
            }

            public Task<int[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var res = new int[_end - _start];

                var value = _start;

                for (var i = 0; i < res.Length; i++)
                {
                    res[i] = value++;
                }

                return Task.FromResult(res);
            }

            public Task<List<int>> ToListAsync(CancellationToken cancellationToken)
            {
                var res = new List<int>(_end - _start);

                for (var value = _start; value < _end; value++)
                {
                    res.Add(value);
                }

                return Task.FromResult(res);
            }

            public Task<Maybe<int>> TryGetElementAsync(int index, CancellationToken cancellationToken)
            {
                if ((uint)index < (uint)(_end - _start))
                {
                    return Task.FromResult(new Maybe<int>(_start + index));
                }

                return Task.FromResult(new Maybe<int>());
            }

            public Task<Maybe<int>> TryGetFirstAsync(CancellationToken cancellationToken) => Task.FromResult(new Maybe<int>(_start));

            public Task<Maybe<int>> TryGetLastAsync(CancellationToken cancellationToken) => Task.FromResult(new Maybe<int>(_end - 1));

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _current = _start;

                        _state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        _current++;

                        if (_current != _end)
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
