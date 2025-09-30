﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.firstasync?view=net-9.0-pp#system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)

        /// <summary>
        /// Returns the first element of an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source async-enumerable sequence.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>ValueTask containing the first element in the async-enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
            {
                var first = await TryGetFirst(source, cancellationToken).ConfigureAwait(false);

                return first.HasValue ? first.Value : throw Error.NoElements();
            }
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.firstasync?view=net-9.0-pp#system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)

        /// <summary>
        /// Returns the first element of an async-enumerable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source async-enumerable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>ValueTask containing the first element in the async-enumerable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
            {
                var first = await TryGetFirst(source, predicate, cancellationToken).ConfigureAwait(false);

                return first.HasValue ? first.Value : throw Error.NoElements();
            }
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.firstasync?view=net-9.0-pp#system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        // That only offers the async-with-cancellation callback overload, but the simpler form offers no additional functionality.

        /// <summary>
        /// Returns the first element of an async-enumerable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the sequence.</typeparam>
        /// <param name="source">Source async-enumerable sequence.</param>
        /// <param name="predicate">An asynchronous predicate that will be invoked and awaited for each element in the sequence.</param>
        /// <param name="cancellationToken">An optional cancellation token for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the first element in the sequence that satisfies the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use FirstAsync. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the FirstAwaitAsync functionality now exists as overloads of FirstAsync.")]
        private static ValueTask<TSource> FirstAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                var first = await TryGetFirst(source, predicate, cancellationToken).ConfigureAwait(false);

                return first.HasValue ? first.Value : throw Error.NoElements();
            }
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use FirstAsync. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the FirstAwaitWithCancellationAsync functionality now exists as overloads of FirstAsync.")]
        internal static ValueTask<TSource> FirstAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                var first = await TryGetFirst(source, predicate, cancellationToken).ConfigureAwait(false);

                return first.HasValue ? first.Value : throw Error.NoElements();
            }
        }
#endif
    }
}
