// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public sealed class ImmediateAsyncScheduler : AsyncSchedulerBase
    {
        public static ImmediateAsyncScheduler Instance { get; } = new ImmediateAsyncScheduler();

        private ImmediateAsyncScheduler() { }

        protected override Task Delay(TimeSpan dueTime, CancellationToken token) => Task.Delay(dueTime);

        protected override Task ScheduleAsyncCore(Func<CancellationToken, Task> action, CancellationToken token) => action(token);
    }
}
