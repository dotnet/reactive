// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a sequence whose termination or disposal of an enumerator causes a finally action to be executed.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to run upon termination of the sequence, or when an enumerator is disposed.</param>
        /// <returns>Source sequence with guarantees on the invocation of the finally action.</returns>
        public static IEnumerable<TSource> Finally<TSource>(this IEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return FinallyCore(source, finallyAction);
        }

        private static IEnumerable<TSource> FinallyCore<TSource>(IEnumerable<TSource> source, Action finallyAction)
        {
            try
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
            finally
            {
                finallyAction();
            }
        }
    }
}
