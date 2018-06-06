// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<long> Timer(TimeSpan dueTime)
        {
            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime));
        }

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, scheduler));
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime)
        {
            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime));
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, scheduler));
        }

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, period));
        }

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, period, scheduler));
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, period));
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<long>(observer => AsyncObserver.Timer(observer, dueTime, period, scheduler));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, TimeSpan dueTime) => Timer(observer, dueTime, TaskPoolAsyncScheduler.Default);

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                await observer.OnNextAsync(0L).RendezVous(scheduler, ct);

                if (ct.IsCancellationRequested)
                    return;

                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            }, dueTime);

        }

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, DateTimeOffset dueTime) => Timer(observer, dueTime, TaskPoolAsyncScheduler.Default);

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                await observer.OnNextAsync(0L).RendezVous(scheduler, ct);

                if (ct.IsCancellationRequested)
                    return;

                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            }, dueTime);
        }

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, TimeSpan dueTime, TimeSpan period) => Timer(observer, dueTime, period, TaskPoolAsyncScheduler.Default);

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, TimeSpan dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var tick = 0L;

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                // TODO: Compensate for drift by adding stopwatch functionality.

                do
                {
                    await observer.OnNextAsync(tick++).RendezVous(scheduler, ct);

                    await scheduler.Delay(period, ct).RendezVous(scheduler, ct);
                } while (!ct.IsCancellationRequested);
            }, dueTime);
        }

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, DateTimeOffset dueTime, TimeSpan period) => Timer(observer, dueTime, period, TaskPoolAsyncScheduler.Default);

        public static Task<IAsyncDisposable> Timer(IAsyncObserver<long> observer, DateTimeOffset dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var tick = 0L;

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                // TODO: Compensate for drift by adding stopwatch functionality.

                do
                {
                    await observer.OnNextAsync(tick++).RendezVous(scheduler, ct);

                    await scheduler.Delay(period, ct).RendezVous(scheduler, ct);
                } while (!ct.IsCancellationRequested);
            }, dueTime);
        }
    }
}
