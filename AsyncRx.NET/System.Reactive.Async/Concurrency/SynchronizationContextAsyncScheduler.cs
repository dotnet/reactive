// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override ValueTask Delay(TimeSpan dueTime, CancellationToken token) => new(Task.Delay(dueTime, token));

        protected override ValueTask ScheduleAsyncCore(Func<CancellationToken, ValueTask> action, CancellationToken token)
        {
            _context.Post(_ =>
            {
                if (!token.IsCancellationRequested)
                {
                    action(token);
                }
            }, null);

            return default;
        }
    }
}
