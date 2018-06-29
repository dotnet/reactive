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
            private readonly Func<TState, bool> condition;
            private readonly TState initialState;
            private readonly Func<TState, TState> iterate;
            private readonly Func<TState, TResult> resultSelector;

            private TState currentState;

            private bool started;

            public GenerateAsyncIterator(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
            {
                Debug.Assert(condition != null);
                Debug.Assert(iterate != null);
                Debug.Assert(resultSelector != null);

                this.initialState = initialState;
                this.condition = condition;
                this.iterate = iterate;
                this.resultSelector = resultSelector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GenerateAsyncIterator<TState, TResult>(initialState, condition, iterate, resultSelector);
            }

            public override void Dispose()
            {
                currentState = default;

                base.Dispose();
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        started = false;
                        currentState = initialState;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (started)
                        {
                            currentState = iterate(currentState);
                        }

                        started = true;

                        if (condition(currentState))
                        {
                            current = resultSelector(currentState);
                            return TaskExt.True;
                        }
                        break;
                }

                Dispose();
                return TaskExt.False;
            }
        }
    }
}