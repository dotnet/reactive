// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL
using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class TaskPoolSchedulerTest
    {
        [TestMethod]
        public void TaskPool_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new TaskPoolScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule<int>(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule<int>(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule<int>(42, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => TaskPoolScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void TaskPool_Now()
        {
            var res = TaskPoolScheduler.Default.Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void TaskPool_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionDueNow()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.Zero, () => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromMilliseconds(1), () => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void TaskPool_ScheduleActionCancel()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = TaskPoolScheduler.Default;
            var set = false;
            var d = nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.Fail(); set = true; });
            d.Dispose();
            Thread.Sleep(400);
            Assert.IsFalse(set);
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
                    lock (gate)
                        n++;
                e.Set();
            });

            while (true)
            {
                lock (gate)
                    if (n >= 10)
                        break;

                Thread.Sleep(10);
            }

            d.Dispose();
            e.WaitOne();

            Assert.IsTrue(n >= 0);
        }
#endif

#if !NO_PERF
#if !NO_STOPWATCH
        [TestMethod]
        public void Stopwatch()
        {
            StopwatchTest.Run(TaskPoolScheduler.Default);
        }
#endif
#endif

        [TestMethod]
        public void TaskPool_Periodic()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = TaskPoolScheduler.Default.SchedulePeriodic(TimeSpan.FromMilliseconds(25), () =>
            {
                if (Interlocked.Increment(ref n) == 10)
                    e.Set();
            });

            if (!e.WaitOne(10000))
                Assert.Fail();

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
                        fail = true;

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

            Assert.IsFalse(fail);
        }
    }
}
#endif