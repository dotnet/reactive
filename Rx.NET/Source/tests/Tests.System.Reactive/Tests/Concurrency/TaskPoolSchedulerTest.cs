// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class TaskPoolSchedulerTest
    {
        [TestMethod]
        public void TaskPool_ArgumentChecking()
        {
#pragma warning disable CA1806 // (Unused new instance.) We expect the constructor to throw.
            ReactiveAssert.Throws<ArgumentNullException>(() => new TaskPoolScheduler(null));
#pragma warning restore CA1806
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(42, TimeSpan.Zero, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => TaskPoolScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void TaskPool_Now()
        {
            var res = TaskPoolScheduler.Default.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [TestMethod]
        public void TaskPool_ScheduleAction()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionDueNow()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.Zero, () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionDue()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromMilliseconds(1), () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionCancel()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var set = false;
            var d = nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.True(false); set = true; });
            d.Dispose();
            Thread.Sleep(400);
            Assert.False(set);
        }

#if !NO_PERF
        [TestMethod]
        public void TaskPool_ScheduleLongRunning()
        {
            var n = 0;
            var e = new ManualResetEvent(false);
            var gate = new object();

            var d = TaskPoolScheduler.Default.ScheduleLongRunning(42, (x, cancel) =>
            {
                while (!cancel.IsDisposed)
                {
                    lock (gate)
                    {
                        n++;
                    }
                }

                e.Set();
            });

            while (true)
            {
                lock (gate)
                {
                    if (n >= 10)
                    {
                        break;
                    }
                }

                Thread.Sleep(10);
            }

            d.Dispose();
            e.WaitOne();

            Assert.True(n >= 0);
        }
#endif

#if !NO_PERF
        [TestMethod]
        public void Stopwatch()
        {
            StopwatchTest.Run(TaskPoolScheduler.Default);
        }
#endif

        [TestMethod]
        public void TaskPool_Periodic()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = TaskPoolScheduler.Default.SchedulePeriodic(TimeSpan.FromMilliseconds(25), () =>
            {
                if (Interlocked.Increment(ref n) == 10)
                {
                    e.Set();
                }
            });

            if (!e.WaitOne(10000))
            {
                Assert.True(false);
            }

            d.Dispose();
        }

        [TestMethod]
        public void TaskPool_Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = TaskPoolScheduler.Default.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
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

        [TestMethod]
        public void TaskPool_Delay_LargerThanIntMaxValue()
        {
            var dueTime = TimeSpan.FromMilliseconds((double)int.MaxValue + 1);

            // Just ensuring the call to Schedule does not throw.
            var d = TaskPoolScheduler.Default.Schedule(dueTime, () => { });

            d.Dispose();
        }
    }
}
