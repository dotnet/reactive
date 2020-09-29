// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Concatenates all inner async-enumerable sequences, as long as the previous async-enumerable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence of inner async-enumerable sequences.</param>
        /// <returns>An async-enumerable sequence that contains the elements of each observed inner sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return Core(sources);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<IAsyncEnumerable<TSource>> sources, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var source in sources.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Concatenates all async-enumerable sequences in the given enumerable sequence, as long as the previous async-enumerable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An async-enumerable sequence that contains the elements of each given sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return Core(sources);

            static async IAsyncEnumerable<TSource> Core(IEnumerable<IAsyncEnumerable<TSource>> sources, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                foreach (var source in sources)
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Concatenates all of the specified async-enumerable sequences, as long as the previous async-enumerable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An async-enumerable sequence that contains the elements of each given sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return Core(sources);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource>[] sources, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                foreach (var source in sources)
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
