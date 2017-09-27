// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

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

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Take(observer, count)));
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

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want Take on the observer?

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, timer) = await AsyncObserver.Take(observer, duration).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, timer);
            });
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

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want Take on the observer?

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, timer) = await AsyncObserver.Take(observer, duration).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, timer);
            });
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

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => Take(observer, duration, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Take<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want TakeUntil on the observer?
                // DESIGN: It seems that if an observer would be an IAsyncDisposable, this could get a bit easier ("inject" the inner disposable).

                var gate = new AsyncLock();

                return
                    (
                        Synchronize(observer, gate),
                        await scheduler.ScheduleAsync(async ct =>
                        {
                            if (!ct.IsCancellationRequested)
                            {
                                using (await gate.LockAsync().RendezVous(scheduler, ct))
                                {
                                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                                }
                            }
                        }, duration).ConfigureAwait(false)
                    );
            }
        }
    }
}
