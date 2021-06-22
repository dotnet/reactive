// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    partial class AsyncEnumerable
    {
#if SUPPORT_FLAT_ASYNC_API
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#else
        /// <summary>
        /// Merges two async-enumerable sequences into one async-enumerable sequence by combining their elements in a pairwise fashion.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First async-enumerable source.</param>
        /// <param name="second">Second async-enumerable source.</param>
        /// <param name="selector">An asynchronous function to invoke and await for each consecutive pair of elements from the first and second source.</param>
        /// <returns>An async-enumerable sequence containing the result of pairwise combining the elements of the first and second source using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> ZipAwait<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#endif
    }
}
