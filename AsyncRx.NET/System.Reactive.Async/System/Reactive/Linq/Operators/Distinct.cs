// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Distinct<TSource>(IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer)));
        }

        public static IAsyncObservable<TSource> Distinct<TSource>(IAsyncObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Distinct(observer, comparer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Distinct<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Distinct(observer, EqualityComparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> Distinct<TSource>(IAsyncObserver<TSource> observer, IEqualityComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var set = new HashSet<TSource>(comparer);

            return Create<TSource>(
                async x =>
                {
                    var added = false;

                    try
                    {
                        added = set.Add(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (added)
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
