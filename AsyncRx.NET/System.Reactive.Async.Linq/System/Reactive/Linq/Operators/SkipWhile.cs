// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> SkipWhile<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipWhile(observer, predicate)));
        }

        public static IAsyncObservable<TSource> SkipWhile<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipWhile(observer, predicate)));
        }

        public static IAsyncObservable<TSource> SkipWhile<TSource>(this IAsyncObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipWhile(observer, predicate)));
        }

        public static IAsyncObservable<TSource> SkipWhile<TSource>(this IAsyncObservable<TSource> source, Func<TSource, int, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipWhile(observer, predicate)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> SkipWhile<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            bool open = false;

            return Create<TSource>(
                async x =>
                {
                    if (!open)
                    {
                        try
                        {
                            open = !predicate(x);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (open)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> SkipWhile<TSource>(IAsyncObserver<TSource> observer, Func<TSource, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            bool open = false;

            return Create<TSource>(
                async x =>
                {
                    if (!open)
                    {
                        try
                        {
                            open = !await predicate(x).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (open)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> SkipWhile<TSource>(IAsyncObserver<TSource> observer, Func<TSource, int, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            bool open = false;
            int i = 0;

            return Create<TSource>(
                async x =>
                {
                    if (!open)
                    {
                        try
                        {
                            open = !predicate(x, checked(i++));
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (open)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> SkipWhile<TSource>(IAsyncObserver<TSource> observer, Func<TSource, int, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            bool open = false;
            int i = 0;

            return Create<TSource>(
                async x =>
                {
                    if (!open)
                    {
                        try
                        {
                            open = !await predicate(x, checked(i++)).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (open)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
