// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        /// <summary>
        /// Generates an async-enumerable sequence that repeats the given element the specified number of times.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be repeated in the produced sequence.</typeparam>
        /// <param name="element">Element to repeat.</param>
        /// <param name="count">Number of times to repeat the element.</param>
        /// <returns>An async-enumerable sequence that repeats the given element the specified number of times.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw Error.ArgumentOutOfRange(nameof(count));

            return new RepeatAsyncIterator<TResult>(element, count);
        }

        private sealed class RepeatAsyncIterator<TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly TResult _element;
            private readonly int _count;
            private int _remaining;

            public RepeatAsyncIterator(TResult element, int count)
            {
                _element = element;
                _count = count;
            }

            public override AsyncIteratorBase<TResult> Clone() => new RepeatAsyncIterator<TResult>(_element, _count);

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => new ValueTask<int>(_count);

            public ValueTask<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var res = new TResult[_count];

                for (var i = 0; i < _count; i++)
                {
                    res[i] = _element;
                }

                return new ValueTask<TResult[]>(res);
            }

            public ValueTask<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var res = new List<TResult>(_count);

                for (var i = 0; i < _count; i++)
                {
                    res.Add(_element);
                }

                return new ValueTask<List<TResult>>(res);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _remaining = _count;

                        if (_remaining > 0)
                        {
                            _current = _element;
                        }

                        _state = AsyncIteratorState.Iterating;

                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_remaining-- != 0)
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
