// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class NewThreadSchedulerTest
    {
        [TestMethod]
        public void NewThread_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new NewThreadScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule<int>(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule<int>(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule<int>(42, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.SchedulePeriodic<int>(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.ScheduleLongRunning<int>(42, default(Action<int, ICancelable>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => NewThreadScheduler.Default.SchedulePeriodic<int>(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void NewThread_Now()
        {
            var res = NewThreadScheduler.Default.Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void NewThread_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

#if !SILVERLIGHT
        [TestMethod]
        [Ignore]
        public void NewThread_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            var sw = new Stopwatch();
            sw.Start();
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
            Assert.IsTrue(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif

#if !NO_PERF
#if !NO_STOPWATCH
        [TestMethod]
        public void Stopwatch()
        {
            StopwatchTest.Run(NewThreadScheduler.Default);
        }
#endif
#endif

        [TestMethod]
        public void NewThread_Periodic()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = NewThreadScheduler.Default.SchedulePeriodic(TimeSpan.FromMilliseconds(25), () =>
            {
                if (Interlocked.Increment(ref n) == 10)
                    e.Set();
            });

            if (!e.WaitOne(10000))
                Assert.Fail();

            d.Dispose();
        }

        [TestMethod]
        public void NewThread_Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = NewThreadScheduler.Default.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
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
