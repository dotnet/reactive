// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> TakeUntil<TSource, TUntil>(this IAsyncObservable<TSource> source, IAsyncObservable<TUntil> until)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (until == null)
                throw new ArgumentNullException(nameof(until));

            return CreateAsyncObservable<TSource>.From(
                source,
                until,
                static async (source, until, observer) =>
                {
                    var (sourceObserver, untilObserver) = AsyncObserver.TakeUntil<TSource, TUntil>(observer);

                    // As in Rx.NET: the other sequence is subscribed first, so that one that fires
                    // at once preempts the source before it is subscribed, and its subscription is
                    // released when it completes, since a completion without a value is no signal.
                    var untilSubscription = await SubscribeReleasingOnCompletedAsync(until, untilObserver).ConfigureAwait(false);
                    var sourceSubscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, untilSubscription);
                });
        }

        public static IAsyncObservable<TSource> TakeUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset endTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want TakeUntil on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                endTime,
                static async (source, endTime, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.TakeUntil(observer, endTime).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<TSource> TakeUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset endTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want TakeUntil on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                (endTime, scheduler),
                static async (source, state, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.TakeUntil(observer, state.endTime, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }
        public static IAsyncObservable<TSource> TakeUntil<TSource>(this IAsyncObservable<TSource> source, Func<TSource, bool> stopPredicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (stopPredicate == null)
                throw new ArgumentNullException(nameof(stopPredicate));

            return CreateAsyncObservable<TSource>.From(
                source,
                stopPredicate,
                static (source, stopPredicate, observer) => source.SubscribeSafeAsync(AsyncObserver.TakeUntil(observer, stopPredicate)));
        }

        public static IAsyncObservable<TSource> TakeUntil<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask<bool>> stopPredicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (stopPredicate == null)
                throw new ArgumentNullException(nameof(stopPredicate));

            return CreateAsyncObservable<TSource>.From(
                source,
                stopPredicate,
                static (source, stopPredicate, observer) => source.SubscribeSafeAsync(AsyncObserver.TakeUntil(observer, stopPredicate)));
        }

        public static IAsyncObservable<TSource> TakeUntil<TSource>(this IAsyncObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CreateAsyncObservable<TSource>.From(
                source,
                cancellationToken,
                static async (source, cancellationToken, observer) =>
                {
                    // As in Rx.NET: a token already cancelled completes the result at once, and
                    // the source is never subscribed.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                        return AsyncDisposable.Nop;
                    }

                    var (sink, registration) = AsyncObserver.TakeUntil(observer, cancellationToken);

                    var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, registration);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TUntil>) TakeUntil<TSource, TUntil>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncGate();

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnNextAsync(x).ConfigureAwait(false);
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
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        () => default
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> TakeUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset endTime) => TakeUntil(observer, endTime, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> TakeUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset endTime, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                // REVIEW: May be easier to just use TakeUntil with a Timer parameter. Do we want TakeUntil on the observer?
                // DESIGN: It seems that if an observer would be an IAsyncDisposable, this could get a bit easier ("inject" the inner disposable).

                var gate = new AsyncGate();

                return
                    (
                        Synchronize(observer, gate),
                        await scheduler.ScheduleAsync((observer, scheduler, gate), endTime, static async (state, ct) =>
                        {
                            var (observer, scheduler, gate) = state;

                            if (!ct.IsCancellationRequested)
                            {
                                using (await gate.LockAsync().RendezVous(scheduler, ct))
                                {
                                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                                }
                            }
                        }).ConfigureAwait(false)
                    );
            }
        }

        public static IAsyncObserver<TSource> TakeUntil<TSource>(IAsyncObserver<TSource> observer, Func<TSource, bool> stopPredicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (stopPredicate == null)
                throw new ArgumentNullException(nameof(stopPredicate));

            return TakeUntil(observer, x => new ValueTask<bool>(stopPredicate(x)));
        }

        public static IAsyncObserver<TSource> TakeUntil<TSource>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask<bool>> stopPredicate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (stopPredicate == null)
                throw new ArgumentNullException(nameof(stopPredicate));

            // As in Rx.NET: the element is delivered first, then the predicate decides whether it
            // was the last.
            return Create<TSource>(
                async x =>
                {
                    await observer.OnNextAsync(x).ConfigureAwait(false);

                    bool shouldStop;

                    try
                    {
                        shouldStop = await stopPredicate(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (shouldStop)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) TakeUntil<TSource>(IAsyncObserver<TSource> observer, CancellationToken cancellationToken)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncGate();
            var done = false;

            async ValueTask CompleteAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (!done)
                    {
                        done = true;
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            }

            // The token's callback is synchronous, so the completion it starts runs on as detached
            // work, as the library's other token callbacks do; the gate orders it against the
            // source's notifications.
            var registration = cancellationToken.Register(
                static state => { _ = ((Func<ValueTask>)state!)(); },
                (Func<ValueTask>)CompleteAsync,
                useSynchronizationContext: false);

            var sink = Create<TSource>(
                async x =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (!done)
                        {
                            await observer.OnNextAsync(x).ConfigureAwait(false);
                        }
                    }
                },
                async ex =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (!done)
                        {
                            done = true;
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    }
                },
                CompleteAsync
            );

            return (sink, AsyncDisposable.Create(() =>
            {
                registration.Dispose();
                return default;
            }));
        }
    }
}
