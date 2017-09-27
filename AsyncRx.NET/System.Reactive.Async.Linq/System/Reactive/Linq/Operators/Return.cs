// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Return<TSource>(TSource value)
        {
            return Create<TSource>(observer => AsyncObserver.Return(observer, value));
        }

        public static IAsyncObservable<TSource> Return<TSource>(TSource value, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => AsyncObserver.Return(observer, value, scheduler));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Return<TSource>(IAsyncObserver<TSource> observer, TSource value) => Return(observer, value, ImmediateAsyncScheduler.Instance);

        public static Task<IAsyncDisposable> Return<TSource>(IAsyncObserver<TSource> observer, TSource value, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (ct.IsCancellationRequested)
                    return;

                await observer.OnNextAsync(value).RendezVous(scheduler, ct);

                if (ct.IsCancellationRequested)
                    return;

                await observer.OnCompletedAsync().RendezVous(scheduler, ct);
            });
        }
    }
}
