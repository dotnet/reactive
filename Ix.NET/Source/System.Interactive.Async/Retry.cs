// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new[] { source }.Repeat()
                                 .Catch();
        }

        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            return new[] { source }.Repeat(retryCount)
                                 .Catch();
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            for (var i = 0; i < count; i++)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }
}