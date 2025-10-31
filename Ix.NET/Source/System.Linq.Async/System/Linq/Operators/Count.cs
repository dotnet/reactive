// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.countasync?view=net-9.0-pp#system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)

        /// <summary>
        /// Returns an async-enumerable sequence containing an <see cref="int" /> that represents the total number of elements in an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence that contains elements to be counted.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element with the number of elements in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The number of elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source switch
            {
                ICollection<TSource> collection => new ValueTask<int>(collection.Count),
                IAsyncIListProvider<TSource> listProv => listProv.GetCountAsync(onlyIfCheap: false, cancellationToken),
                ICollection collection => new ValueTask<int>(collection.Count),
                _ => Core(source, cancellationToken),
            };

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
            {
                var count = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.countasync?view=net-9.0-pp#system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)

        /// <summary>
        /// Returns an async-enumerable sequence containing an <see cref="int" /> that represents how many elements in the specified async-enumerable sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence that contains elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element with a number that represents how many elements in the input sequence satisfy the condition in the predicate function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
            {
                var count = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (predicate(item))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.countasync?view=net-9.0-pp#system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        // Two overloads here are replaced by a single method.
        // System.Linq.AsyncEnumerable only supplies the async predicate form where the callback accepts a cancellation token, but
        // the CountAwaitAsync version that does not take this does not add any additional functionality. Since developers will need
        // to change their code in any case to move off these obsolete methods, it would not be particularly useful to add a
        // non-cancellable async predicate overload in System.Interactive.Async.

        /// <summary>
        /// Counts the elements in an async-enumerable sequence that satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to count.</param>
        /// <param name="predicate">An asynchronous predicate to apply to each element in the source sequence.</param>
        /// <param name="cancellationToken">An optional cancellation token for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the number of elements in the sequence that satisfy the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use CountAsync. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the CountAwaitAsync functionality now exists as overloads of CountAsync.")]
        private static ValueTask<int> CountAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                var count = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (await predicate(item).ConfigureAwait(false))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }

#if !NO_DEEP_CANCELLATION
        [Obsolete("Use CountAsync. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the CountAwaitWithCancellationAsync functionality now exists as overloads of CountAsync.")]
        [GenerateAsyncOverload]
        private static ValueTask<int> CountAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                var count = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }
#endif
    }
}
