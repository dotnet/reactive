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

            public override AsyncIterator<TResult> Clone() => new RepeatAsyncIterator<TResult>(_element, _count);

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => Task.FromResult(_count);

            public Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var res = new TResult[_count];

                for (var i = 0; i < _count; i++)
                {
                    res[i] = _element;
                }

                return Task.FromResult(res);
            }

            public Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var res = new List<TResult>(_count);

                for (var i = 0; i < _count; i++)
                {
                    res.Add(_element);
                }

                return Task.FromResult(res);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _remaining = _count;

                        if (_remaining > 0)
                        {
                            current = _element;
                        }

                        state = AsyncIteratorState.Iterating;

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
