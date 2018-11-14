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
        public static Task<TSource> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<TSource> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }
    }
}
