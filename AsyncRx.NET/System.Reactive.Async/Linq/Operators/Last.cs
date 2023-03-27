// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> LastAsync<TSource>(this IAsyncObservable<TSource> source) => Last(source);
        public static IAsyncObservable<TSource> LastAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate) => Last(source, predicate);
        public static IAsyncObservable<TSource> LastAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => Last(source, predicate);

        public static IAsyncObservable<TSource> Last<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(
                source,
                (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Last(observer)));
        }

        public static IAsyncObservable<TSource> Last<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateAsyncObservable<TSource>.From(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.Last(observer, predicate)));
        }

        public static IAsyncObservable<TSource> Last<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateAsyncObservable<TSource>.From(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.Last(observer, predicate)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Last<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var hasValue = false;
            var lastValue = default(TSource);

            return Create<TSource>(
                x =>
                {
                    hasValue = true;
                    lastValue = x;

                    return default;
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
                        await observer.OnNextAsync(lastValue).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<TSource> Last<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Last(observer), predicate);
        }

        public static IAsyncObserver<TSource> Last<TSource>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(Last(observer), predicate);
        }
    }
}
