// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Cast<TSource, TResult>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Cast<TSource, TResult>(observer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Cast<TSource, TResult>(IAsyncObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Select<TSource, TResult>(observer, x => (TResult)(object)x);
        }
    }
}
