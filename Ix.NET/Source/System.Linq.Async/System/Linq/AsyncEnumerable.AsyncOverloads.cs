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
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#else
        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence using the specified asynchronous key and element selector functions.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence using the specified asynchronous key and element selector functions.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking key and element selector functions on each source element and awaiting the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking key and element selector functions on each source element and awaiting the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each source element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);

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
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#endif
    }
}
