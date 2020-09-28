// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<long> Interval(TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return Create<long>(observer => AsyncObserver.Interval(observer, period));
        }

        public static IAsyncObservable<long> Interval(TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<long>(observer => AsyncObserver.Interval(observer, period, scheduler));
        }
    }

    public partial class AsyncObserver
    {
        public static ValueTask<IAsyncDisposable> Interval(IAsyncObserver<long> observer, TimeSpan period) => Timer(observer, period, period);

        public static ValueTask<IAsyncDisposable> Interval(IAsyncObserver<long> observer, TimeSpan period, IAsyncScheduler scheduler) => Timer(observer, period, period, scheduler);
    }
}
