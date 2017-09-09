// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> SingleAsync<TSource>(this IAsyncObservable<TSource> source) => Single(source);
        public static IAsyncObservable<TSource> SingleAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate) => Single(source, predicate);
        public static IAsyncObservable<TSource> SingleAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate) => Single(source, predicate);

        public static IAsyncObservable<TSource> Single<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Single(observer)));
        }

        public static IAsyncObservable<TSource> Single<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Single(observer, predicate)));
        }

        public static IAsyncObservable<TSource> Single<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Single(observer, predicate)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Single<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var hasValue = false;
            var value = default(TSource);

            return Create<TSource>(
                async x =>
                {
                    if (hasValue)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence contains more than one element.")).ConfigureAwait(false);
                        return;
                    }

                    hasValue = true;
                    value = x;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    if (!hasValue)
                    {
                        await observer.OnErrorAsync(new InvalidOperationException("The sequence is empty.")).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(value).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<TSource> Single<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Single(observer), predicate);
        }

        public static IAsyncObserver<TSource> Single<TSource>(IAsyncObserver<TSource> observer, Func<TSource, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Single(observer), predicate);
        }
    }
}
