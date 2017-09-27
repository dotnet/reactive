// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource>(IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer)));
        }

        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource>(IAsyncObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer, comparer)));
        }

        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer, keySelector)));
        }

        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer, keySelector)));
        }

        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer, keySelector, comparer)));
        }

        public static IAsyncObservable<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.DistinctUntilChanged(observer, keySelector, comparer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DistinctUntilChanged(observer, x => x, EqualityComparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource>(IAsyncObserver<TSource> observer, IEqualityComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChanged(observer, x => x, comparer);
        }

        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctUntilChanged(observer, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, Task<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctUntilChanged(observer, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var hasCurrentKey = false;
            var currentKey = default(TKey);

            return Create<TSource>(
                async x =>
                {
                    var key = default(TKey);

                    try
                    {
                        key = keySelector(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    var equals = default(bool);

                    if (hasCurrentKey)
                    {
                        try
                        {
                            equals = comparer.Equals(currentKey, key);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (!hasCurrentKey || !equals)
                    {
                        hasCurrentKey = true;
                        currentKey = key;

                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> DistinctUntilChanged<TSource, TKey>(IAsyncObserver<TSource> observer, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var hasCurrentKey = false;
            var currentKey = default(TKey);

            return Create<TSource>(
                async x =>
                {
                    var key = default(TKey);

                    try
                    {
                        key = await keySelector(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    var equals = default(bool);

                    if (hasCurrentKey)
                    {
                        try
                        {
                            equals = comparer.Equals(currentKey, key);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (!hasCurrentKey || !equals)
                    {
                        hasCurrentKey = true;
                        currentKey = key;

                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}
