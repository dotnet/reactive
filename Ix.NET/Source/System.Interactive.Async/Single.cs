// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
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

            return Single(source, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Single(source, predicate, CancellationToken.None);
        }


        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Single_(source, cancellationToken);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .Single(cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefault_(source, cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .SingleOrDefault(cancellationToken);
        }

        private static async Task<TSource> Single_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var list = source as IList<TSource>;
            if (list != null)
            {
                switch (list.Count)
                {
                    case 0: throw new InvalidOperationException(Strings.NO_ELEMENTS);
                    case 1: return list[0];
                }
                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
            }

            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }
                var result = e.Current;
                if (await e.MoveNext(cancellationToken)
                           .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
                }
                return result;
            }
        }

        private static async Task<TSource> SingleOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var list = source as IList<TSource>;
            if (list != null)
            {
                switch (list.Count)
                {
                    case 0: return default(TSource);
                    case 1: return list[0];
                }
                throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
            }

            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    return default(TSource);
                }

                var result = e.Current;
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    return result;
                }
            }
            throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
        }
    }
}