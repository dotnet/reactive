// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<Notification<TSource>> Materialize<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<Notification<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.Materialize(observer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Materialize<TSource>(IAsyncObserver<Notification<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(
                x => observer.OnNextAsync(Notification.CreateOnNext(x)),
                async ex =>
                {
                    await observer.OnNextAsync(Notification.CreateOnError<TSource>(ex)).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                },
                async () =>
                {
                    await observer.OnNextAsync(Notification.CreateOnCompleted<TSource>()).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
