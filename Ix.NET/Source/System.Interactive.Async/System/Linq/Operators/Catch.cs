// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // REVIEW: All Catch operators may catch OperationCanceledException due to cancellation of the enumeration
        //         of the source. Should we explicitly avoid handling this? E.g. as follows:
        //
        //         catch (TException ex) when(!(ex is OperationCanceledException oce && oce.CancellationToken == cancellationToken))

        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception of the specified type with the async-enumerable sequence produced by the handler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and sequences returned by the exception handler function.</typeparam>
        /// <typeparam name="TException">The type of the exception to catch and handle. Needs to derive from <see cref="Exception"/>.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Exception handler function, producing another async-enumerable sequence.</param>
        /// <returns>An async-enumerable sequence containing the source sequence's elements, followed by the elements produced by the handler's resulting async-enumerable sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="handler"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, IAsyncEnumerable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (true)
                    {
                        TSource c;

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = handler(ex);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception of the specified type with the async-enumerable sequence produced asynchronously by the handler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and sequences returned by the exception handler function.</typeparam>
        /// <typeparam name="TException">The type of the exception to catch and handle. Needs to derive from <see cref="Exception"/>.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Exception handler function, producing another async-enumerable sequence asynchronously.</param>
        /// <returns>An async-enumerable sequence containing the source sequence's elements, followed by the elements produced by the handler's resulting async-enumerable sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="handler"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, ValueTask<IAsyncEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (true)
                    {
                        TSource c;

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = await handler(ex).ConfigureAwait(false);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception of the specified type with the async-enumerable sequence produced asynchronously (cancellable) by the handler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and sequences returned by the exception handler function.</typeparam>
        /// <typeparam name="TException">The type of the exception to catch and handle. Needs to derive from <see cref="Exception"/>.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Exception handler function, producing another async-enumerable sequence asynchronously while supporting cancellation.</param>
        /// <returns>An async-enumerable sequence containing the source sequence's elements, followed by the elements produced by the handler's resulting async-enumerable sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="handler"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (true)
                    {
                        TSource c;

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = await handler(ex, cancellationToken).ConfigureAwait(false);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception with the next async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source and handler sequences.</typeparam>
        /// <param name="sources">Observable sequences to catch exceptions for.</param>
        /// <returns>An async-enumerable sequence containing elements from consecutive source sequences until a source sequence terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception with the next async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source and handler sequences.</typeparam>
        /// <param name="sources">Observable sequences to catch exceptions for.</param>
        /// <returns>An async-enumerable sequence containing elements from consecutive source sequences until a source sequence terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

        /// <summary>
        /// Continues an async-enumerable sequence that is terminated by an exception with the next async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and handler sequence.</typeparam>
        /// <param name="first">First async-enumerable sequence whose exception (if any) is caught.</param>
        /// <param name="second">Second async-enumerable sequence used to produce results when an error occurred in the first sequence.</param>
        /// <returns>An async-enumerable sequence containing the first sequence's elements, followed by the elements of the second sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return CatchCore(new[] { first, second });
        }

        private static IAsyncEnumerable<TSource> CatchCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var error = default(ExceptionDispatchInfo);

                foreach (var source in sources)
                {
                    await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                    error = null;

                    while (true)
                    {
                        TSource c;

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (Exception ex)
                        {
                            error = ExceptionDispatchInfo.Capture(ex);
                            break;
                        }

                        yield return c;
                    }

                    if (error == null)
                        break;
                }

                error?.Throw();
            }
        }
    }
}
