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
        /// Repeats the element indefinitely.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="element">Element to repeat.</param>
        /// <returns>The async-enumerable sequence producing the element repeatedly and sequentially.</returns>
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element)
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            return Core(element);

            static async IAsyncEnumerable<TResult> Core(TResult element, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    yield return element;
                }
            }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        }

        /// <summary>
        /// Repeats the async-enumerable sequence indefinitely.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat.</param>
        /// <returns>The async-enumerable sequence producing the elements of the given sequence repeatedly and sequentially.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                while (true)
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Repeats the async-enumerable sequence a specified number of times.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat.</param>
        /// <param name="count">Number of times to repeat the sequence.</param>
        /// <returns>The async-enumerable sequence producing the elements of the given sequence repeatedly.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count < 0)
                throw Error.ArgumentOutOfRange(nameof(count));

            return Core(source, count);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, int count, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                for (var i = 0; i < count; i++)
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
