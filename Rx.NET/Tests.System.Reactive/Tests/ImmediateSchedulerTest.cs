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
    public class ImmediateSchedulerTest
    {
        [TestMethod]
        public void Immediate_Now()
        {
            var res = Scheduler.Immediate.Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void Immediate_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.Immediate.Schedule(() => { Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.IsTrue(ran);
        }

        [TestMethod]
        public void Immediate_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                Scheduler.Immediate.Schedule(() => { throw ex; });
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreSame(e, ex);
            }
        }

        [TestMethod]
        public void Immediate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule<int>(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule<int>(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule<int>(42, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
        }

        [TestMethod]
        public void Immediate_Simple1()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule<int>(42, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.AreEqual(42, _x);
        }

        [TestMethod]
        public void Immediate_Simple2()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule<int>(42, DateTimeOffset.Now, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.AreEqual(42, _x);
        }

        [TestMethod]
        public void Immediate_Simple3()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule<int>(42, TimeSpan.Zero, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.AreEqual(42, _x);
        }

        [TestMethod]
        public void Immediate_Recursive1()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule<int>(42, (self, x) => { _x = x; return self.Schedule<int>(43, (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.AreEqual(42, _x);
            Assert.AreEqual(43, _y);
        }

        [TestMethod]
        public void Immediate_Recursive2()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule<int>(42, (self, x) => { _x = x; return self.Schedule<int>(43, DateTimeOffset.Now, (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.AreEqual(42, _x);
            Assert.AreEqual(43, _y);
        }

        [TestMethod]
        public void Immediate_Recursive3()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule<int>(42, (self, x) => { _x = x; return self.Schedule<int>(43, TimeSpan.FromMilliseconds(100), (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.AreEqual(42, _x);
            Assert.AreEqual(43, _y);
        }

        [TestMethod]
        public void Immediate_ArgumentChecking_More()
        {
            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, default(Func<IScheduler, int, IDisposable>));
                });

                return Disposable.Empty;
            });

            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>));
                });

                return Disposable.Empty;
            });

            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, DateTimeOffset.UtcNow.AddDays(1), default(Func<IScheduler, int, IDisposable>));
                });

                return Disposable.Empty;
            });
        }

#if !SILVERLIGHT
        [TestMethod]
        [Ignore]
        public void Immediate_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.Immediate.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif
    }
}
