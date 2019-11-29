// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer: null, cancellationToken);
        }

        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }

        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function returning a key possibly asynchronously.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore<TSource, TKey>(source, keySelector, comparer: null, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function returning a key possibly asynchronously and supporting cancellation.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore<TSource, TKey>(source, keySelector, comparer: null, cancellationToken);
        }
#endif

        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function returning a key possibly asynchronously.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Returns the elements in an async-enumerable sequence with the maximum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function returning a key possibly asynchronously and supporting cancellation.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }
#endif

        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }
#endif
    }
}
