// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, params TSource[] values) => Prepend(source, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IEnumerable<TSource> values) => Prepend(source, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values) => Prepend(source, scheduler, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values) => Prepend(source, scheduler, values);

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CreateAsyncObservable<TSource>.From(
                source,
                value,
                static async (source, value, observer) => await source.SubscribeSafeAsync(await AsyncObserver.Prepend(observer, value).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource value, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CreateAsyncObservable<TSource>.From(
                source,
                (scheduler, value),
                static (source, state, observer) => AsyncObserver.Prepend(observer, source, state.value, state.scheduler));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CreateAsyncObservable<TSource>.From(
                source,
                values,
                static async (source, values, observer) => await source.SubscribeSafeAsync(await AsyncObserver.Prepend(observer, values).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CreateAsyncObservable<TSource>.From(
                source,
                (scheduler, values),
                static (source, state, observer) => AsyncObserver.Prepend(observer, source, state.scheduler, state.values)); ;
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CreateAsyncObservable<TSource>.From(
                source,
                values,
                static async (source, values, observer) => await source.SubscribeSafeAsync(await AsyncObserver.Prepend(observer, values).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CreateAsyncObservable<TSource>.From(
                source,
                (scheduler, values),
                static (source, state, observer) => AsyncObserver.Prepend(observer, source, state.scheduler, state.values));
        }
    }

    public partial class AsyncObserver
    {
        // REVIEW: There's some asymmetry on these overloads. Should standardize to Concat style.

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, TSource value)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return CoreAsync();

            async Task<IAsyncObserver<TSource>> CoreAsync()
            {
                await observer.OnNextAsync(value).ConfigureAwait(false);

                return observer;
            }
        }

        public static ValueTask<IAsyncDisposable> Prepend<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, TSource value, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return CoreAsync();

            async ValueTask<IAsyncDisposable> CoreAsync()
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    if (ct.IsCancellationRequested)
                        return;

                    await observer.OnNextAsync(value).RendezVous(scheduler, ct);

                    if (ct.IsCancellationRequested)
                        return;

                    var inner = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
                    await subscription.AssignAsync(inner).RendezVous(scheduler, ct);
                }).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(task, subscription);
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CoreAsync();

            async Task<IAsyncObserver<TSource>> CoreAsync()
            {
                foreach (var value in values)
                {
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                }

                return observer;
            }
        }

        public static ValueTask<IAsyncDisposable> Prepend<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CoreAsync();

            async ValueTask<IAsyncDisposable> CoreAsync()
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    if (ct.IsCancellationRequested)
                        return;

                    for (var i = 0; i < values.Length && !ct.IsCancellationRequested; i++)
                    {
                        await observer.OnNextAsync(values[i]).RendezVous(scheduler, ct);
                    }

                    if (ct.IsCancellationRequested)
                        return;

                    var inner = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
                    await subscription.AssignAsync(inner).RendezVous(scheduler, ct);
                }).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(task, subscription);
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CoreAsync();

            async Task<IAsyncObserver<TSource>> CoreAsync()
            {
                foreach (var value in values)
                {
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                }

                return observer;
            }
        }

        public static ValueTask<IAsyncDisposable> Prepend<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return CoreAsync();

            async ValueTask<IAsyncDisposable> CoreAsync()
            {
                var subscription = new SingleAssignmentAsyncDisposable();

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    if (ct.IsCancellationRequested)
                        return;

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

                    if (ct.IsCancellationRequested)
                        return;

                    var inner = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
                    await subscription.AssignAsync(inner).RendezVous(scheduler, ct);
                }).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(task, subscription);
            }
        }
    }
}
