// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        protected override Task Delay(TimeSpan dueTime, CancellationToken token) => Task.Delay(dueTime, token);

        protected override Task ScheduleAsyncCore(Func<CancellationToken, Task> action, CancellationToken token)
        {
            var task = _factory.StartNew(() => action(token), token);

            task.Unwrap().ContinueWith(t =>
            {
                if (!t.IsCanceled && t.Exception != null)
                {
                    // TODO: Call event handler.
                }
            });

            return Task.CompletedTask;
        }
    }
}
