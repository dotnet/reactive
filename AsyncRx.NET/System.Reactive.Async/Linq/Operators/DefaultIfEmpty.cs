// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> DefaultIfEmpty<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.DefaultIfEmpty(observer)));
        }

        public static IAsyncObservable<TSource> DefaultIfEmpty<TSource>(this IAsyncObservable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(
                source,
                defaultValue,
                static (source, defaultValue, observer) => source.SubscribeSafeAsync(AsyncObserver.DefaultIfEmpty(observer, defaultValue)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> DefaultIfEmpty<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DefaultIfEmpty(observer, default);
        }

        public static IAsyncObserver<TSource> DefaultIfEmpty<TSource>(IAsyncObserver<TSource> observer, TSource defaultValue)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var hasValue = false;

            return Create<TSource>(
                x =>
                {
                    hasValue = true;
                    return observer.OnNextAsync(x);
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!hasValue)
                    {
                        await observer.OnNextAsync(defaultValue).ConfigureAwait(false);
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
