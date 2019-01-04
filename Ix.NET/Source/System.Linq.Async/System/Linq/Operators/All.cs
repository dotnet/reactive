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
        public static Task<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<bool> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (!predicate(e.Current))
                            return false;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }

                return true;
            }
        }

        public static Task<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<bool> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (!await predicate(e.Current).ConfigureAwait(false))
                            return false;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }

                return true;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core();

            async Task<bool> Core()
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (!await predicate(e.Current, cancellationToken).ConfigureAwait(false))
                            return false;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }

                return true;
            }
        }
#endif
    }
}
