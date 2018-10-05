// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Repeat<TSource>(TSource value)
        {
            return Create<TSource>(observer => AsyncObserver.Repeat(observer, value));
        }

        public static IAsyncObservable<TSource> Repeat<TSource>(TSource value, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => AsyncObserver.Repeat(observer, value, scheduler));
        }

        public static IAsyncObservable<TSource> Repeat<TSource>(TSource value, int repeatCount)
        {
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));

            return Create<TSource>(observer => AsyncObserver.Repeat(observer, value, repeatCount));
        }

        public static IAsyncObservable<TSource> Repeat<TSource>(TSource value, int repeatCount, IAsyncScheduler scheduler)
        {
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => AsyncObserver.Repeat(observer, value, repeatCount, scheduler));
        }

        public static IAsyncObservable<TSource> Repeat<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => AsyncObserver.Repeat(observer, source));
        }

        public static IAsyncObservable<TSource> Repeat<TSource>(this IAsyncObservable<TSource> source, int repeatCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));

            return Create<TSource>(observer => AsyncObserver.Repeat(observer, source, repeatCount));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, TSource value)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Repeat(observer, value, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, TSource value, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                while (!ct.IsCancellationRequested)
                {
                    await observer.OnNextAsync(value).RendezVous(scheduler, ct);
                }
            });
        }

        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, TSource value, int repeatCount)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));

            return Repeat(observer, value, repeatCount, TaskPoolAsyncScheduler.Default);
        }

        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, TSource value, int repeatCount, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                var i = 0;

                while (!ct.IsCancellationRequested && i < repeatCount)
                {
                    await observer.OnNextAsync(value).RendezVous(scheduler, ct);

                    i++;
                }

                if (i == repeatCount)
                {
                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                }
            });
        }

        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            async Task<IAsyncDisposable> CoreAsync()
            {
                var (sink, inner) = Concat(observer, Repeat(source).GetEnumerator());

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            }

            return CoreAsync();
        }

        public static Task<IAsyncDisposable> Repeat<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, int repeatCount)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (repeatCount < 0)
                throw new ArgumentNullException(nameof(repeatCount));

            async Task<IAsyncDisposable> CoreAsync()
            {
                var (sink, inner) = Concat(observer, Enumerable.Repeat(source, repeatCount).GetEnumerator());

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            }

            return CoreAsync();
        }

        private static IEnumerable<T> Repeat<T>(T value)
        {
            while (true)
            {
                yield return value;
            }
        }
    }
}
