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
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");

            //
            // The contract for periodic scheduling in Rx is that specifying TimeSpan.Zero as the period causes the scheduler to 
            // call back periodically as fast as possible, sequentially.
            //
            if (period == TimeSpan.Zero)
            {
                return new FastPeriodicTimer(action);
            }
            else
            {
                return new PeriodicTimer(action, period);
            }
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
            }) { IsBackground = true }.Start();
        }

        private static TimeSpan Normalize(TimeSpan dueTime)
        {
            if (dueTime < TimeSpan.Zero)
                return TimeSpan.Zero;

            return dueTime;
        }

#if USE_TIMER_SELF_ROOT

        //
        // Some historical context. In the early days of Rx, we discovered an issue with
        // the rooting of timers, causing them to get GC'ed even when the IDisposable of
        // a scheduled activity was kept alive. The original code simply created a timer
        // as follows:
        //
        //   var t = default(Timer);
        //   t = new Timer(_ =>
        //   {
        //       t = null;
        //       Debug.WriteLine("Hello!");
        //   }, null, 5000, Timeout.Infinite);
        //
        // IIRC the reference to "t" captured by the closure wasn't sufficient on .NET CF
        // to keep the timer rooted, causing problems on Windows Phone 7. As a result, we
        // added rooting code using a dictionary (SD 7280), which we carried forward all
        // the way to Rx v2.0 RTM.
        //
        // However, the desktop CLR's implementation of System.Threading.Timer exhibits
        // other characteristics where a timer can root itself when the timer is still
        // reachable through the state or callback parameters. To illustrate this, run
        // the following piece of code:
        //
        //   static void Main()
        //   {
        //       Bar();
        //   
        //       while (true)
        //       {
        //           GC.Collect();
        //           GC.WaitForPendingFinalizers();
        //           Thread.Sleep(100);
        //       }
        //   }
        //   
        //   static void Bar()
        //   {
        //       var t = default(Timer);
        //       t = new Timer(_ =>
        //       {
        //           t = null; // Comment out this line to see the timer stop
        //           Console.WriteLine("Hello!");
        //       }, null, 5000, Timeout.Infinite);
        //   }
        //
        // When the closure over "t" is removed, the timer will stop automatically upon
        // garbage collection. However, when retaining the reference, this problem does
        // not exist. The code below exploits this behavior, avoiding unnecessary costs
        // to root timers in a thread-safe manner.
        //
        // Below is a fragment of SOS output, proving the proper rooting:
        //
        //   !gcroot 02492440
        //    HandleTable:
        //        005a13fc (pinned handle)
        //        -> 03491010 System.Object[]
        //        -> 024924dc System.Threading.TimerQueue
        //        -> 02492450 System.Threading.TimerQueueTimer
        //        -> 02492420 System.Threading.TimerCallback
        //        -> 02492414 TimerRootingExperiment.Program+<>c__DisplayClass1
        //        -> 02492440 System.Threading.Timer
        //
        // With the USE_TIMER_SELF_ROOT symbol, we shake off this additional rooting code
        // for newer platforms where this no longer needed. We checked this on .NET Core
        // as well as .NET 4.0, and only #define this symbol for those platforms.
        //

        class Timer : IDisposable
        {
            private Action<object> _action;
            private volatile System.Threading.Timer _timer;

            public Timer(Action<object> action, object state, TimeSpan dueTime)
            {
                _action = action;

                // Don't want the spin wait in Tick to get stuck if this thread gets aborted.
                try { }
                finally
                {
                    //
                    // Rooting of the timer happens through the this.Tick delegate's target object,
                    // which is the current instance and has a field to store the Timer instance.
                    //
                    _timer = new System.Threading.Timer(this.Tick, state, dueTime, TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite));
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
                    SpinWait.SpinUntil(IsTimerAssigned);
                    Dispose();
                }
            }

            private bool IsTimerAssigned()
            {
                return _timer != null;
            }

            public void Dispose()
            {
                var timer = _timer;
                if (timer != TimerStubs.Never)
                {
                    _action = Stubs<object>.Ignore;
                    _timer = TimerStubs.Never;

                    timer.Dispose();
                }
            }
        }

        class PeriodicTimer : IDisposable
        {
            private Action _action;
            private volatile System.Threading.Timer _timer;

            public PeriodicTimer(Action action, TimeSpan period)
            {
                _action = action;

                //
                // Rooting of the timer happens through the this.Tick delegate's target object,
                // which is the current instance and has a field to store the Timer instance.
                //
                _timer = new System.Threading.Timer(this.Tick, null, period, period);
            }

            private void Tick(object state)
            {
                _action();
            }

            public void Dispose()
            {
                var timer = _timer;
                if (timer != null)
                {
                    _action = Stubs.Nop;
                    _timer = null;

                    timer.Dispose();
                }
            }
        }
#else
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
#endif

        class FastPeriodicTimer : IDisposable
        {
            private readonly Action _action;
            private bool disposed;

            public FastPeriodicTimer(Action action)
            {
                _action = action;

                new System.Threading.Thread(Loop)
                {
                    Name = "Rx-FastPeriodicTimer",
                    IsBackground = true
                }
                .Start();
            }

            private void Loop()
            {
                while (!disposed)
                {
                    _action();
                }
            }

            public void Dispose()
            {
                disposed = true;
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
            if (period <= TimeSpan.Zero)
            {
                return new FastPeriodicTimer(action);
            }
            else
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

        class FastPeriodicTimer : IDisposable
        {
            private readonly Action _action;
            private bool disposed;

            public FastPeriodicTimer(Action action)
            {
                _action = action;
                
                Task.Factory.StartNew(Loop, TaskCreationOptions.LongRunning);
            }

            private void Loop()
            {
                while (!disposed)
                {
                    _action();
                }
            }

            public void Dispose()
            {
                disposed = true;
            }
        }
    }
}
#endif