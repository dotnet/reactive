// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // NB: Synchronous LINQ to Objects doesn't hide the implementation of the source either.

        // Note: this was previous in System.Linq.Async, but since .NET 10.0's System.Linq.AsyncEnumerable chose not to
        // implement it (even though Enumerable.AsEnumerable exists), we moved it into System.Interactive.Async so that
        // it remains available even after developers remove their dependency on the deprecated System.Linq.Async.

        /// <summary>
        /// Hides the identity of an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose identity to hide.</param>
        /// <returns>An async-enumerable sequence that hides the identity of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncEnumerable<TSource> source) => source;
    }
}
