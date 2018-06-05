// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// Use the Unix milliseconds for the current time
        /// so it can be atomically read/written without locking.
        /// </summary>
        private long _lastTimeUnixMillis;

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

            var n = 0L;
            for (; ; )
            {
                var now = SystemClock.UtcNow.ToUnixTimeMilliseconds();
                Interlocked.Exchange(ref _lastTimeUnixMillis, now);

                _timer.Disposable = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(TimeChanged, _period);

                if (Math.Abs(SystemClock.UtcNow.ToUnixTimeMilliseconds() - now) <= SYNC_MAXDELTA)
                {
                    break;
                }
                if (_timer.Disposable == Disposable.Empty)
                {
                    break;
                }
                if (++n >= SYNC_MAXRETRIES)
                {
                    Task.Delay((int)SYNC_MAXDELTA).Wait();
                }
            };
        }

        private void TimeChanged()
        {
            var newTime = SystemClock.UtcNow;
            var now = newTime.ToUnixTimeMilliseconds();
            var last = Volatile.Read(ref _lastTimeUnixMillis);

            var oldTime = (long)(last + _period.TotalMilliseconds);
            var diff = now - oldTime;
            if (Math.Abs(diff) >= MAXERROR)
            {
                _systemClockChanged?.Invoke(this, new SystemClockChangedEventArgs(
                    DateTimeOffset.FromUnixTimeMilliseconds(oldTime), newTime));

                NewTimer();
            }
            else
            {
                Interlocked.Exchange(ref _lastTimeUnixMillis, SystemClock.UtcNow.ToUnixTimeMilliseconds());
            }
        }
    }
}
