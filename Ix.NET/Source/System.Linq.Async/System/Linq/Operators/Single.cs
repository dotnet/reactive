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
        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
            {
                if (source is IList<TSource> list)
                {
                    return list.Count switch
                    {
                        0 => throw Error.NoElements(),
                        1 => list[0],
                        _ => throw Error.MoreThanOneElement(),
                    };
                }

                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                if (!await e.MoveNextAsync())
                {
                    throw Error.NoElements();
                }

                var result = e.Current;

                if (await e.MoveNextAsync())
                {
                    throw Error.MoreThanOneElement();
                }

                return result;
            }
        }

        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (predicate(result))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (predicate(e.Current))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }

                throw Error.NoElements();
            }
        }

        internal static ValueTask<TSource> SingleAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (await predicate(result).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (await predicate(e.Current).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }

                throw Error.NoElements();
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<TSource> SingleAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (await predicate(result, cancellationToken).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (await predicate(e.Current, cancellationToken).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }

                throw Error.NoElements();
            }
        }
#endif
    }
}
