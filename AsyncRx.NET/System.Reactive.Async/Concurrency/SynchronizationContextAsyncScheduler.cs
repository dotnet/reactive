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

        protected override ValueTask ScheduleAsyncCore<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, CancellationToken token)
        {
            // The work item is boxed once as the Post state; a static callback avoids a closure and delegate per call.
            _context.Post(static s =>
            {
                var (state, action, token) = ((TState, Func<TState, CancellationToken, ValueTask>, CancellationToken))s;

                if (!token.IsCancellationRequested)
                {
                    action(state, token);
                }
            }, (state, action, token));

            return default;
        }
    }
}
