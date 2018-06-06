// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<int> Range(int start, int count)
        {
            if (count < 0 || ((long)start) + count - 1 > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<int>(observer => AsyncObserver.Range(observer, start, count));
        }

        public static IAsyncObservable<int> Range(int start, int count, IAsyncScheduler scheduler)
        {
            if (count < 0 || ((long)start) + count - 1 > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<int>(observer => AsyncObserver.Range(observer, start, count, scheduler));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Range(IAsyncObserver<int> observer, int start, int count) => Range(observer, start, count, TaskPoolAsyncScheduler.Default);

        public static Task<IAsyncDisposable> Range(IAsyncObserver<int> observer, int start, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count < 0 || ((long)start) + count - 1 > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                for (int i = start, end = start + count - 1; i <= end && !ct.IsCancellationRequested; i++)
                {
                    await observer.OnNextAsync(i).RendezVous(scheduler, ct);
                }

                if (ct.IsCancellationRequested)
                    return;

                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }
    }
}
