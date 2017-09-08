// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> ElementAtOrDefault<TSource>(this IAsyncObservable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.ElementAtOrDefault(observer, index)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ElementAtOrDefault<TSource>(IAsyncObserver<TSource> observer, int index)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Create<TSource>(
                async x =>
                {
                    if (index-- == 0)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(default(TSource)).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
