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
        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return LastOrDefaultCore(source, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return LastOrDefaultCore(source, cancellationToken);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastOrDefaultCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return LastOrDefaultCore(source, predicate, cancellationToken);
        }

        private static async Task<TSource> LastOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : default;
        }

        private static async Task<TSource> LastOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : default;
        }

        private static async Task<TSource> LastOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : default;
        }

        private static ValueTask<Maybe<TSource>> TryGetLast<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is IList<TSource> list)
            {
                var count = list.Count;
                if (count > 0)
                {
                    return new ValueTask<Maybe<TSource>>(new Maybe<TSource>(list[count - 1]));
                }
            }
            else if (source is IAsyncPartition<TSource> p)
            {
                return p.TryGetLastAsync(cancellationToken);
            }
            else
            {
                return Core();

                async ValueTask<Maybe<TSource>> Core()
                {
                    var last = default(TSource);
                    var hasLast = false;

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            hasLast = true;
                            last = e.Current;
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return hasLast ? new Maybe<TSource>(last) : new Maybe<TSource>();
                }
            }

            return new ValueTask<Maybe<TSource>>(new Maybe<TSource>());
        }

        private static async Task<Maybe<TSource>> TryGetLast<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var value = e.Current;

                    if (predicate(value))
                    {
                        hasLast = true;
                        last = value;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            if (hasLast)
            {
                return new Maybe<TSource>(last);
            }

            return new Maybe<TSource>();
        }

        private static async Task<Maybe<TSource>> TryGetLast<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var value = e.Current;

                    if (await predicate(value).ConfigureAwait(false))
                    {
                        hasLast = true;
                        last = value;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            if (hasLast)
            {
                return new Maybe<TSource>(last);
            }

            return new Maybe<TSource>();
        }
    }
}
