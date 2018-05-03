// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_THREAD
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
    internal class /*Default*/ConcurrencyAbstractionLayerImpl : IConcurrencyAbstractionLayer
    {
        public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime) => new Timer(action, state, Normalize(dueTime));

        public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

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

        public void Sleep(TimeSpan timeout) => System.Threading.Thread.Sleep(Normalize(timeout));

        public IStopwatch StartStopwatch() => new StopwatchImpl();

        public bool SupportsLongRunning => true;

        public void StartThread(Action<object> action, object state)
        {
            new Thread(() =>
            {
                action(state);
            }) { IsBackground = true }.Start();
        }

        private static TimeSpan Normalize(TimeSpan dueTime) => dueTime < TimeSpan.Zero ? TimeSpan.Zero : dueTime;

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
        // NB: 4/13/2017 - All target platforms for the 4.x release have the self-rooting
        //                 behavior described here, so we removed the USE_TIMER_SELF_ROOT
        //                 symbol.
        //

        private sealed class Timer : IDisposable
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
                    _timer = new System.Threading.Timer(Tick, state, dueTime, TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite));
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

            private bool IsTimerAssigned() => _timer != null;

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

        private sealed class PeriodicTimer : IDisposable
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
                _timer = new System.Threading.Timer(Tick, null, period, period);
            }

            private void Tick(object state) => _action();

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

        private sealed class FastPeriodicTimer : IDisposable
        {
            private readonly Action _action;
            private volatile bool disposed;

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
#endif
