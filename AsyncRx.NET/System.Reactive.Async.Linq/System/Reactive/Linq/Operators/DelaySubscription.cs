// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> DelaySubscription<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DelaySubscription(source, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> DelaySubscription<TSource>(this IAsyncObservable<TSource> source, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    var inner = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
                    await d.AddAsync(inner).ConfigureAwait(false);
                }, dueTime).ConfigureAwait(false);

                await d.AddAsync(task).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<TSource> DelaySubscription<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DelaySubscription(source, dueTime, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> DelaySubscription<TSource>(this IAsyncObservable<TSource> source, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer =>
            {
                var d = new CompositeAsyncDisposable();

                var task = await scheduler.ScheduleAsync(async ct =>
                {
                    var inner = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
                    await d.AddAsync(inner).ConfigureAwait(false);
                }, dueTime).ConfigureAwait(false);

                await d.AddAsync(task).ConfigureAwait(false);

                return d;
            });
        }
    }
}
