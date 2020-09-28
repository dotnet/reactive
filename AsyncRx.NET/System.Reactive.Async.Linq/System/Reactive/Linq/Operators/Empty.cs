// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Empty<TSource>()
        {
            return Create<TSource>(observer => AsyncObserver.Empty(observer));
        }

        public static IAsyncObservable<TSource> Empty<TSource>(IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => AsyncObserver.Empty(observer, scheduler));
        }
    }

    public partial class AsyncObserver
    {
        public static ValueTask<IAsyncDisposable> Empty<TSource>(IAsyncObserver<TSource> observer) => Empty(observer, ImmediateAsyncScheduler.Instance);

        public static ValueTask<IAsyncDisposable> Empty<TSource>(IAsyncObserver<TSource> observer, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return scheduler.ScheduleAsync(async ct =>
            {
                if (!ct.IsCancellationRequested)
                {
                    await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                }
            });
        }
    }
}
