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
        /// Determines whether two sequences are equal by comparing the elements pairwise.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First async-enumerable sequence to compare.</param>
        /// <param name="second">Second async-enumerable sequence to compare.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken = default) =>
            SequenceEqualAsync(first, second, comparer: null, cancellationToken);

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements pairwise using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First async-enumerable sequence to compare.</param>
        /// <param name="second">Second async-enumerable sequence to compare.</param>
        /// <param name="comparer">Comparer used to compare elements of both sequences.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the specified equality comparer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            comparer ??= EqualityComparer<TSource>.Default;

            if (first is ICollection<TSource> firstCol && second is ICollection<TSource> secondCol)
            {
                if (firstCol.Count != secondCol.Count)
                {
                    return new ValueTask<bool>(false);
                }

                if (firstCol is IList<TSource> firstList && secondCol is IList<TSource> secondList)
                {
                    var count = firstCol.Count;

                    for (var i = 0; i < count; i++)
                    {
                        if (!comparer.Equals(firstList[i], secondList[i]))
                        {
                            return new ValueTask<bool>(false);
                        }
                    }

                    return new ValueTask<bool>(true);
                }
            }

            return Core(first, second, comparer, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
            {
                await using var e1 = first.GetConfiguredAsyncEnumerator(cancellationToken, false);
                await using var e2 = second.GetConfiguredAsyncEnumerator(cancellationToken, false);

                while (await e1.MoveNextAsync())
                {
                    if (!(await e2.MoveNextAsync() && comparer.Equals(e1.Current, e2.Current)))
                    {
                        return false;
                    }
                }

                return !await e2.MoveNextAsync();
            }
        }
    }
}
