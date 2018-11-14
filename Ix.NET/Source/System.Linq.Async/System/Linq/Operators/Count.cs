// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return CountCore(source, CancellationToken.None);
        }

        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return CountCore(source, cancellationToken);
        }

        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return CountCore(source, predicate, CancellationToken.None);
        }

        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return CountCore(source, predicate, cancellationToken);
        }

        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return CountCore(source, predicate, CancellationToken.None);
        }

        public static Task<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return CountCore(source, predicate, cancellationToken);
        }

        private static Task<int> CountCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            switch (source)
            {
                case ICollection<TSource> collection:
                    return Task.FromResult(collection.Count);
                case IAsyncIListProvider<TSource> listProv:
                    return listProv.GetCountAsync(onlyIfCheap: false, cancellationToken);
                case ICollection collection:
                    return Task.FromResult(collection.Count);
            }

            return Core();

            async Task<int> Core()
            {
                var count = 0;

                var e = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }

                return count;
            }
        }

        private static async Task<int> CountCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var count = 0;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    if (predicate(e.Current))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return count;
        }

        private static async Task<int> CountCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken)
        {
            var count = 0;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    if (await predicate(e.Current).ConfigureAwait(false))
                    {
                        checked
                        {
                            count++;
                        }
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return count;
        }
    }
}
