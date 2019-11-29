// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // REVIEW: Add async variant?

        /// <summary>
        /// Generates an async-enumerable sequence by running a state-driven loop producing the sequence's elements.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw Error.ArgumentNull(nameof(condition));
            if (iterate == null)
                throw Error.ArgumentNull(nameof(iterate));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AsyncEnumerable.Create(Core);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                for (var state = initialState; condition(state); state = iterate(state))
                {
                    // REVIEW: Check for cancellation?

                    yield return resultSelector(state);
                }
            }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        }
    }
}
