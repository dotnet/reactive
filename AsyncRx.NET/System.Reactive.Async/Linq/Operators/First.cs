// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> FirstAsync<TSource>(this IAsyncObservable<TSource> source) => First(source);
        public static IAsyncObservable<TSource> FirstAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate) => First(source, predicate);
        public static IAsyncObservable<TSource> FirstAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => First(source, predicate);

        public static IAsyncObservable<TSource> First<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.First(observer)));
        }

        public static IAsyncObservable<TSource> First<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.First(observer, predicate)));
        }

        public static IAsyncObservable<TSource> First<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(
                source,
                predicate,
                static (source, predicate, observer) => source.SubscribeSafeAsync(AsyncObserver.First(observer, predicate)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> First<TSource>(IAsyncObserver<TSource> observer)
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
                    // Thrown and caught, as in Rx.NET, so that the error carries a stack trace
                    // that names the operator.
                    try
                    {
                        throw new InvalidOperationException("The sequence is empty.");
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                }
            );
        }

        public static IAsyncObserver<TSource> First<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(First(observer), predicate);
        }

        public static IAsyncObserver<TSource> First<TSource>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Where(First(observer), predicate);
        }
    }
}
