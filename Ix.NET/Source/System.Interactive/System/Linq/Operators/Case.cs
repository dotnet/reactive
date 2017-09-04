// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns a sequence from a dictionary based on the result of evaluating a selector function.
        /// </summary>
        /// <typeparam name="TValue">Type of the selector value.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
        /// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
        /// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty sequence.</returns>
        public static IEnumerable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IEnumerable<TResult>> sources)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Case(selector, sources, Enumerable.Empty<TResult>());
        }

        /// <summary>
        /// Returns a sequence from a dictionary based on the result of evaluating a selector function, also specifying a
        /// default sequence.
        /// </summary>
        /// <typeparam name="TValue">Type of the selector value.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
        /// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
        /// <param name="defaultSource">
        /// Default sequence to return in case there's no corresponding source for the computed
        /// selector value.
        /// </param>
        /// <returns>The source sequence corresponding with the evaluated selector value; otherwise, the default source.</returns>
        public static IEnumerable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IEnumerable<TResult>> sources, IEnumerable<TResult> defaultSource)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (defaultSource == null)
                throw new ArgumentNullException(nameof(defaultSource));

            return Defer(() =>
            {
                if (!sources.TryGetValue(selector(), out var result))
                {
                    result = defaultSource;
                }

                return result;
            });
        }
    }
}
