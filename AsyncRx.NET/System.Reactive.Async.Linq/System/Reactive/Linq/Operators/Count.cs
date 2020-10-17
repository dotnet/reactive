// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<int> Count<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource, int>(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Count<TSource>(observer)));
        }

        public static IAsyncObservable<int> Count<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateAsyncObservable<int>.From(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.Count(observer, predicate)));
        }

        public static IAsyncObservable<int> Count<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateAsyncObservable<int>.From(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.Count(observer, predicate)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Count<TSource>(IAsyncObserver<int> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var count = 0;

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

        public static IAsyncObserver<TSource> Count<TSource>(IAsyncObserver<int> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Count<TSource>(observer), predicate);
        }

        public static IAsyncObserver<TSource> Count<TSource>(IAsyncObserver<int> observer, Func<TSource, ValueTask<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Count<TSource>(observer), predicate);
        }
    }
}
