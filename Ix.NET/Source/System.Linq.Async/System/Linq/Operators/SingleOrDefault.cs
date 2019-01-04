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
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core();

            async Task<TSource> Core()
            {
                if (source is IList<TSource> list)
                {
                    switch (list.Count)
                    {
                        case 0: return default;
                        case 1: return list[0];
                    }

                    throw Error.MoreThanOneElement();
                }

                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return default;
                    }

                    var result = e.Current;

                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return result;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }

                throw Error.MoreThanOneElement();
            }
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<TSource> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (predicate(result))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (predicate(e.Current))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }

                    return default;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<TSource> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (await predicate(result).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (await predicate(e.Current).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }

                    return default;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<TSource> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (await predicate(result, cancellationToken).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (await predicate(e.Current, cancellationToken).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }

                    return default;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
#endif
    }
}
