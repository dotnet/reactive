// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_THREAD
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal class DefaultConcurrencyAbstractionLayer/*Impl*/ : IConcurrencyAbstractionLayer
    {
        public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
        {
            return new Timer(action, state, Normalize(dueTime));
        }

        public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
        {
            //
            // MSDN documentation states the following:
            //
            //    "If period is zero (0) or negative one (-1) milliseconds and dueTime is positive, callback is invoked once;
            //     the periodic behavior of the timer is disabled, but can be re-enabled using the Change method."
            //
            if (period <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");

            return new PeriodicTimer(action, period);
        }

        public IDisposable QueueUserWorkItem(Action<object> action, object state)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(_ => action(_), state);
            return Disposable.Empty;
        }

#if USE_SLEEP_MS
        public void Sleep(TimeSpan timeout)
        {
            System.Threading.Thread.Sleep((int)Normalize(timeout).TotalMilliseconds);
        }
#else
        public void Sleep(TimeSpan timeout)
        {
            System.Threading.Thread.Sleep(Normalize(timeout));
        }
#endif

        public IStopwatch StartStopwatch()
        {
            return new DefaultStopwatch();
        }

        public bool SupportsLongRunning
        {
            get { return true; }
        }

        public void StartThread(Action<object> action, object state)
        {
            new Thread(() =>
            {
                action(state);
            }).Start();
        }

        private static TimeSpan Normalize(TimeSpan dueTime)
        {
            if (dueTime < TimeSpan.Zero)
                return TimeSpan.Zero;

            return dueTime;
        }

        class Timer : IDisposable
        {
            //
            // Note: the dictionary exists to "root" the timers so that they are not garbage collected and finalized while they are running.
            //
#if !NO_HASHSET
            private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();
#else
            private static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
#endif

            private Action<object> _action;
            private System.Threading.Timer _timer;

            private bool _hasAdded;
            private bool _hasRemoved;

            public Timer(Action<object> action, object state, TimeSpan dueTime)
            {
                _action = action;
                _timer = new System.Threading.Timer(Tick, state, dueTime, TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite));

                lock (s_timers)
                {
                    if (!_hasRemoved)
                    {
#if !NO_HASHSET
                        s_timers.Add(_timer);
#else
                        s_timers.Add(_timer, null);
#endif

                        _hasAdded = true;
                    }
                }
            }

            private void Tick(object state)
            {
                try
                {
                    _action(state);
                }
                finally
                {
                    Dispose();
                }
            }

            public void Dispose()
            {
                _action = Stubs<object>.Ignore;

                var timer = default(System.Threading.Timer);

                lock (s_timers)
                {
                    if (!_hasRemoved)
                    {
                        timer = _timer;
                        _timer = null;

                        if (_hasAdded && timer != null)
                            s_timers.Remove(timer);

                        _hasRemoved = true;
                    }
                }

                if (timer != null)
                    timer.Dispose();
            }
        }

        class PeriodicTimer : IDisposable
        {
            //
            // Note: the dictionary exists to "root" the timers so that they are not garbage collected and finalized while they are running.
            //
#if !NO_HASHSET
            private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();
#else
            private static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
#endif

            private Action _action;
            private System.Threading.Timer _timer;

            public PeriodicTimer(Action action, TimeSpan period)
            {
                _action = action;
                _timer = new System.Threading.Timer(Tick, null, period, period);

                lock (s_timers)
                {
#if !NO_HASHSET
                    s_timers.Add(_timer);
#else
                    s_timers.Add(_timer, null);
#endif
                }
            }

            private void Tick(object state)
            {
                _action();
            }

            public void Dispose()
            {
                var timer = default(System.Threading.Timer);

                lock (s_timers)
                {
                    timer = _timer;
                    _timer = null;

                    if (timer != null)
                        s_timers.Remove(timer);
                }

                if (timer != null)
                {
                    timer.Dispose();
                    _action = Stubs.Nop;
                }
            }
        }
    }
}
#else
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    internal class DefaultConcurrencyAbstractionLayer : IConcurrencyAbstractionLayer
    {
        public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
        {
            var cancel = new CancellationDisposable();
            Task.Delay(dueTime, cancel.Token).ContinueWith(
                _ => action(state),
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion
            );
            return cancel;
        }

        public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
        {
            var cancel = new CancellationDisposable();

            var moveNext = default(Action);
            moveNext = () =>
            {
                Task.Delay(period, cancel.Token).ContinueWith(
                    _ =>
                    {
                        moveNext();
                        action();
                    },
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion
                );
            };

            moveNext();

            return cancel;
        }

        public IDisposable QueueUserWorkItem(Action<object> action, object state)
        {
            var cancel = new CancellationDisposable();
            Task.Factory.StartNew(action, state, cancel.Token);
            return cancel;
        }
        
        public void Sleep(TimeSpan timeout)
        {
            Task.Delay(timeout).Wait();
        }

        public IStopwatch StartStopwatch()
        {
            return new DefaultStopwatch();
        }

        public bool SupportsLongRunning
        {
            get { return true; }
        }

        public void StartThread(Action<object> action, object state)
        {
            Task.Factory.StartNew(() =>
            {
                action(state);
            }, TaskCreationOptions.LongRunning);
        }
    }
}
#endif