﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Skip<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return source;
            }

            return CreateAsyncObservable<TSource>.From(
                source,
                count,
                static (source, count, observer) => source.SubscribeSafeAsync(AsyncObserver.Skip(observer, count)));
        }

        public static IAsyncObservable<TSource> Skip<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return source;
            }

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want Skip on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                duration,
                static async (source, duration, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.Skip(observer, duration).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<TSource> Skip<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (duration == TimeSpan.Zero)
            {
                return source;
            }

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want Skip on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                (duration, scheduler),
                static async (source, state, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.Skip(observer, state.duration).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Skip<TSource>(IAsyncObserver<TSource> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<TSource>(
                async x =>
                {
                    if (count == 0)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                    else
                    {
                        --count;
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Skip<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration) => Skip(observer, duration, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Skip<TSource>(IAsyncObserver<TSource> observer, TimeSpan duration, IAsyncScheduler scheduler)
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
                // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want Skip on the observer?
                // DESIGN: It seems that if an observer would be an IAsyncDisposable, this could get a bit easier ("inject" the inner disposable).

                var gate = AsyncGate.Create();
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
                        }, duration).ConfigureAwait(false)
                    );
            }
        }
    }
}
