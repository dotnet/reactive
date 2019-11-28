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
        /// <summary>
        /// Generates an async-enumerable sequence of integral numbers within a specified range.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <returns>An async-enumerable sequence that contains a range of sequential integral numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero. -or- <paramref name="start"/> + <paramref name="count"/> - 1 is larger than <see cref="int.MaxValue"/>.</exception>
        public static IAsyncEnumerable<int> Range(int start, int count)
        {
            if (count < 0)
                throw Error.ArgumentOutOfRange(nameof(count));

            var end = (long)start + count - 1L;
            if (end > int.MaxValue)
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

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => new ValueTask<int>(_end - _start);

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

            public ValueTask<int[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var res = new int[_end - _start];

                var value = _start;

                for (var i = 0; i < res.Length; i++)
                {
                    res[i] = value++;
                }

                return new ValueTask<int[]>(res);
            }

            public ValueTask<List<int>> ToListAsync(CancellationToken cancellationToken)
            {
                var res = new List<int>(_end - _start);

                for (var value = _start; value < _end; value++)
                {
                    res.Add(value);
                }

                return new ValueTask<List<int>>(res);
            }

            public ValueTask<Maybe<int>> TryGetElementAtAsync(int index, CancellationToken cancellationToken)
            {
                if ((uint)index < (uint)(_end - _start))
                {
                    return new ValueTask<Maybe<int>>(new Maybe<int>(_start + index));
                }

                return new ValueTask<Maybe<int>>(new Maybe<int>());
            }

            public ValueTask<Maybe<int>> TryGetFirstAsync(CancellationToken cancellationToken) => new ValueTask<Maybe<int>>(new Maybe<int>(_start));

            public ValueTask<Maybe<int>> TryGetLastAsync(CancellationToken cancellationToken) => new ValueTask<Maybe<int>>(new Maybe<int>(_end - 1));

            protected override ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _current = _start;

                        _state = AsyncIteratorState.Iterating;
                        return new ValueTask<bool>(true);

                    case AsyncIteratorState.Iterating:
                        _current++;

                        if (_current != _end)
                        {
                            return new ValueTask<bool>(true);
                        }

                        break;
                }

                return Core();

                async ValueTask<bool> Core()
                {
                    await DisposeAsync().ConfigureAwait(false);
                    return false;
                }
            }
        }
    }
}
