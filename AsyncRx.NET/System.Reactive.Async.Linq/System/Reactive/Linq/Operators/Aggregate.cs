// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Aggregate<TSource>(this IAsyncObservable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, func)));
        }

        public static IAsyncObservable<TSource> Aggregate<TSource>(this IAsyncObservable<TSource> source, Func<TSource, TSource, Task<TSource>> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, func)));
        }

        public static IAsyncObservable<TResult> Aggregate<TSource, TResult>(this IAsyncObservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, seed, func)));
        }

        public static IAsyncObservable<TResult> Aggregate<TSource, TResult>(this IAsyncObservable<TSource> source, TResult seed, Func<TResult, TSource, Task<TResult>> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, seed, func)));
        }

        public static IAsyncObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, seed, func, resultSelector)));
        }

        public static IAsyncObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, Task<TAccumulate>> func, Func<TAccumulate, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Create<TResult>(observer => source.SubscribeSafeAsync(AsyncObserver.Aggregate(observer, seed, func, resultSelector)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Aggregate<TSource>(IAsyncObserver<TSource> observer, Func<TSource, TSource, TSource> func)
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

        public static IAsyncObserver<TSource> Aggregate<TSource>(IAsyncObserver<TSource> observer, Func<TSource, TSource, Task<TSource>> func)
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

        public static IAsyncObserver<TSource> Aggregate<TSource, TResult>(IAsyncObserver<TResult> observer, TResult seed, Func<TResult, TSource, TResult> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Aggregate(observer, seed, func, a => a);
        }

        public static IAsyncObserver<TSource> Aggregate<TSource, TResult>(IAsyncObserver<TResult> observer, TResult seed, Func<TResult, TSource, Task<TResult>> func)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            return Aggregate<TSource, TResult, TResult>(observer, seed, (a, x) => func(a, x), a => Task.FromResult(a));
        }

        public static IAsyncObserver<TSource> Aggregate<TSource, TAccumulate, TResult>(IAsyncObserver<TResult> observer, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
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
                },
                observer.OnErrorAsync,
                async () =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = resultSelector(value);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> Aggregate<TSource, TAccumulate, TResult>(IAsyncObserver<TResult> observer, TAccumulate seed, Func<TAccumulate, TSource, Task<TAccumulate>> func, Func<TAccumulate, Task<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
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
                },
                observer.OnErrorAsync,
                async () =>
                {
                    var res = default(TResult);

                    try
                    {
                        res = await resultSelector(value).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
