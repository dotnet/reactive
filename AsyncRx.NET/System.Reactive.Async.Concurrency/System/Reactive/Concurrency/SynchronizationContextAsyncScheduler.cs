// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public sealed class SynchronizationContextAsyncScheduler : AsyncSchedulerBase
    {
        private readonly SynchronizationContext _context;

        public SynchronizationContextAsyncScheduler(SynchronizationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        protected override Task Delay(TimeSpan dueTime, CancellationToken token) => Task.Delay(dueTime, token);

        protected override Task ScheduleAsyncCore(Func<CancellationToken, Task> action, CancellationToken token)
        {
            _context.Post(_ =>
            {
                if (!token.IsCancellationRequested)
                {
                    action(token);
                }
            }, null);

            return Task.CompletedTask;
        }
    }
}
