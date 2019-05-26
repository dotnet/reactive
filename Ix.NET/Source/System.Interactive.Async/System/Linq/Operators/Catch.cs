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

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

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
                    await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                    {
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
                }

                error?.Throw();
            }
        }
    }
}
