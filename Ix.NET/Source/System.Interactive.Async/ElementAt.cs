// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<TSource> ElementAt<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ElementAt_(source, index, cancellationToken);
        }


        public static Task<TSource> ElementAt<TSource>(this IAsyncEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ElementAt(source, index, CancellationToken.None);
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ElementAtOrDefault_(source, index, cancellationToken);
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ElementAtOrDefault(source, index, CancellationToken.None);
        }

        private static async Task<TSource> ElementAt_<TSource>(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            var list = source as IList<TSource>;
            if (list != null)
            {
                return list[index];
            }

            if (index >= 0)
            {
                using (var e = source.GetEnumerator())
                {
                    while (await e.MoveNext(cancellationToken)
                                  .ConfigureAwait(false))
                    {
                        if (index == 0)
                        {
                            return e.Current;
                        }

                        index--;
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        private static async Task<TSource> ElementAtOrDefault_<TSource>(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (index >= 0)
            {
                using (var e = source.GetEnumerator())
                {
                    while (await e.MoveNext(cancellationToken)
                                  .ConfigureAwait(false))
                    {
                        if (index == 0)
                        {
                            return e.Current;
                        }

                        index--;
                    }
                }
            }

            return default(TSource);
        }
    }
}