// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Return<TSource>(TSource value)
        {
            return Return(value, ImmediateAsyncScheduler.Instance);
        }

        public static IAsyncObservable<TSource> Return<TSource>(TSource value, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(observer => scheduler.ScheduleAsync(async ct =>
            {
                ct.ThrowIfCancellationRequested();

                await observer.OnNextAsync(value).RendezVous(scheduler);

                ct.ThrowIfCancellationRequested();

                await observer.OnCompletedAsync().RendezVous(scheduler);
            }));
        }
    }
}
