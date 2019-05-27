// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Generates a sequence by mimicking a for loop.
        /// </summary>
        /// <typeparam name="TState">State type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="initialState">Initial state of the generator loop.</param>
        /// <param name="condition">Loop condition.</param>
        /// <param name="iterate">State update function to run after every iteration of the generator loop.</param>
        /// <param name="resultSelector">Result selector to compute resulting sequence elements.</param>
        /// <returns>Sequence obtained by running the generator loop, yielding computed elements.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return GenerateCore(initialState, condition, iterate, resultSelector);
        }

        private static IEnumerable<TResult> GenerateCore<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            for (var i = initialState; condition(i); i = iterate(i))
            {
                yield return resultSelector(i);
            }
        }
    }
}
