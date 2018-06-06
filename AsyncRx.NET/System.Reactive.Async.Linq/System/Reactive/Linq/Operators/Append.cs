// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Append(observer, value)));
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, TSource value, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, disposable) = AsyncObserver.Append(observer, value, scheduler);

                await d.AddAsync(disposable).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(subscription).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Append(observer, values)));
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, disposable) = AsyncObserver.Append(observer, scheduler, values);

                await d.AddAsync(disposable).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(subscription).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(observer => source.SubscribeSafeAsync(AsyncObserver.Append(observer, values)));
        }

        public static IAsyncObservable<TSource> Append<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var (sink, disposable) = AsyncObserver.Append(observer, scheduler, values);

                await d.AddAsync(disposable).ConfigureAwait(false);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                await d.AddAsync(subscription).ConfigureAwait(false);

                return d;
            });
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Append<TSource>(IAsyncObserver<TSource> observer, TSource value)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(
                observer.OnNextAsync,
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Append<TSource>(IAsyncObserver<TSource> observer, TSource value, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var d = new SingleAssignmentAsyncDisposable();

            return
                (
                    Create<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        async () =>
                        {
                            var task = await scheduler.ScheduleAsync(async ct =>
                            {
                                if (!ct.IsCancellationRequested)
                                {
                                    await observer.OnNextAsync(value).RendezVous(scheduler, ct);
                                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                                }
                            }).ConfigureAwait(false);

                            await d.AssignAsync(task).ConfigureAwait(false);
                        }
                    ),
                    d
                );
        }

        public static IAsyncObserver<TSource> Append<TSource>(IAsyncObserver<TSource> observer, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(
                observer.OnNextAsync,
                observer.OnErrorAsync,
                async () =>
                {
                    foreach (var value in values)
                    {
                        await observer.OnNextAsync(value).ConfigureAwait(false);
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Append<TSource>(IAsyncObserver<TSource> observer, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var d = new SingleAssignmentAsyncDisposable();

            return
                (
                    Create<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        async () =>
                        {
                            var task = await scheduler.ScheduleAsync(async ct =>
                            {
                                for (var i = 0; i < values.Length && !ct.IsCancellationRequested; i++)
                                {
                                    await observer.OnNextAsync(values[i]).RendezVous(scheduler, ct);
                                }

                                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            }).ConfigureAwait(false);

                            await d.AssignAsync(task).ConfigureAwait(false);
                        }
                    ),
                    d
                );
        }

        public static IAsyncObserver<TSource> Append<TSource>(IAsyncObserver<TSource> observer, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(
                observer.OnNextAsync,
                observer.OnErrorAsync,
                async () =>
                {
                    foreach (var value in values)
                    {
                        await observer.OnNextAsync(value).ConfigureAwait(false);
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Append<TSource>(IAsyncObserver<TSource> observer, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var d = new SingleAssignmentAsyncDisposable();

            return
                (
                    Create<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        async () =>
                        {
                            var task = await scheduler.ScheduleAsync(async ct =>
                            {
                                var e = default(IEnumerator<TSource>);

                                try
                                {
                                    e = values.GetEnumerator();
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                                    return;
                                }

                                using (e)
                                {
                                    while (!ct.IsCancellationRequested)
                                    {
                                        var b = default(bool);
                                        var value = default(TSource);

                                        try
                                        {
                                            b = e.MoveNext();

                                            if (b)
                                            {
                                                value = e.Current;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await observer.OnErrorAsync(ex).RendezVous(scheduler, ct);
                                            return;
                                        }

                                        if (b)
                                        {
                                            await observer.OnNextAsync(value).RendezVous(scheduler, ct);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            }).ConfigureAwait(false);

                            await d.AssignAsync(task).ConfigureAwait(false);
                        }
                    ),
                    d
                );
        }
    }
}
