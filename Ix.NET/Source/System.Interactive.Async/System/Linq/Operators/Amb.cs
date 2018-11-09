// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

            return new AmbEnumerable<TSource>(first, second);
        }

        public static IAsyncEnumerable<TSource> Amb<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new AmbAsyncIteratorN<TSource>(sources);
        }

        public static IAsyncEnumerable<TSource> Amb<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new AmbAsyncIteratorN<TSource>(sources.ToArray());
        }

        private sealed class AmbEnumerable<TSource> : IAsyncEnumerable<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source1;

            private readonly IAsyncEnumerable<TSource> _source2;

            public AmbEnumerable(IAsyncEnumerable<TSource> source1, IAsyncEnumerable<TSource> source2)
            {
                _source1 = source1;
                _source2 = source2;
            }

            public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new AmbEnumerator(_source1, _source2, cancellationToken);
            }

            private sealed class AmbEnumerator : IAsyncEnumerator<TSource>
            {
                private readonly IAsyncEnumerable<TSource> _source1;
                private CancellationTokenSource _token1;
                private IAsyncEnumerator<TSource> _enumerator1;
                private readonly IAsyncEnumerable<TSource> _source2;
                private CancellationTokenSource _token2;
                private IAsyncEnumerator<TSource> _enumerator2;

                private readonly CancellationToken _mainToken;

                private CancellationTokenRegistration _cancelBoth;

                private IAsyncEnumerator<TSource> _winner;

                public TSource Current => _winner.Current;

                private bool _once;

                private Task _loserTask;

                public AmbEnumerator(IAsyncEnumerable<TSource> source1, IAsyncEnumerable<TSource> source2, CancellationToken mainToken)
                {
                    _source1 = source1;
                    _source2 = source2;
                    _mainToken = mainToken;
                }

                public async ValueTask DisposeAsync()
                {
                    _cancelBoth.Dispose();
                    if (_winner != null)
                    {
                        await _winner.DisposeAsync();
                        _winner = this; // keep it non-null
                        await _loserTask;
                    }
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    if (!_once)
                    {
                        _once = true;

                        _token1 = new CancellationTokenSource();
                        _token2 = new CancellationTokenSource();

                        _cancelBoth = _mainToken.Register(state => ((AmbEnumerator)state).CancelBoth(), this);

                        _enumerator1 = _source1.GetAsyncEnumerator(_token1.Token);
                        _enumerator2 = _source2.GetAsyncEnumerator(_token2.Token);

                        var t1 = _enumerator1.MoveNextAsync().AsTask();
                        var t2 = _enumerator2.MoveNextAsync().AsTask();

                        var winner = await Task.WhenAny(t1, t2);

                        if (winner == t1)
                        {
                            _winner = _enumerator1;
                            _loserTask = t2.ContinueWith(async (t, s) =>
                                await ((AmbEnumerator)s)._enumerator2.DisposeAsync(),
                                this);
                        }
                        else
                        {
                            _winner = _enumerator2;
                            _loserTask = t1.ContinueWith(async (t, s) =>
                                await ((AmbEnumerator)s)._enumerator1.DisposeAsync(),
                                this);
                        }

                        return await winner;
                    }

                    return await _winner.MoveNextAsync().ConfigureAwait(false);
                }

                private void CancelBoth()
                {
                    _token1.Cancel();
                    _token2.Cancel();
                }
            }
        }

        private sealed class AmbAsyncIteratorN<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource>[] _sources;

            private IAsyncEnumerator<TSource> _enumerator;

            public AmbAsyncIteratorN(IAsyncEnumerable<TSource>[] sources)
            {
                Debug.Assert(sources != null);

                _sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AmbAsyncIteratorN<TSource>(_sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        var n = _sources.Length;

                        var enumerators = new IAsyncEnumerator<TSource>[n];
                        var moveNexts = new ValueTask<bool>[n];

                        for (var i = 0; i < n; i++)
                        {
                            var enumerator = _sources[i].GetAsyncEnumerator(cancellationToken);

                            enumerators[i] = enumerator;
                            moveNexts[i] = enumerator.MoveNextAsync();
                        }

                        var winner = await Task.WhenAny(moveNexts.Select(t => t.AsTask())).ConfigureAwait(false);

                        //
                        // REVIEW: An alternative option is to call DisposeAsync on the other and await it, but this has two drawbacks:
                        //
                        // 1. Concurrent DisposeAsync while a MoveNextAsync is in flight.
                        // 2. The winner elected by Amb is blocked to yield results until all losers unblocks.
                        //
                        // The approach below has one drawback, namely that exceptions raised by any loser are dropped on the floor.
                        //

                        var winnerIndex = Array.IndexOf(moveNexts, winner);

                        _enumerator = enumerators[winnerIndex];

                        for (var i = 0; i < n; i++)
                        {
                            if (i != winnerIndex)
                            {
                                var ignored = moveNexts[i].AsTask().ContinueWith(_ =>
                                {
                                    enumerators[i].DisposeAsync();
                                });
                            }
                        }

                        state = AsyncIteratorState.Iterating;

                        if (await winner.ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
                            return true;
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
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
