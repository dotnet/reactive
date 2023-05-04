// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ThreadPoolSchedulerTest
    {
        [TestMethod]
        public void Schedule_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ThreadPoolScheduler.Instance.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => ThreadPoolScheduler.Instance.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => ThreadPoolScheduler.Instance.Schedule(42, TimeSpan.Zero, default));
        }

        [TestMethod]
        public void Get_Now()
        {
            var res = ThreadPoolScheduler.Instance.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [TestMethod]
        public void ScheduleAction()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void ProperRooting_NoGC_SingleShot()
        {
            var cts = new CancellationTokenSource();

            new Thread(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    Thread.Sleep(50);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }).Start();

            var tp = ThreadPoolScheduler.Instance;
            var N = 100;
            var cd = new CountdownEvent(N);
            for (var i = 0; i < N; i++)
            {
                tp.Schedule(TimeSpan.FromMilliseconds(100 + i), () => { cd.Signal(); });
            }

            Assert.True(cd.Wait(TimeSpan.FromMinutes(1)));
            cts.Cancel();
        }

        [TestMethod]
        public void ProperRooting_NoGC_Periodic()
        {
            var cts = new CancellationTokenSource();

            new Thread(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    Thread.Sleep(50);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }).Start();

            var tp = ThreadPoolScheduler.Instance;
            var N = 5;
            var e = new ManualResetEvent(false);
            var n = 0;
            var d = tp.SchedulePeriodic(TimeSpan.FromMilliseconds(80), () => { if (Interlocked.Increment(ref n) == N) { e.Set(); } });

            Assert.True(e.WaitOne(TimeSpan.FromMinutes(1)));

            d.Dispose();

            cts.Cancel();
        }

        [TestMethod]
        public void ScheduleActionDueRelative()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void ScheduleActionDue0()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromTicks(0), () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void ScheduleActionDueAbsolute()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(0.2), () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void ScheduleActionCancel()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var set = false;
            var d = nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.True(false); set = true; });
            d.Dispose();
            Thread.Sleep(400);
            Assert.False(set);
        }

#if !NO_PERF

#if !WINDOWS_UWP

        [TestMethod]
        public void ScheduleLongRunning_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ThreadPoolScheduler.Instance.ScheduleLongRunning(42, default));
        }

        [TestMethod]
        public void ScheduleLongRunning()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = ThreadPoolScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.ScheduleLongRunning(42, (x, cancel) => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void ScheduleLongRunningCancel()
        {
            var nt = ThreadPoolScheduler.Instance;

            var started = new ManualResetEvent(false);
            var stopped = new ManualResetEvent(false);

            var n = 0;

            var d = nt.ScheduleLongRunning(42, (x, cancel) =>
            {
                for (n = 0; !cancel.IsDisposed; n++)
                {
                    if (n == 10)
                    {
                        started.Set();
                    }
                }

                stopped.Set();
            });

            started.WaitOne();
            d.Dispose();

            stopped.WaitOne();
            Assert.True(n >= 10);
        }

#endif

        [TestMethod]
        public void Stopwatch()
        {
            var nt = ThreadPoolScheduler.Instance;

            var sw = nt.StartStopwatch();

            var s0 = sw.Elapsed.Ticks;
            Thread.Sleep(10);
            var s1 = sw.Elapsed.Ticks;

            Assert.True(s1 > s0);
        }

        [TestMethod]
        public void Periodic_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ThreadPoolScheduler.Instance.SchedulePeriodic(0, TimeSpan.FromSeconds(1), null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => ThreadPoolScheduler.Instance.SchedulePeriodic(0, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void Periodic_Regular()
        {
            Periodic_Impl(TimeSpan.FromMilliseconds(25));
        }

#if !WINDOWS_UWP

        [TestMethod]
        public void Periodic_Zero()
        {
            Periodic_Impl(TimeSpan.Zero);
        }

#endif

        private void Periodic_Impl(TimeSpan period)
        {
            var gate = new object();
            var n = 0;
            var e = new ManualResetEvent(false);

            var lst = new List<int>();

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(0, period, x =>
            {
                lock (gate)
                {
                    if (n++ == 10)
                    {
                        e.Set();
                    }
                }

                lst.Add(x);
                return x + 1;
            });

            e.WaitOne();
            d.Dispose();

            var m = default(int);
            var k = default(int);

            var i = 0;
            do
            {
                lock (gate)
                {
                    m = n;
                }

                Thread.Sleep(50);

                lock (gate)
                {
                    k = n;
                }
            } while (m != k && i++ < 10); // Wait for Dispose to reach the timer; should be almost instantaneous due to nop'ing out of the action.

            Assert.NotEqual(10, i);

            var res = lst.ToArray();
            Assert.True(res.Length >= 10);
            Assert.True(res.Take(10).SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
            {
                try
                {
                    if (Interlocked.Increment(ref n) > 1) // Without an AsyncLock this would fail.
                    {
                        fail = true;
                    }

                    Thread.Sleep(100);

                    return x + 1;
                }
                finally
                {
                    Interlocked.Decrement(ref n);
                }
            });

            Thread.Sleep(500);
            d.Dispose();

            Assert.False(fail);
        }

#endif

#if DESKTOPCLR
        [TestCategory("SkipCI")]
        [TestMethod]
        public void No_ThreadPool_Starvation_Dispose()
        {
            ThreadPool.GetAvailableThreads(out var bwt, out var bio);

            var N = Environment.ProcessorCount * 2;

            for (var i = 0; i < N; i++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);
                var d = ThreadPoolScheduler.Instance.Schedule(TimeSpan.FromMilliseconds(1), () => { e.Set(); f.WaitOne(); });
                e.WaitOne();
                d.Dispose();
                f.Set();
            }
            ThreadPool.GetAvailableThreads(out var ewt, out var eio);

            Assert.False(bwt - ewt >= N);
        }
#endif
    }
}
