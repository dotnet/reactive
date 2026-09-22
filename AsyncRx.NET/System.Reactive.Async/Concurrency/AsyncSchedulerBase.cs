// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public abstract class AsyncSchedulerBase : IAsyncScheduler
    {
        public virtual DateTimeOffset Now => DateTimeOffset.Now;

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, Func<TState, CancellationToken, ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ScheduleAsyncCore(state, action);
        }

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, TimeSpan dueTime, Func<TState, CancellationToken, ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var dueTimeRelative = Normalize(dueTime);

            return ScheduleAsyncCore((scheduler: this, state, action, dueTimeRelative), static async (s, ct) =>
            {
                await s.scheduler.Delay(s.dueTimeRelative, ct); // NB: Honor SynchronizationContext to stay on scheduler.

                await s.action(s.state, ct);
            });
        }

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, DateTimeOffset dueTime, Func<TState, CancellationToken, ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ScheduleAsyncCore((scheduler: this, state, action, dueTime), static async (s, ct) =>
            {
                var dueTimeRelative = Normalize(s.dueTime - s.scheduler.Now); // TODO: Support clock drift and clock changes.

                await s.scheduler.Delay(dueTimeRelative, ct); // NB: Honor SynchronizationContext to stay on scheduler.

                await s.action(s.state, ct);
            });
        }

        protected virtual async ValueTask<IAsyncDisposable> ScheduleAsyncCore<TState>(TState state, Func<TState, CancellationToken, ValueTask> action)
        {
            var cad = new CancellationAsyncDisposable();

            await ScheduleAsyncCore(state, action, cad.Token).ConfigureAwait(false);

            return cad;
        }

        protected abstract ValueTask ScheduleAsyncCore<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, CancellationToken token);

        protected abstract ValueTask Delay(TimeSpan dueTime, CancellationToken token);

        protected static TimeSpan Normalize(TimeSpan timeSpan) => timeSpan < TimeSpan.Zero ? TimeSpan.Zero : timeSpan;

        private sealed class CancellationAsyncDisposable : IAsyncDisposable
        {
            private readonly CancellationTokenSource _cts = new();

            public CancellationToken Token => _cts.Token;

            public ValueTask DisposeAsync()
            {
                _cts.Cancel();

                return default;
            }
        }
    }
}
