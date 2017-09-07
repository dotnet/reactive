// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<long> Timer(TimeSpan dueTime) => Timer(dueTime, TaskPoolAsyncScheduler.Default);

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime) => Timer(dueTime, TaskPoolAsyncScheduler.Default);

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, TimeSpan period) => Timer(dueTime, period, TaskPoolAsyncScheduler.Default);

        public static IAsyncObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period) => Timer(dueTime, period, TaskPoolAsyncScheduler.Default);

        public static IAsyncObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IAsyncScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
