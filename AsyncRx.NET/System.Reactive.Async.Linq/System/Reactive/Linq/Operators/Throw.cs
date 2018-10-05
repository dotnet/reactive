// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Throw<TSource>(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return Create<TSource>(observer => AsyncObserver.Throw(observer, error));
        }

        public static IAsyncObservable<TSource> Throw<TSource>(Exception error, IAsyncScheduler scheduler)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => AsyncObserver.Throw(observer, error, scheduler));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncDisposable> Throw<TSource>(IAsyncObserver<TSource> observer, Exception error) => Throw(observer, error, ImmediateAsyncScheduler.Instance);

        public static Task<IAsyncDisposable> Throw<TSource>(IAsyncObserver<TSource> observer, Exception error, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (!ct.IsCancellationRequested)
                {
                    await observer.OnErrorAsync(error).RendezVous(scheduler, ct);
                }
            });
        }
    }
}
