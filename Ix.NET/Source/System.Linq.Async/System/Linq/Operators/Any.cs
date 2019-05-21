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
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    return await e.MoveNextAsync();
                }
            }
        }

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
            {
                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        internal static ValueTask<bool> AnyAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (await predicate(item).ConfigureAwait(false))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<bool> AnyAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
            {
                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
#endif
    }
}
