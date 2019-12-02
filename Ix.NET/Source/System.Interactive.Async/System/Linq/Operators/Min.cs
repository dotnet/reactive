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
        /// Returns the minimum element in an async-enumerable sequence according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="comparer">Comparer used to compare elements.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element with the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<TSource> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, comparer, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, IComparer<TSource>? comparer, CancellationToken cancellationToken)
            {
                comparer ??= Comparer<TSource>.Default;

                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                if (!await e.MoveNextAsync())
                    throw Error.NoElements();

                var min = e.Current;

                while (await e.MoveNextAsync())
                {
                    var cur = e.Current;

                    if (comparer.Compare(cur, min) < 0)
                    {
                        min = cur;
                    }
                }

                return min;
            }
        }
    }
}
