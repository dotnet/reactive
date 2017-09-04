// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns a sequence that throws an exception upon enumeration.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="exception">Exception to throw upon enumerating the resulting sequence.</param>
        /// <returns>Sequence that throws the specified exception upon enumeration.</returns>
        public static IEnumerable<TResult> Throw<TResult>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return ThrowCore<TResult>(exception);
        }

        private static IEnumerable<TResult> ThrowCore<TResult>(Exception exception)
        {
            throw exception;
#pragma warning disable 0162
            yield break;
#pragma warning restore 0162
        }
    }
}
