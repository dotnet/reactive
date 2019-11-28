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
        /// Produces the set intersection of two async-enumerable sequences by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An async-enumerable sequence whose distinct elements that also appear in second will be returned.</param>
        /// <param name="second">An async-enumerable sequence whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            Intersect(first, second, comparer: null);

        /// <summary>
        /// Produces the set intersection of two async-enumerable sequences by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">An async-enumerable sequence whose distinct elements that also appear in second will be returned.</param>
        /// <param name="second">An async-enumerable sequence whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var set = new Set<TSource>(comparer);

                await foreach (var element in second.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    set.Add(element);
                }

                await foreach (var element in first.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (set.Remove(element))
                    {
                        yield return element;
                    }
                }
            }
        }
    }
}
