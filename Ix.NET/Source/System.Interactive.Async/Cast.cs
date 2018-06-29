// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Check to see if it already is and short-circuit
            if (source is IAsyncEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

            return source.Select(x => (TResult)x);
        }

        public static IAsyncEnumerable<TType> OfType<TType>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(x => x is TType)
                         .Cast<TType>();
        }
    }
}