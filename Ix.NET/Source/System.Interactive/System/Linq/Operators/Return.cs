// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Returns a sequence with a single element.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="value">Single element of the resulting sequence.</param>
        /// <returns>Sequence with a single element.</returns>
        public static IEnumerable<TResult> Return<TResult>(TResult value)
        {
            yield return value;
        }
    }
}
