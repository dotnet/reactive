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
        public static IAsyncObservable<TSource> SkipUntil<TSource, TUntil>(this IAsyncObservable<TSource> source, IAsyncObservable<TUntil> until)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (until == null)
                throw new ArgumentNullException(nameof(until));

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, untilObserver) = AsyncObserver.SkipUntil<TSource, TUntil>(observer);

                var sourceTask = source.SubscribeSafeAsync(sourceObserver);
                var untilTask = until.SubscribeSafeAsync(untilObserver);

                // REVIEW: Consider concurrent subscriptions.

                var d1 = await sourceTask.ConfigureAwait(false);
                var d2 = await untilTask.ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(d1, d2);
            });
        }

        public static IAsyncObservable<TSource> SkipUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset endTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want SkipUntil on the observer?

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, timer) = await AsyncObserver.SkipUntil(observer, endTime).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, timer);
            });
        }

        public static IAsyncObservable<TSource> SkipUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset endTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want SkipUntil on the observer?

            return Create<TSource>(async observer =>
            {
                var (sourceObserver, timer) = await AsyncObserver.SkipUntil(observer, endTime).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, timer);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TUntil>) SkipUntil<TSource, TUntil>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();
            var open = false;

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (open)
                                {
                                    await observer.OnNextAsync(x).ConfigureAwait(false);
                                }
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    ),
                    Create<TUntil>(
                        async y =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                open = true;
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        () => Task.CompletedTask
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> SkipUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset endTime) => SkipUntil(observer, endTime, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> SkipUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset endTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want SkipUntil on the observer?
                // DESIGN: It seems that if an observer would be an IAsyncDisposable, this could get a bit easier ("inject" the inner disposable).

                var gate = new AsyncLock();
                var open = false;

                return
                    (
                        Create<TSource>(
                            async x =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    if (open)
                                    {
                                        await observer.OnNextAsync(x).ConfigureAwait(false);
                                    }
                                }
                            },
                            async ex =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            },
                            async () =>
                            {
                                using (await gate.LockAsync().ConfigureAwait(false))
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        ),
                        await scheduler.ScheduleAsync(async ct =>
                        {
                            ct.ThrowIfCancellationRequested();

                            using (await gate.LockAsync().RendezVous(scheduler, ct))
                            {
                                open = true;
                            }
                        }, endTime).ConfigureAwait(false)
                    );
            }
        }
    }
}
