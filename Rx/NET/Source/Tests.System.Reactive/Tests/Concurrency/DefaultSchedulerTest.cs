// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class DefaultSchedulerTest
    {
        [TestMethod]
        public void Schedule_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule<int>(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule<int>(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule<int>(42, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [TestMethod]
        public void Get_Now()
        {
            var res = DefaultScheduler.Instance.Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

#if !SILVERLIGHT
        [TestMethod]
        public void ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }
#endif

        [TestMethod]
        public void ScheduleActionCancel()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var set = false;
            var d = nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.Fail(); set = true; });
            d.Dispose();
            Thread.Sleep(400);
            Assert.IsFalse(set);
        }

        [TestMethod]
        public void Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = DefaultScheduler.Instance.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
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

#if DESKTOPCLR
        [TestMethod]
        public void No_ThreadPool_Starvation_Dispose()
        {
            var bwt = default(int);
            var bio = default(int);
            ThreadPool.GetAvailableThreads(out bwt, out bio);

            var N = Environment.ProcessorCount * 2;

            for (int i = 0; i < N; i++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);
                var d = Scheduler.Default.Schedule(TimeSpan.FromMilliseconds(1), () => { e.Set(); f.WaitOne(); });
                e.WaitOne();
                d.Dispose();
                f.Set();
            }

            var ewt = default(int);
            var eio = default(int);
            ThreadPool.GetAvailableThreads(out ewt, out eio);

            Assert.IsFalse(bwt - ewt >= N);
        }
#endif
    }
}
