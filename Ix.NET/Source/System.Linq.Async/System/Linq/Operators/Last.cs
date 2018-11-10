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
        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastCore(source, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastCore(source, cancellationToken);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastCore(source, predicate, cancellationToken);
        }

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw new InvalidOperationException(Strings.NO_ELEMENTS);
        }

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw new InvalidOperationException(Strings.NO_ELEMENTS);
        }

        private static async Task<TSource> LastCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            var last = await TryGetLast(source, predicate, cancellationToken).ConfigureAwait(false);

            return last.HasValue ? last.Value : throw new InvalidOperationException(Strings.NO_ELEMENTS);
        }
    }
}
