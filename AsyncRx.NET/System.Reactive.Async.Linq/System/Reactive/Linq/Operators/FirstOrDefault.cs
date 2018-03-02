// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncObservable<TSource> source) => FirstOrDefault(source);
        public static IAsyncObservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate) => FirstOrDefault(source, predicate);
        public static IAsyncObservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate) => FirstOrDefault(source, predicate);

        public static IAsyncObservable<TSource> FirstOrDefault<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.FirstOrDefault(observer)));
        }

        public static IAsyncObservable<TSource> FirstOrDefault<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.FirstOrDefault(observer, predicate)));
        }

        public static IAsyncObservable<TSource> FirstOrDefault<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.FirstOrDefault(observer, predicate)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> FirstOrDefault<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(
                async x =>
                {
                    await observer.OnNextAsync(x).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(default(TSource)).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> FirstOrDefault<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(FirstOrDefault(observer), predicate);
        }

        public static IAsyncObserver<TSource> FirstOrDefault<TSource>(IAsyncObserver<TSource> observer, Func<TSource, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(FirstOrDefault(observer), predicate);
        }
    }
}
