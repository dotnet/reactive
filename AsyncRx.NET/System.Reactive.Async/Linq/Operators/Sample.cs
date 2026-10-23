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
        public static IAsyncObservable<TSource> Sample<TSource, TSample>(this IAsyncObservable<TSource> source, IAsyncObservable<TSample> sampler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (sampler == null)
                throw new ArgumentNullException(nameof(sampler));

            return CreateAsyncObservable<TSource>.From(
                source,
                sampler,
                static async (source, sampler, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();
                    var samplerSubscription = new SingleAssignmentAsyncDisposable();

                    var (sourceSink, samplerSink) = AsyncObserver.Sample<TSource, TSample>(observer, sourceSubscription, samplerSubscription);

                    // A sequence that completes during its own subscribe disposes its holder
                    // before it is assigned, which disposes the subscription the moment it is.
                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false)).ConfigureAwait(false);
                    await samplerSubscription.AssignAsync(await sampler.SubscribeSafeAsync(samplerSink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, samplerSubscription);
                });
        }

        public static IAsyncObservable<TSource> Sample<TSource>(this IAsyncObservable<TSource> source, TimeSpan interval)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));

            return CreateAsyncObservable<TSource>.From(
                source,
                interval,
                static async (source, interval, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sourceSink, sampler) = await AsyncObserver.Sample(observer, sourceSubscription, interval).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, sampler);
                });
        }

        public static IAsyncObservable<TSource> Sample<TSource>(this IAsyncObservable<TSource> source, TimeSpan interval, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (scheduler, interval),
                static async (source, state, observer) =>
                {
                    var sourceSubscription = new SingleAssignmentAsyncDisposable();

                    var (sourceSink, sampler) = await AsyncObserver.Sample(observer, sourceSubscription, state.interval, state.scheduler).ConfigureAwait(false);

                    await sourceSubscription.AssignAsync(await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(sourceSubscription, sampler);
                });
        }
    }

    public partial class AsyncObserver
    {
        // The sink for the sampler forms. It observes the source and the sampler; each sequence's
        // subscription (or a holder for it) is passed in, since each side's completion ends its
        // own subscription while the other goes on, as in Rx.NET.
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSample>) Sample<TSource, TSample>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, IAsyncDisposable samplerSubscription)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (samplerSubscription == null)
                throw new ArgumentNullException(nameof(samplerSubscription));

            // As in Rx.NET: a sample (the sampler's element or completion) forwards the latest
            // element since the last. The source's completion ends its subscription at once and
            // is forwarded with the next sample; the sampler's completion ends its subscription
            // and leaves the source to complete the result by itself. Either's error fails the
            // result.
            var gate = new AsyncGate();

            var hasValue = false;
            var value = default(TSource);
            var sourceAtEnd = false;
            var samplerAtEnd = false;

            async ValueTask OnSampleAsync()
            {
                if (hasValue)
                {
                    hasValue = false;
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                }

                if (sourceAtEnd)
                {
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            }

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                hasValue = true;
                                value = x;
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
                                sourceAtEnd = true;

                                if (samplerAtEnd)
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
                    Create<TSample>(
                        async _ =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await OnSampleAsync().ConfigureAwait(false);
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
                                samplerAtEnd = true;

                                await OnSampleAsync().ConfigureAwait(false);

                                if (!sourceAtEnd)
                                {
                                    await samplerSubscription.DisposeAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Sample<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan interval)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));

            return Sample(observer, sourceSubscription, interval, TaskPoolAsyncScheduler.Default);
        }

        // The timed form: the sampler sink over a periodic timer, as in Rx.NET. The returned
        // disposable is the timer's.
        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Sample<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable sourceSubscription, TimeSpan interval, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (sourceSubscription == null)
                throw new ArgumentNullException(nameof(sourceSubscription));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var timer = new SingleAssignmentAsyncDisposable();

                var (sink, tick) = Sample<TSource, long>(observer, sourceSubscription, timer);

                await timer.AssignAsync(await Interval(tick, interval, scheduler).ConfigureAwait(false)).ConfigureAwait(false);

                return (sink, timer);
            }

            return CoreAsync();
        }
    }
}
