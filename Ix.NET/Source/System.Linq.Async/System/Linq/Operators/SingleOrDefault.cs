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
        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefaultCore(source, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefaultCore(source, cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefaultCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefaultCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefaultCore(source, predicate, cancellationToken);
        }

        private static async Task<TSource> SingleOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0: return default;
                    case 1: return list[0];
                }

                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
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

            throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
        }

        private static Task<TSource> SingleOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            return source.Where(predicate).SingleOrDefault(cancellationToken);
        }

        private static Task<TSource> SingleOrDefaultCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            return source.Where(predicate).SingleOrDefault(cancellationToken);
        }
    }
}
