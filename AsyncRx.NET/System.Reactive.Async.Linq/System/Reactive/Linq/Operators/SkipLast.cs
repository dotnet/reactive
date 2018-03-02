// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> SkipLast<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return source;
            }

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipLast(observer, count)));
        }


        public static IAsyncObservable<TSource> SkipLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return source;
            }

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipLast(observer, duration)));
        }

        public static IAsyncObservable<TSource> SkipLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            if (duration == TimeSpan.Zero)
            {
                return source;
            }

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.SkipLast(observer, duration, clock)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> SkipLast<TSource>(IAsyncObserver<TSource> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var queue = new Queue<TSource>();

            return Create<TSource>(
                async x =>
                {
                    queue.Enqueue(x);

                    if (queue.Count > count)
                    {
                        await observer.OnNextAsync(queue.Dequeue()).ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static IAsyncObserver<TSource> SkipLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => SkipLast(observer, duration, Clock.Default);

        public static IAsyncObserver<TSource> SkipLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            var queue = new Queue<Timestamped<TSource>>();

            async Task FlushAsync(DateTimeOffset now)
            {
                while (queue.Count > 0 && now - queue.Peek().Timestamp >= duration)
                {
                    await observer.OnNextAsync(queue.Dequeue().Value).ConfigureAwait(false);
                }
            }

            return Create<TSource>(
                async x =>
                {
                    var now = clock.Now;

                    queue.Enqueue(new Timestamped<TSource>(x, now));

                    await FlushAsync(now).ConfigureAwait(false);
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await FlushAsync(clock.Now).ConfigureAwait(false);

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
