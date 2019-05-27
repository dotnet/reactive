// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns an enumerable sequence based on the evaluation result of the given condition.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="thenSource">Sequence to return in case the condition evaluates true.</param>
        /// <param name="elseSource">Sequence to return in case the condition evaluates false.</param>
        /// <returns>Either of the two input sequences based on the result of evaluating the condition.</returns>
        public static IEnumerable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource, IEnumerable<TResult> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return Defer(() => condition() ? thenSource : elseSource);
        }

        /// <summary>
        /// Returns an enumerable sequence if the evaluation result of the given condition is true, otherwise returns an empty
        /// sequence.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="thenSource">Sequence to return in case the condition evaluates true.</param>
        /// <returns>The given input sequence if the condition evaluates true; otherwise, an empty sequence.</returns>
        public static IEnumerable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));

            return Defer(() => condition() ? thenSource : Enumerable.Empty<TResult>());
        }
    }
}
