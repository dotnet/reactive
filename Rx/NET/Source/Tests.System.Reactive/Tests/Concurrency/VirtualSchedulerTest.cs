// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class VirtualSchedulerTest
    {
        class VirtualSchedulerTestScheduler : VirtualTimeScheduler<string, char>
        {
            public VirtualSchedulerTestScheduler()
            {
            }

            public VirtualSchedulerTestScheduler(string initialClock, IComparer<string> comparer)
                : base(initialClock, comparer)
            {
            }

            protected override string Add(string absolute, char relative)
            {
                return (absolute ?? string.Empty) + relative;
            }

            protected override DateTimeOffset ToDateTimeOffset(string absolute)
            {
                return new DateTimeOffset((absolute ?? string.Empty).Length, TimeSpan.Zero);
            }

            protected override char ToRelative(TimeSpan timeSpan)
            {
                return (char)(timeSpan.Ticks % char.MaxValue);
            }
        }

        [TestMethod]
        public void Virtual_Now()
        {
            var res = new VirtualSchedulerTestScheduler().Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void Virtual_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var scheduler = new VirtualSchedulerTestScheduler();
            scheduler.Schedule(() => { Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            scheduler.Start();
            Assert.IsTrue(ran);
        }

        [TestMethod]
        public void Virtual_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                var scheduler = new VirtualSchedulerTestScheduler();
                scheduler.Schedule(() => { throw ex; });
                scheduler.Start();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreSame(e, ex);
            }
        }

        [TestMethod]
        public void Virtual_InitialAndComparer_Now()
        {
            var s = new VirtualSchedulerTestScheduler("Bar", Comparer<string>.Default);
            Assert.AreEqual(3, s.Now.Ticks);
        }

        [TestMethod]
        public void Virtual_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler("", null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().ScheduleRelative(0, 'a', null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().ScheduleAbsolute(0, "", null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, TimeSpan.Zero, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new VirtualSchedulerTestScheduler().Schedule(0, DateTimeOffset.UtcNow, default(Func<IScheduler, int, IDisposable>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleAbsolute(default(VirtualSchedulerTestScheduler), "", () => {}));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleAbsolute(new VirtualSchedulerTestScheduler(), "", default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleRelative(default(VirtualSchedulerTestScheduler), 'a', () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => VirtualTimeSchedulerExtensions.ScheduleRelative(new VirtualSchedulerTestScheduler(), 'a', default(Action)));
        }

        [TestMethod]
        public void Historical_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler(DateTime.Now, default(IComparer<DateTimeOffset>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler().ScheduleAbsolute(42, DateTime.Now, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new HistoricalScheduler().ScheduleRelative(42, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>)));
        }

#if !SILVERLIGHT
        [TestMethod]
        [Ignore]
        public void Virtual_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            var scheduler = new VirtualSchedulerTestScheduler();
            scheduler.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            scheduler.Start();
            Assert.IsTrue(ran, "ran");
            Assert.IsTrue(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif
    }
}
