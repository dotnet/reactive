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
        public static IAsyncObservable<TSource> Sample<TSource, TSample>(this IAsyncObservable<TSource> source, IAsyncObservable<TSample> sampler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (sampler == null)
                throw new ArgumentNullException(nameof(sampler));

            return Create<TSource>(async observer =>
            {
                var (sourceSink, samplerSink) = AsyncObserver.Sample<TSource, TSample>(observer);

                var sourceSubscription = await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false);
                var samplerSubscription = await sampler.SubscribeSafeAsync(samplerSink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(sourceSubscription, samplerSubscription);
            });
        }

        public static IAsyncObservable<TSource> Sample<TSource>(this IAsyncObservable<TSource> source, TimeSpan interval)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer =>
            {
                var (sourceSink, sampler) = await AsyncObserver.Sample(observer, interval).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(sourceSubscription, sampler);
            });
        }

        public static IAsyncObservable<TSource> Sample<TSource>(this IAsyncObservable<TSource> source, TimeSpan interval, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var (sourceSink, sampler) = await AsyncObserver.Sample(observer, interval, scheduler).ConfigureAwait(false);

                var sourceSubscription = await source.SubscribeSafeAsync(sourceSink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(sourceSubscription, sampler);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSample>) Sample<TSource, TSample>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var gate = new AsyncLock();

            var hasValue = false;
            var value = default(TSource);
            var atEnd = false;

            async Task OnSampleAsync()
            {
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (hasValue)
                    {
                        hasValue = false;
                        await observer.OnNextAsync(value).ConfigureAwait(false);
                    }

                    if (atEnd)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
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
                                atEnd = true;
                            }
                        }
                    ),
                    Create<TSample>(
                        _ => OnSampleAsync(),
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        },
                        OnSampleAsync
                    )
                );
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Sample<TSource>(IAsyncObserver<TSource> observer, TimeSpan interval)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Sample(observer, interval, TaskPoolAsyncScheduler.Default);
        }

        public static Task<(IAsyncObserver<TSource>, IAsyncDisposable)> Sample<TSource>(IAsyncObserver<TSource> observer, TimeSpan interval, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            async Task<(IAsyncObserver<TSource>, IAsyncDisposable)> CoreAsync()
            {
                var (sink, timer) = Sample<TSource, long>(observer);

                var sampler = await Interval(timer, interval, scheduler).ConfigureAwait(false);

                return (sink, sampler);
            }

            return CoreAsync();
        }
    }
}
