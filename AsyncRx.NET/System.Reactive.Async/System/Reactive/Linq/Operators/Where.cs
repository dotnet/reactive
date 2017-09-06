// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<T> Where<T>(this IAsyncObservable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(observer => source.SubscribeAsync(AsyncObserver.Where(observer, predicate)));
        }

        public static IAsyncObservable<T> Where<T>(this IAsyncObservable<T> source, Func<T, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(observer => source.SubscribeAsync(AsyncObserver.Where(observer, predicate)));
        }

        public static IAsyncObservable<T> Where<T>(this IAsyncObservable<T> source, Func<T, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(observer => source.SubscribeAsync(AsyncObserver.Where(observer, predicate)));
        }

        public static IAsyncObservable<T> Where<T>(this IAsyncObservable<T> source, Func<T, int, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(observer => source.SubscribeAsync(AsyncObserver.Where(observer, predicate)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<T> Where<T>(IAsyncObserver<T> observer, Func<T, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(
                async x =>
                {
                    bool b;

                    try
                    {
                        b = predicate(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<T> Where<T>(IAsyncObserver<T> observer, Func<T, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create<T>(
                async x =>
                {
                    bool b;

                    try
                    {
                        b = await predicate(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<T> Where<T>(IAsyncObserver<T> observer, Func<T, int, bool> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int i = 0;

            return Create<T>(
                async x =>
                {
                    bool b;

                    try
                    {
                        b = predicate(x, checked(i++));
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<T> Where<T>(IAsyncObserver<T> observer, Func<T, int, Task<bool>> predicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int i = 0;

            return Create<T>(
                async x =>
                {
                    bool b;

                    try
                    {
                        b = await predicate(x, checked(i++)).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
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
