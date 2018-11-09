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
        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefaultCore(source, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefaultCore(source, cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefaultCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefaultCore(source, predicate, cancellationToken);
        }

        private static async Task<TSource> FirstOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var first = await TryGetFirst(source, cancellationToken).ConfigureAwait(false);

            return first.HasValue ? first.Value : default;
        }

        private static async Task<TSource> FirstOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var first = await TryGetFirst(source, predicate, cancellationToken).ConfigureAwait(false);

            return first.HasValue ? first.Value : default;
        }

        private static async Task<TSource> FirstOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            var first = await TryGetFirst(source, predicate, cancellationToken).ConfigureAwait(false);

            return first.HasValue ? first.Value : default;
        }

        private static async Task<Maybe<TSource>> TryGetFirst<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return new Maybe<TSource>(list[0]);
                }
            }
            else if (source is IAsyncPartition<TSource> p)
            {
                return await p.TryGetFirstAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    if (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        return new Maybe<TSource>(e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
            }

            return new Maybe<TSource>();
        }

        private static async Task<Maybe<TSource>> TryGetFirst<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var value = e.Current;

                    if (predicate(value))
                    {
                        return new Maybe<TSource>(value);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return new Maybe<TSource>();
        }

        private static async Task<Maybe<TSource>> TryGetFirst<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var value = e.Current;

                    if (await predicate(value).ConfigureAwait(false))
                    {
                        return new Maybe<TSource>(value);
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return new Maybe<TSource>();
        }
    }
}
