// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Dematerialize<TSource>(this IAsyncObservable<Notification<TSource>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Dematerialize(observer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<Notification<TSource>> Dematerialize<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<Notification<TSource>>(
                n => n.AcceptAsync(observer),
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
