// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Retry<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.Retry(observer, source);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }

        public static IAsyncObservable<TSource> Retry<TSource>(this IAsyncObservable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.Retry(observer, source, retryCount);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Retry<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Catch(observer, Repeat(source).GetEnumerator());
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Retry<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, int retryCount)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            return Catch(observer, Enumerable.Repeat(source, retryCount).GetEnumerator());
        }
    }
}
