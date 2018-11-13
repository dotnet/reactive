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
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return new ConcatAsyncEnumerableAsyncIterator<TSource>(sources);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return ConcatCore(sources);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return ConcatCore(sources);
        }

        private static IAsyncEnumerable<TSource> ConcatCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new ConcatEnumerableAsyncIterator<TSource>(sources);
        }

        private sealed class ConcatEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> _source;

            public ConcatEnumerableAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ConcatEnumerableAsyncIterator<TSource>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_outerEnumerator != null)
                {
                    _outerEnumerator.Dispose();
                    _outerEnumerator = null;
                }

                if (_currentEnumerator != null)
                {
                    await _currentEnumerator.DisposeAsync().ConfigureAwait(false);
                    _currentEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private IEnumerator<IAsyncEnumerable<TSource>> _outerEnumerator;
            private IAsyncEnumerator<TSource> _currentEnumerator;
            private int _mode;

            private const int State_OuterNext = 1;
            private const int State_While = 4;

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _source.GetEnumerator();
                        _mode = State_OuterNext;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_OuterNext:
                                if (_outerEnumerator.MoveNext())
                                {
                                    // make sure we dispose the previous one if we're about to replace it
                                    if (_currentEnumerator != null)
                                    {
                                        await _currentEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentEnumerator = _outerEnumerator.Current.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_While;
                                    goto case State_While;
                                }

                                break;
                            case State_While:
                                if (await _currentEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _currentEnumerator.Current;
                                    return true;
                                }

                                // No more on the inner enumerator, move to the next outer
                                goto case State_OuterNext;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class ConcatAsyncEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<IAsyncEnumerable<TSource>> _source;

            public ConcatAsyncEnumerableAsyncIterator(IAsyncEnumerable<IAsyncEnumerable<TSource>> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ConcatAsyncEnumerableAsyncIterator<TSource>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_outerEnumerator != null)
                {
                    await _outerEnumerator.DisposeAsync().ConfigureAwait(false);
                    _outerEnumerator = null;
                }

                if (_currentEnumerator != null)
                {
                    await _currentEnumerator.DisposeAsync().ConfigureAwait(false);
                    _currentEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private IAsyncEnumerator<IAsyncEnumerable<TSource>> _outerEnumerator;
            private IAsyncEnumerator<TSource> _currentEnumerator;
            private int _mode;

            private const int State_OuterNext = 1;
            private const int State_While = 4;

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_OuterNext;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_OuterNext:
                                if (await _outerEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    // make sure we dispose the previous one if we're about to replace it
                                    if (_currentEnumerator != null)
                                    {
                                        await _currentEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _currentEnumerator = _outerEnumerator.Current.GetAsyncEnumerator(_cancellationToken);

                                    _mode = State_While;
                                    goto case State_While;
                                }

                                break;
                            case State_While:
                                if (await _currentEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _currentEnumerator.Current;
                                    return true;
                                }

                                // No more on the inner enumerator, move to the next outer
                                goto case State_OuterNext;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}
