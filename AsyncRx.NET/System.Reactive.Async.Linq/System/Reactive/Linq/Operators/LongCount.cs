// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<long> LongCount<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<long>(observer => source.SubscribeSafeAsync(AsyncObserver.LongCount<TSource>(observer)));
        }

        public static IAsyncObservable<long> LongCount<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<long>(observer => source.SubscribeSafeAsync(AsyncObserver.LongCount(observer, predicate)));
        }

        public static IAsyncObservable<long> LongCount<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<long>(observer => source.SubscribeSafeAsync(AsyncObserver.LongCount(observer, predicate)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> LongCount<TSource>(IAsyncObserver<long> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var count = 0L;

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(count).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> LongCount<TSource>(IAsyncObserver<long> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(LongCount<TSource>(observer), predicate);
        }

        public static IAsyncObserver<TSource> LongCount<TSource>(IAsyncObserver<long> observer, Func<TSource, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(LongCount<TSource>(observer), predicate);
        }
    }
}
