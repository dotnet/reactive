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

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ScheduleAsyncCore(action);
        }

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action, TimeSpan dueTime)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var dueTimeRelative = Normalize(dueTime);

            return ScheduleAsyncCore(async ct =>
            {
                await Delay(dueTimeRelative, ct); // NB: Honor SynchronizationContext to stay on scheduler.

                await action(ct);
            });
        }

        public virtual ValueTask<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, ValueTask> action, DateTimeOffset dueTime)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ScheduleAsyncCore(async ct =>
            {
                var dueTimeRelative = Normalize(dueTime - Now); // TODO: Support clock drift and clock changes.

                await Delay(dueTimeRelative, ct); // NB: Honor SynchronizationContext to stay on scheduler.

                await action(ct);
            });
        }

        protected virtual async ValueTask<IAsyncDisposable> ScheduleAsyncCore(Func<CancellationToken, ValueTask> action)
        {
            var cad = new CancellationAsyncDisposable();

            await ScheduleAsyncCore(action, cad.Token).ConfigureAwait(false);

            return cad;
        }

        protected abstract ValueTask ScheduleAsyncCore(Func<CancellationToken, ValueTask> action, CancellationToken token);

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
