// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// A <see cref="HistoricalScheduler"/> that also implements <see cref="ISchedulerPeriodic"/>, so the
    /// periodic operators (<c>Interval</c>, <c>Timer(dueTime, period)</c>, <c>Sample(TimeSpan)</c>,
    /// <c>Buffer(TimeSpan)</c>, <c>Window(TimeSpan)</c>) exercise Rx's real <c>SchedulePeriodic</c> fast
    /// path in virtual time, instead of the stopwatch-emulated fallback they hit on a plain
    /// <see cref="HistoricalScheduler"/>.
    /// </summary>
    /// <remarks>
    /// <c>Scheduler.AsPeriodic()</c> resolves the service via <c>IServiceProvider.GetService</c>, which
    /// <c>VirtualTimeSchedulerBase</c> routes through its virtual <c>GetService</c> — implementing the
    /// interface alone is never discovered, hence the override below.
    /// </remarks>
    public sealed class PeriodicVirtualScheduler : HistoricalScheduler, ISchedulerPeriodic
    {
        protected override object GetService(Type serviceType) =>
            serviceType == typeof(ISchedulerPeriodic) ? this : base.GetService(serviceType);

        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Scheduler.SchedulePeriodic permits a zero period, but in virtual time that would make
            // Start() spin forever at a fixed clock. Fail fast — this is benchmark infrastructure only.
            if (period <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            return new PeriodicallyScheduledWorkItem<TState>(this, state, period, action);
        }

        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private readonly PeriodicVirtualScheduler _scheduler;
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;
            private readonly SerialDisposable _run = new();
            private TState _state;
            private DateTimeOffset _next;   // absolute due time: exact tick spacing, immune to Sleep()
            private bool _disposed;         // virtual schedulers are single-threaded; no locking needed

            public PeriodicallyScheduledWorkItem(PeriodicVirtualScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
            {
                _scheduler = scheduler;
                _state = state;
                _period = period;
                _action = action;

                // ISchedulerPeriodic contract: the first tick fires one period after the call.
                _next = scheduler.Now + period;
                _run.Disposable = scheduler.ScheduleAbsolute(this, _next, static (_, self) => self.Tick());
            }

            private IDisposable Tick()
            {
                _state = _action(_state);

                // The action may have disposed us re-entrantly (e.g. Interval(...).Take(n) completing
                // inside OnNext). Only reschedule afterwards, otherwise Start() would never terminate.
                if (!_disposed)
                {
                    _next += _period;
                    _run.Disposable = _scheduler.ScheduleAbsolute(this, _next, static (_, self) => self.Tick());
                }

                return Disposable.Empty;
            }

            public void Dispose()
            {
                _disposed = true;
                _run.Dispose();   // cancels the pending ScheduledItem so GetNext() skips it
            }
        }
    }
}
