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
        public static IAsyncObservable<TSource> SkipUntil<TSource, TUntil>(this IAsyncObservable<TSource> source, IAsyncObservable<TUntil> until)
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
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();
                    var untilSubscription = new SingleAssignmentAsyncDisposable();

                    var (sourceObserver, untilObserver) = AsyncObserver.SkipUntil<TSource, TUntil>(observer, sourceSubscription, untilSubscription);

                    // As in Rx.NET: the other sequence is subscribed first, so that one that fires
                    // at once opens the gate before the source is subscribed. A sequence that
                    // fires or completes during its own subscribe disposes its holder before it is
                    // assigned, which disposes the subscription the moment it is.
                    await untilSubscription.AssignAsync(await until.SubscribeSafeAsync(untilObserver).ConfigureAwait(false)).ConfigureAwait(false);
                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, untilSubscription);
                });
        }

        public static IAsyncObservable<TSource> SkipUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset startTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want SkipUntil on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                startTime,
                static async (source, startTime, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.SkipUntil(observer, startTime).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }

        public static IAsyncObservable<TSource> SkipUntil<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset startTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // REVIEW: May be easier to just use SkipUntil with a Timer parameter. Do we want SkipUntil on the observer?

            return CreateAsyncObservable<TSource>.From(
                source,
                (scheduler, startTime),
                static async (source, state, observer) =>
                {
                    var (sourceObserver, timer) = await AsyncObserver.SkipUntil(observer, state.startTime, state.scheduler).ConfigureAwait(false);

                    var subscription = await source.SubscribeSafeAsync(sourceObserver).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(subscription, timer);
                });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TUntil>) SkipUntil<TSource, TUntil>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, IAsyncDisposable untilSubscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (untilSubscription == null)
                throw new ArgumentNullException(nameof(untilSubscription));

            // As in Rx.NET: the other sequence's first element opens the gate and ends its own
            // subscription, as does its completion (which opens nothing); its error fails the
            // result. Before the gate opens the source's elements are dropped and its completion
            // only ends its subscription, so the result never completes; its error is forwarded.
            var gate = new AsyncGate();
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
                                if (open)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                                else
                                {
                                    await sourceSubscription.DisposeAsync().ConfigureAwait(false);
                                }
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

                            await untilSubscription.DisposeAsync().ConfigureAwait(false);
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        () => untilSubscription.DisposeAsync()
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> SkipUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset startTime) => SkipUntil(observer, startTime, TaskPoolAsyncScheduler.Default);

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> SkipUntil<TSource>(IAsyncObserver<TSource> observer, DateTimeOffset startTime, IAsyncScheduler scheduler)
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

                var gate = new AsyncGate();
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
                        }, startTime).ConfigureAwait(false)
                    );
            }
        }
    }
}
