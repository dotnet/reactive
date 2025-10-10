// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
        // NB: Synchronous LINQ to Objects doesn't hide the implementation of the source either.
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // Note: this one isn't actually in the System.Linq.AsyncEnumerable package, so we've moved it
        // to System.Interactive.Async because that's the home for LINQ-like implementations for
        // IAsyncEnumerable<T> that aren't in the runtime libraries.
        // It therefore remains available only for runtime binary compatibility, and is no longer
        // visible in System.Linq.Async at compile time.

        /// <summary>
        /// Hides the identity of an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose identity to hide.</param>
        /// <returns>An async-enumerable sequence that hides the identity of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncEnumerable<TSource> source) => source;
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
    }
}
