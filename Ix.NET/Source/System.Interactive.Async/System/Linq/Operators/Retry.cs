// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Repeats the source async-enumerable sequence until it successfully terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
        /// <returns>An async-enumerable sequence producing the elements of the given sequence repeatedly until it terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new[] { source }.Repeat().Catch();
        }

        /// <summary>
        /// Repeats the source async-enumerable sequence the specified number of times or until it successfully terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
        /// <param name="retryCount">Number of times to repeat the sequence.</param>
        /// <returns>An async-enumerable sequence producing the elements of the given sequence repeatedly until it terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="retryCount"/> is less than zero.</exception>
        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (retryCount < 0)
                throw Error.ArgumentOutOfRange(nameof(retryCount));

            return new[] { source }.Repeat(retryCount).Catch();
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            for (var i = 0; i < count; i++)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }
}
