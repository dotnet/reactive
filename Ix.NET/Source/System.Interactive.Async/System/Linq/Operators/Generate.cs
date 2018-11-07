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
        public static IAsyncEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (iterate == null)
            {
                throw new ArgumentNullException(nameof(iterate));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return new GenerateAsyncIterator<TState, TResult>(initialState, condition, iterate, resultSelector);
        }

        private sealed class GenerateAsyncIterator<TState, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TState, bool> _condition;
            private readonly TState _initialState;
            private readonly Func<TState, TState> _iterate;
            private readonly Func<TState, TResult> _resultSelector;

            private TState _currentState;

            private bool _started;

            public GenerateAsyncIterator(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
            {
                Debug.Assert(condition != null);
                Debug.Assert(iterate != null);
                Debug.Assert(resultSelector != null);

                _initialState = initialState;
                _condition = condition;
                _iterate = iterate;
                _resultSelector = resultSelector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GenerateAsyncIterator<TState, TResult>(_initialState, _condition, _iterate, _resultSelector);
            }

            public override async ValueTask DisposeAsync()
            {
                _currentState = default;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _started = false;
                        _currentState = _initialState;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_started)
                        {
                            _currentState = _iterate(_currentState);
                        }

                        _started = true;

                        if (_condition(_currentState))
                        {
                            current = _resultSelector(_currentState);
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
