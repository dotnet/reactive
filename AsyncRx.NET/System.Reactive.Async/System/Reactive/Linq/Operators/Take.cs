// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.Take(observer, count)));
        }

        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.Take(observer, duration)));
        }

        public static IAsyncObservable<TSource> Take<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.Take(observer, duration, scheduler)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Take<TSource>(IAsyncObserver<TSource> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<TSource>(
                async x =>
                {
                    var remaining = --count;

                    await observer.OnNextAsync(x).ConfigureAwait(false);

                    if (remaining == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
