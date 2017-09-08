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
        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.TakeLast(observer, count)));
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.TakeLast(observer, count, scheduler)));
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.TakeLast(observer, duration)));
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.TakeLast(observer, duration, clock)));
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IClock clock, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (duration == TimeSpan.Zero)
            {
                return Empty<TSource>();
            }

            return Create<TSource>(observer => source.SubscribeAsync(AsyncObserver.TakeLast(observer, duration, clock, scheduler)));
        }

        public static IAsyncObservable<TSource> TakeLast<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler) => TakeLast(source, duration, scheduler, scheduler);
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, int count)
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

                    while (queue.Count > 0)
                    {
                        await observer.OnNextAsync(queue.Dequeue()).ConfigureAwait(false);
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => TakeLast(observer, duration, TaskPoolAsyncScheduler.Default, TaskPoolAsyncScheduler.Default);

        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler) => TakeLast(observer, duration, scheduler, scheduler);

        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock) => TakeLast(observer, duration, clock, TaskPoolAsyncScheduler.Default);

        public static IAsyncObserver<TSource> TakeLast<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IClock clock, IAsyncScheduler scheduler)
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
