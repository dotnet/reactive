// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Select<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, selector, observer) => source.SubscribeSafeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<TResult> Select<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, selector, observer) => source.SubscribeSafeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<TResult> Select<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, selector, observer) => source.SubscribeSafeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<TResult> Select<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, int, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateAsyncObservable<TResult>.From(
                source,
                selector,
                static (source, selector, observer) => source.SubscribeSafeAsync(AsyncObserver.Select(observer, selector)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Select<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TSource>(
                async x =>
                {
                    TResult res;

                    try
                    {
                        res = selector(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Select<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, ValueTask<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TSource>(
                async x =>
                {
                    TResult res;

                    try
                    {
                        res = await selector(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Select<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, TResult> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var i = 0;

            return Create<TSource>(
                async x =>
                {
                    TResult res;

                    try
                    {
                        res = selector(x, checked(i++));
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Select<TSource, TResult>(IAsyncObserver<TResult> observer, Func<TSource, int, ValueTask<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var i = 0;

            return Create<TSource>(
                async x =>
                {
                    TResult res;

                    try
                    {
                        res = await selector(x, checked(i++)).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
