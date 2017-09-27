// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Scan<TSource>(this IAsyncObservable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Scan(observer, func)));
        }

        public static IAsyncObservable<TSource> Scan<TSource>(this IAsyncObservable<TSource> source, Func<TSource, TSource, Task<TSource>> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Scan(observer, func)));
        }

        public static IAsyncObservable<TResult> Scan<TSource, TResult>(this IAsyncObservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Scan(observer, seed, func)));
        }

        public static IAsyncObservable<TResult> Scan<TSource, TResult>(this IAsyncObservable<TSource> source, TResult seed, Func<TResult, TSource, Task<TResult>> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Scan(observer, seed, func)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Scan<TSource>(IAsyncObserver<TSource> observer, Func<TSource, TSource, TSource> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var hasValue = false;
            var value = default(TSource);

            return Create<TSource>(
                async x =>
                {
                    if (hasValue)
                    {
                        try
                        {
                            value = func(value, x);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }
                    else
                    {
                        value = x;
                        hasValue = true;
                    }

                    await observer.OnNextAsync(value).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Scan<TSource>(IAsyncObserver<TSource> observer, Func<TSource, TSource, Task<TSource>> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var hasValue = false;
            var value = default(TSource);

            return Create<TSource>(
                async x =>
                {
                    if (hasValue)
                    {
                        try
                        {
                            value = await func(value, x).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }
                    else
                    {
                        value = x;
                        hasValue = true;
                    }

                    await observer.OnNextAsync(value).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Scan<TSource, TResult>(IAsyncObserver<TResult> observer, TResult seed, Func<TResult, TSource, TResult> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var value = seed;

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        value = func(value, x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(value).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Scan<TSource, TResult>(IAsyncObserver<TResult> observer, TResult seed, Func<TResult, TSource, Task<TResult>> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var value = seed;

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        value = await func(value, x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(value).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
