// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class CurrentThreadSchedulerTest
    {
        [TestMethod]
        public void CurrentThread_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(TimeSpan), default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(DateTimeOffset), default(Func<IScheduler, int, IDisposable>)));
        }

        [TestMethod]
        public void CurrentThread_Now()
        {
            var res = Scheduler.CurrentThread.Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void CurrentThread_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() => { Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.IsTrue(ran);
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                Scheduler.CurrentThread.Schedule(() => { throw ex; });
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreSame(e, ex);
            }
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionNested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() => {
                Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId);
                Scheduler.CurrentThread.Schedule(() => { ran = true; });
            });
            Assert.IsTrue(ran);
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionNested_TimeSpan()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() =>
            {
                Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId);
                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(1), () => { ran = true; });
            });
            Assert.IsTrue(ran);
        }

#if !SILVERLIGHT

        [TestMethod]
        [Ignore]
        public void CurrentThread_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }

        [TestMethod]
        [Ignore]
        public void CurrentThread_ScheduleActionDueNested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () => {
                sw.Stop();
                Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId);
                sw.Start();
                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () => {
                    sw.Stop();
                    ran = true;
                });
            });
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 380, "due " + sw.ElapsedMilliseconds);
        }
#endif
        [TestMethod]
        public void CurrentThread_EnsureTrampoline()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() => {
                Scheduler.CurrentThread.Schedule(() => { ran1 = true; });
                Scheduler.CurrentThread.Schedule(() => { ran2 = true; });
            });
            Assert.IsTrue(ran1);
            Assert.IsTrue(ran2);
        }

        [TestMethod]
        public void CurrentThread_EnsureTrampoline_Nested()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.EnsureTrampoline(() => { ran1 = true; });
                Scheduler.CurrentThread.EnsureTrampoline(() => { ran2 = true; });
            });
            Assert.IsTrue(ran1);
            Assert.IsTrue(ran2);
        }

        [TestMethod]
        public void CurrentThread_EnsureTrampolineAndCancel()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.Schedule(() => {
                    ran1 = true;
                    var d = Scheduler.CurrentThread.Schedule(() => { ran2 = true; });
                    d.Dispose();
                });
            });
            Assert.IsTrue(ran1);
            Assert.IsFalse(ran2);
        }

        [TestMethod]
        public void CurrentThread_EnsureTrampolineAndCancelTimed()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.Schedule(() =>
                {
                    ran1 = true;
                    var d = Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(1), () => { ran2 = true; });
                    d.Dispose();
                });
            });
            Assert.IsTrue(ran1);
            Assert.IsFalse(ran2);
        }
    }
}
