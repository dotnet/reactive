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
        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return LastCore(source, CancellationToken.None);
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return LastCore(source, cancellationToken);
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastCore(source, predicate, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastCore(source, predicate, cancellationToken);
        }
#endif

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            Maybe<TSource> last = await TryGetLast(source, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw Error.NoElements();
        }

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            Maybe<TSource> last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw Error.NoElements();
        }

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            Maybe<TSource> last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw Error.NoElements();
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            Maybe<TSource> last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw Error.NoElements();
        }
#endif
    }
}
