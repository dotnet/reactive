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
        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Any(source, predicate, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Any(source, predicate, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Any(source, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return AnyCore(source, predicate, cancellationToken);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return AnyCore(source, predicate, cancellationToken);
        }

        public static async Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var e = source.GetAsyncEnumerator();

            try
            {
                return await e.MoveNextAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<bool> AnyCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (predicate(e.Current))
                        return true;
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return false;
        }

        private static async Task<bool> AnyCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (await predicate(e.Current).ConfigureAwait(false))
                        return true;
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return false;
        }
    }
}
