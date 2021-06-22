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
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#else
        /// <summary>
        /// Filters the elements of an async-enumerable sequence based on an asynchronous predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose elements to filter.</param>
        /// <param name="predicate">An asynchronous predicate to test each source element for a condition.</param>
        /// <returns>An async-enumerable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> WhereAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Filters the elements of an async-enumerable sequence based on an asynchronous predicate that incorporates the element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose elements to filter.</param>
        /// <param name="predicate">An asynchronous predicate to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An async-enumerable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> WhereAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);

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
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#endif
    }
}
