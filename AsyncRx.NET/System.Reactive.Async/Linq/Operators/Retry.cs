// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Retry<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // As in Rx.NET: a Catch over the source without end, through Catch's tail-recursive
            // sink, so that the chain stays one subscription deep however many times it has
            // been retried.
            return new CatchAsyncObservable<TSource>(RepeatInfinite(source));
        }

        public static IAsyncObservable<TSource> Retry<TSource>(this IAsyncObservable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            // As in Rx.NET: a Catch over retryCount copies of the source.
            return new CatchAsyncObservable<TSource>(Enumerable.Repeat(source, retryCount));
        }
    }
}
