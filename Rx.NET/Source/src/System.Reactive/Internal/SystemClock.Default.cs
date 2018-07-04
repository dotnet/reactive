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
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(1);

        public DefaultSystemClockMonitor()
            : base(DefaultPeriod)
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
        private IDisposable _timer;

        /// <summary>
        /// Use the Unix milliseconds for the current time
        /// so it can be atomically read/written without locking.
        /// </summary>
        private long _lastTimeUnixMillis;

        private EventHandler<SystemClockChangedEventArgs> _systemClockChanged;

        private const int SyncMaxRetries = 100;
        private const double SyncMaxDelta = 10;
        private const int MaxError = 100;

        /// <summary>
        /// Creates a new monitor for system clock changes with the specified polling frequency.
        /// </summary>
        /// <param name="period">Polling frequency for system clock changes.</param>
        public PeriodicTimerSystemClockMonitor(TimeSpan period)
        {
            _period = period;
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

                Disposable.TrySetSerial(ref _timer, Disposable.Empty);
            }
        }

        private void NewTimer()
        {
            Disposable.TrySetSerial(ref _timer, Disposable.Empty);

            var n = 0L;
            for (; ; )
            {
                var now = SystemClock.UtcNow.ToUnixTimeMilliseconds();
                Interlocked.Exchange(ref _lastTimeUnixMillis, now);

                Disposable.TrySetSerial(ref _timer, ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(TimeChanged, _period));

                if (Math.Abs(SystemClock.UtcNow.ToUnixTimeMilliseconds() - now) <= SyncMaxDelta)
                {
                    break;
                }
                if (Volatile.Read(ref _timer) == Disposable.Empty)
                {
                    break;
                }
                if (++n >= SyncMaxRetries)
                {
                    Task.Delay((int)SyncMaxDelta).Wait();
                }
            }
        }

        private void TimeChanged()
        {
            var newTime = SystemClock.UtcNow;
            var now = newTime.ToUnixTimeMilliseconds();
            var last = Volatile.Read(ref _lastTimeUnixMillis);

            var oldTime = (long)(last + _period.TotalMilliseconds);
            var diff = now - oldTime;
            if (Math.Abs(diff) >= MaxError)
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
