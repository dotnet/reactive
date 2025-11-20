// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
#if REFERENCE_ASSEMBLY
    public static partial class AsyncEnumerableDeprecated
#else
    public static partial class AsyncEnumerable
#endif
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.takewhile?view=net-9.0-pp#system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean)))

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!predicate(element))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.takewhile?view=net-9.0-pp#system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-boolean)))

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!predicate(element, index))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous predicate to test each element for a condition.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use TakeWhile. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the TakeWhileAwait functionality now exists as overloads of TakeWhile. You will need to modify your callback to take an additional CancellationToken argument.")]
        private static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use TakeWhile. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the TakeWhileAwaitWithCancellation functionality now exists as overloads of TakeWhile.")]
        private static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }
#endif

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use TakeWhile. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the TakeWhileAwait functionality now exists as overloads of TakeWhile. You will need to modify your callback to take an additional CancellationToken argument.")]
        private static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!await predicate(element, index).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use TakeWhile. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the TakeWhileAwaitWithCancellation functionality now exists as overloads of TakeWhile.")]
        private static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var index = -1;

                await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (!await predicate(element, index, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
        }
#endif
    }
}
