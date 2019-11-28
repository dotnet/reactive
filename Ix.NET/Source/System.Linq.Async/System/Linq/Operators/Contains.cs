// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        /// <summary>
        /// Determines whether an async-enumerable sequence contains a specified element by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the source sequence.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element determining whether the source sequence contains an element that has the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken = default) =>
            source is ICollection<TSource> collection ? new ValueTask<bool>(collection.Contains(value)) :
            ContainsAsync(source, value, comparer: null, cancellationToken);

        /// <summary>
        /// Determines whether an async-enumerable sequence contains a specified element by using a specified System.Collections.Generic.IEqualityComparer{T}.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the source sequence.</param>
        /// <param name="comparer">An equality comparer to compare elements.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element determining whether the source sequence contains an element that has the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            //
            // See https://github.com/dotnet/corefx/pull/25097 for the optimization here.
            //
            if (comparer == null)
            {
                return Core(source, value, cancellationToken);

                static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken)
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        if (EqualityComparer<TSource>.Default.Equals(item, value))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else
            {
                return Core(source, value, comparer, cancellationToken);

                static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        if (comparer.Equals(item, value))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }
    }
}
