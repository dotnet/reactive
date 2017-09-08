// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<IList<TSource>> MaxBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxBy(observer, keySelector)));
        }

        public static IAsyncObservable<IList<TSource>> MaxBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxBy(observer, keySelector, comparer)));
        }

        public static IAsyncObservable<IList<TSource>> MaxBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxBy(observer, keySelector)));
        }

        public static IAsyncObservable<IList<TSource>> MaxBy<TSource, TKey>(IAsyncObservable<TSource> source, Func<TSource, Task<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.MaxBy(observer, keySelector, comparer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> MaxBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, TKey> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MaxBy(observer, x => Task.FromResult(keySelector(x)), Comparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> MaxBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return MaxBy(observer, x => Task.FromResult(keySelector(x)), comparer);
        }

        public static IAsyncObserver<TSource> MaxBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, Task<TKey>> keySelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MaxBy(observer, keySelector, Comparer<TKey>.Default);
        }

        public static IAsyncObserver<TSource> MaxBy<TSource, TKey>(IAsyncObserver<IList<TSource>> observer, Func<TSource, Task<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var hasValue = false;
            var lastKey = default(TKey);
            var list = new List<TSource>();

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

                    var comparison = 0;

                    if (!hasValue)
                    {
                        hasValue = true;
                        lastKey = key;
                    }
                    else
                    {
                        try
                        {
                            comparison = comparer.Compare(key, lastKey);
                        }
                        catch (Exception ex)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            return;
                        }
                    }

                    if (comparison > 0)
                    {
                        lastKey = key;
                        list.Clear();
                    }

                    if (comparison >= 0)
                    {
                        list.Add(x);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(list).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
