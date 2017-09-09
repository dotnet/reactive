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
        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.TakeLastBuffer(observer, count)));
        }

        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.TakeLastBuffer(observer, duration)));
        }

        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            if (duration == TimeSpan.Zero)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.TakeLastBuffer(observer, duration, clock)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var queue = new Queue<TSource>();

            return Create<TSource>(
                x =>
                {
                    queue.Enqueue(x);

                    if (queue.Count > count)
                    {
                        queue.Dequeue();
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    var n = queue.Count;

                    var res = new List<TSource>(n);

                    while (queue.Count > 0)
                    {
                        res.Add(queue.Dequeue());
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan duration) => TakeLastBuffer(observer, duration, Clock.Default);

        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan duration, IClock clock)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            var queue = new Queue<Timestamped<TSource>>();

            void Trim(DateTimeOffset now)
            {
                while (queue.Count > 0 && now - queue.Peek().Timestamp >= duration)
                {
                    queue.Dequeue();
                }
            }

            return Create<TSource>(
                x =>
                {
                    var now = clock.Now;

                    queue.Enqueue(new Timestamped<TSource>(x, now));

                    Trim(now);

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    Trim(clock.Now);

                    var n = queue.Count;

                    var res = new List<TSource>(n);

                    while (queue.Count > 0)
                    {
                        res.Add(queue.Dequeue().Value);
                    }

                    await observer.OnNextAsync(res).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}
