// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.Append(observer, element)));
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, TSource element, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.Append(observer, element, scheduler)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Append<TSource>(IAsyncObserver<TSource> observer, TSource element)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(
                observer.OnNextAsync,
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(element).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> Append<TSource>(IAsyncObserver<TSource> observer, TSource element, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
