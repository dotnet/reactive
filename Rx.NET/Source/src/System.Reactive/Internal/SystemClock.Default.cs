// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Provides access to the local system clock.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DefaultSystemClock : ISystemClock
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }

    internal class DefaultSystemClockMonitor : PeriodicTimerSystemClockMonitor
    {
        private static readonly TimeSpan DEFAULT_PERIOD = TimeSpan.FromSeconds(1);

        public DefaultSystemClockMonitor()
            : base(DEFAULT_PERIOD)
        {
        }
    }

    /// <summary>
    /// (Infrastructure) Monitors for system clock changes based on a periodic timer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PeriodicTimerSystemClockMonitor : INotifySystemClockChanged
    {
        private readonly TimeSpan _period;
        private readonly SerialDisposable _timer;

        private DateTimeOffset _lastTime;
        private EventHandler<SystemClockChangedEventArgs> _systemClockChanged;

        private const int SYNC_MAXRETRIES = 100;
        private const double SYNC_MAXDELTA = 10;
        private const int MAXERROR = 100;

        /// <summary>
        /// Creates a new monitor for system clock changes with the specified polling frequency.
        /// </summary>
        /// <param name="period">Polling frequency for system clock changes.</param>
        public PeriodicTimerSystemClockMonitor(TimeSpan period)
        {
            _period = period;
            _timer = new SerialDisposable();
        }

        /// <summary>
        /// Event that gets raised when a system clock change is detected.
        /// </summary>
        public event EventHandler<SystemClockChangedEventArgs> SystemClockChanged
        {
            add
            {
                NewTimer();

                _systemClockChanged += value;
            }

            remove
            {
                _systemClockChanged -= value;

                _timer.Disposable = Disposable.Empty;
            }
        }

        private void NewTimer()
        {
            _timer.Disposable = Disposable.Empty;

            var n = 0;
            do
            {
                _lastTime = SystemClock.UtcNow;
                _timer.Disposable = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(TimeChanged, _period);
            } while (Math.Abs((SystemClock.UtcNow - _lastTime).TotalMilliseconds) > SYNC_MAXDELTA && ++n < SYNC_MAXRETRIES);

            if (n >= SYNC_MAXRETRIES)
                throw new InvalidOperationException(Strings_Core.FAILED_CLOCK_MONITORING);
        }

        private void TimeChanged()
        {
            var now = SystemClock.UtcNow;
            var diff = now - (_lastTime + _period);
            if (Math.Abs(diff.TotalMilliseconds) >= MAXERROR)
            {
                _systemClockChanged?.Invoke(this, new SystemClockChangedEventArgs(_lastTime + _period, now));

                NewTimer();
            }
            else
            {
                _lastTime = SystemClock.UtcNow;
            }
        }
    }
}
