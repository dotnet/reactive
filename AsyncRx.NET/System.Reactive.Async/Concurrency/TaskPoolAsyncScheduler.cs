// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public sealed class TaskPoolAsyncScheduler : AsyncSchedulerBase
    {
        private readonly TaskFactory _factory;

        public static TaskPoolAsyncScheduler Current { get; } = new TaskPoolAsyncScheduler(TaskScheduler.Current);
        public static TaskPoolAsyncScheduler Default { get; } = new TaskPoolAsyncScheduler(TaskScheduler.Default);

        public TaskPoolAsyncScheduler(TaskScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            _factory = new TaskFactory(scheduler);
        }

        public TaskPoolAsyncScheduler(TaskFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        protected override ValueTask Delay(TimeSpan dueTime, CancellationToken token) => new(Task.Delay(dueTime, token));

        protected override ValueTask ScheduleAsyncCore<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, CancellationToken token)
        {
            // The work item is boxed once as the StartNew state; a static callback avoids a closure and delegate per call.
            var task = _factory.StartNew(static s =>
            {
                var (state, action, token) = ((TState, Func<TState, CancellationToken, ValueTask>, CancellationToken))s;

                return action(state, token).AsTask();
            }, (state, action, token), token);

            task.Unwrap().ContinueWith(static t =>
            {
                if (!t.IsCanceled && t.Exception != null)
                {
                    // TODO: Call event handler.
                }
            });

            return default;
        }
    }
}
