// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<R> Select<T, R>(this IAsyncObservable<T> source, Func<T, R> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<R>(observer => source.SubscribeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<R> Select<T, R>(this IAsyncObservable<T> source, Func<T, Task<R>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<R>(observer => source.SubscribeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<R> Select<T, R>(this IAsyncObservable<T> source, Func<T, int, R> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<R>(observer => source.SubscribeAsync(AsyncObserver.Select(observer, selector)));
        }

        public static IAsyncObservable<R> Select<T, R>(this IAsyncObservable<T> source, Func<T, int, Task<R>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<R>(observer => source.SubscribeAsync(AsyncObserver.Select(observer, selector)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<T> Select<T, R>(IAsyncObserver<R> observer, Func<T, R> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<T>(
                async x =>
                {
                    R res;

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

        public static IAsyncObserver<T> Select<T, R>(IAsyncObserver<R> observer, Func<T, Task<R>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<T>(
                async x =>
                {
                    R res;

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

        public static IAsyncObserver<T> Select<T, R>(IAsyncObserver<R> observer, Func<T, int, R> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            int i = 0;

            return Create<T>(
                async x =>
                {
                    R res;

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

        public static IAsyncObserver<T> Select<T, R>(IAsyncObserver<R> observer, Func<T, int, Task<R>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            int i = 0;

            return Create<T>(
                async x =>
                {
                    R res;

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
