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
        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleCore(source, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleCore(source, cancellationToken);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleCore(source, predicate, cancellationToken);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleCore(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleCore(source, predicate, cancellationToken);
        }

        private static async Task<TSource> SingleCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0: throw new InvalidOperationException(Strings.NO_ELEMENTS);
                    case 1: return list[0];
                }

                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
            }

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var result = e.Current;
                if (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
                }

                return result;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<TSource> SingleCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
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
                                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
                            }
                        }

                        return result;
                    }
                }

                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<TSource> SingleCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
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
                                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
                            }
                        }

                        return result;
                    }
                }

                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
