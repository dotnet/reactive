// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_REMOTING
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    static class Exts
    {
        public static T Deq<T>(this List<T> l)
        {
            var t = l[0];
            l.RemoveAt(0);
            return t;
        }
    }

    [TestClass]
    public class SystemClockTest
    {
        private void Run(CrossAppDomainDelegate a)
        {
            var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
            domain.DoCallBack(a);
            AppDomain.Unload(domain);
        }

        [TestMethod]
        public void PastWork()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var due = now - TimeSpan.FromMinutes(1);

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == now);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void ImmediateWork()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var due = now;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void ShortTermWork()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromSeconds(1) /* rel <= SHORTTERM */;
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void ShortTermWork_Dispose()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromSeconds(1) /* rel <= SHORTTERM */;
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                var d = s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                d.Dispose();

                s.SetTime(due);
                next.Invoke();

                Assert.IsFalse(done);
            });
        }

        [TestMethod]
        public void ShortTermWork_InaccurateClock()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromSeconds(1);
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, s._queue.Count);

                var nxt1 = s._queue.Deq();
                Assert.IsTrue(s.Now + nxt1.DueTime == due);

                s.SetTime(due - TimeSpan.FromMilliseconds(500) /* > RETRYSHORT */);
                nxt1.Invoke();

                Assert.AreEqual(1, s._queue.Count);

                var nxt2 = s._queue.Deq();
                Assert.IsTrue(s.Now + nxt2.DueTime == due);

                s.SetTime(due);
                nxt2.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void LongTermWork1()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromMinutes(1) /* rel > SHORTTERM */;
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, cal._queue.Count);

                var work = cal._queue.Deq();
                Assert.IsTrue(work.Interval < rel);

                s.SetTime(s.Now + work.Interval);
                work.Value._action(work.Value._state);

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void LongTermWork2()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromDays(1) /* rel > SHORTTERM and rel * MAXERRORRATIO > SHORTTERM */;
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, cal._queue.Count);

                var wrk1 = cal._queue.Deq();
                Assert.IsTrue(wrk1.Interval < rel);

                s.SetTime(s.Now + wrk1.Interval);
                wrk1.Value._action(wrk1.Value._state);

                // Begin of second long term scheduling
                Assert.AreEqual(1, cal._queue.Count);

                var wrk2 = cal._queue.Deq();
                Assert.IsTrue(wrk2.Interval < rel);

                s.SetTime(s.Now + wrk2.Interval);
                wrk2.Value._action(wrk2.Value._state);
                // End of second long term scheduling

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void LongTerm_Multiple()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);

                var s = new MyScheduler();
                s.SetTime(now);

                var due1 = now + TimeSpan.FromMinutes(10);
                var due2 = now + TimeSpan.FromMinutes(30);
                var due3 = now + TimeSpan.FromMinutes(60);

                var done1 = false;
                var done2 = false;
                var done3 = false;

                s.Schedule(due2, () => { done2 = true; });
                s.Schedule(due1, () => { done1 = true; });
                s.Schedule(due3, () => { done3 = true; });

                // First CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk1 = cal._queue.Deq();
                var fst = s.Now + wrk1.Interval;
                Assert.IsTrue(fst < due1);

                // First TRN
                s.SetTime(fst);
                wrk1.Value._action(wrk1.Value._state);

                // First SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh1 = s._queue.Deq();

                // Second CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk2 = cal._queue.Deq();
                var snd = s.Now + wrk2.Interval;
                Assert.IsTrue(snd < due2);

                // First RUN
                s.SetTime(due1);
                sh1.Invoke();
                Assert.IsTrue(done1);

                // Second TRN
                s.SetTime(snd);
                wrk2.Value._action(wrk2.Value._state);

                // Second SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh2 = s._queue.Deq();

                // Third CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk3 = cal._queue.Deq();
                var trd = s.Now + wrk3.Interval;
                Assert.IsTrue(trd < due3);

                // Second RUN
                s.SetTime(due2);
                sh2.Invoke();
                Assert.IsTrue(done2);

                // Third TRN
                s.SetTime(trd);
                wrk3.Value._action(wrk3.Value._state);

                // Third SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh3 = s._queue.Deq();

                // Third RUN
                s.SetTime(due3);
                sh3.Invoke();
                Assert.IsTrue(done3);
            });
        }

        [TestMethod]
        public void LongTerm_Multiple_Dispose()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);

                var s = new MyScheduler();
                s.SetTime(now);

                var due1 = now + TimeSpan.FromMinutes(10);
                var due2 = now + TimeSpan.FromMinutes(30);
                var due3 = now + TimeSpan.FromMinutes(60);

                var done1 = false;
                var done2 = false;
                var done3 = false;

                var d2 = s.Schedule(due2, () => { done2 = true; });
                var d1 = s.Schedule(due1, () => { done1 = true; });
                var d3 = s.Schedule(due3, () => { done3 = true; });

                // First CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk1 = cal._queue.Deq();
                var fst = s.Now + wrk1.Interval;
                Assert.IsTrue(fst < due1);

                // First TRN
                s.SetTime(fst);
                wrk1.Value._action(wrk1.Value._state);

                // First DIS
                d1.Dispose();

                // First SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh1 = s._queue.Deq();

                // Second CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk2 = cal._queue.Deq();
                var snd = s.Now + wrk2.Interval;
                Assert.IsTrue(snd < due2);

                // First RUN
                s.SetTime(due1);
                sh1.Invoke();
                Assert.IsFalse(done1);

                // Second DIS
                // Third DIS
                d2.Dispose();
                d3.Dispose();

                // Second TRN
                s.SetTime(snd);
                wrk2.Value._action(wrk2.Value._state);

                // Second SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh2 = s._queue.Deq();

                // Third CHK
                Assert.AreEqual(1, cal._queue.Count);
                var wrk3 = cal._queue.Deq();
                var trd = s.Now + wrk3.Interval;
                Assert.IsTrue(trd < due3);

                // Second RUN
                s.SetTime(due2);
                sh2.Invoke();
                Assert.IsFalse(done2);

                // Third TRN
                s.SetTime(trd);
                wrk3.Value._action(wrk3.Value._state);

                // Third SHT
                Assert.AreEqual(1, s._queue.Count);
                var sh3 = s._queue.Deq();

                // Third RUN
                s.SetTime(due3);
                sh3.Invoke();
                Assert.IsFalse(done3);
            });
        }

        [TestMethod]
        public void ClockChanged_FalsePositive()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromMinutes(1);
                var due = now + rel;

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, cal._queue.Count);

                s.SetTime(now);
                scm.OnSystemClockChanged();

                var work = cal._queue.Deq();
                Assert.IsTrue(work.Interval < rel);

                s.SetTime(s.Now + work.Interval);
                work.Value._action(work.Value._state);

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(s.Now + next.DueTime == due);

                s.SetTime(due);
                next.Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void ClockChanged_Forward1()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromMinutes(1);
                var due = now + rel;
                var err = TimeSpan.FromMinutes(1);

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, cal._queue.Count);
                Assert.AreEqual(0, s._queue.Count);

                s.SetTime(due + err);
                scm.OnSystemClockChanged();

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(next.DueTime == TimeSpan.Zero);
                next.Invoke();
                Assert.IsTrue(done);

                var tmr = cal._queue.Deq();
                tmr.Value._action(tmr.Value._state);

                Assert.AreEqual(0, cal._queue.Count);
                Assert.AreEqual(0, s._queue.Count);
            });
        }

        [TestMethod]
        public void ClockChanged_Forward2()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromSeconds(1);
                var due = now + rel;
                var err = TimeSpan.FromMinutes(1);

                var s = new MyScheduler();
                s.SetTime(now);

                var n = 0;
                s.Schedule(due, () => { n++; });

                Assert.AreEqual(1, s._queue.Count);

                var wrk = s._queue.Deq();
                Assert.IsTrue(wrk.DueTime == rel);

                s.SetTime(due + err);
                scm.OnSystemClockChanged();

                Assert.AreEqual(1, s._queue.Count);

                var next = s._queue.Deq();
                Assert.IsTrue(next.DueTime == TimeSpan.Zero);
                next.Invoke();
                Assert.AreEqual(1, n);

                wrk.Invoke(); // Bad schedulers may not grant cancellation immediately.
                Assert.AreEqual(1, n); // Invoke shouldn't cause double execution of the work.
            });
        }

        [TestMethod]
        public void ClockChanged_Backward1()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromMinutes(1);
                var due = now + rel;
                var err = TimeSpan.FromMinutes(-2);

                var s = new MyScheduler();
                s.SetTime(now);

                var done = false;
                s.Schedule(due, () => { done = true; });

                Assert.AreEqual(1, cal._queue.Count);
                Assert.IsTrue(cal._queue[0].Interval < rel);

                Assert.AreEqual(0, s._queue.Count);

                s.SetTime(due + err);
                scm.OnSystemClockChanged();

                Assert.AreEqual(1, cal._queue.Count);

                var tmr = cal._queue.Deq();
                Assert.IsTrue(tmr.Interval > rel);
                Assert.IsTrue(tmr.Interval < -err);

                s.SetTime(s.Now + tmr.Interval);
                tmr.Value._action(tmr.Value._state);

                Assert.IsFalse(done);

                Assert.AreEqual(0, cal._queue.Count);
                Assert.AreEqual(1, s._queue.Count);

                s.SetTime(due);
                s._queue.Deq().Invoke();

                Assert.IsTrue(done);
            });
        }

        [TestMethod]
        public void ClockChanged_Backward2()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);
                var rel = TimeSpan.FromSeconds(1);
                var due = now + rel;
                var err = TimeSpan.FromMinutes(-1);

                var s = new MyScheduler();
                s.SetTime(now);

                var n = 0;
                s.Schedule(due, () => { n++; });

                Assert.AreEqual(0, cal._queue.Count);
                Assert.AreEqual(1, s._queue.Count);
                var wrk = s._queue[0];
                Assert.IsTrue(wrk.DueTime == rel);

                s.SetTime(due + err);
                scm.OnSystemClockChanged();

                Assert.AreEqual(1, cal._queue.Count);

                var tmr = cal._queue.Deq();
                Assert.IsTrue(tmr.Interval > rel);
                Assert.IsTrue(tmr.Interval < -err);

                s.SetTime(s.Now + tmr.Interval);
                tmr.Value._action(tmr.Value._state);

                Assert.AreEqual(0, n);

                Assert.AreEqual(0, cal._queue.Count);
                Assert.AreEqual(1, s._queue.Count);

                s.SetTime(due);
                s._queue.Deq().Invoke();

                Assert.AreEqual(1, n);

                wrk.Invoke(); // Bad schedulers may not grant cancellation immediately.
                Assert.AreEqual(1, n); // Invoke shouldn't cause double execution of the work.
            });
        }

        [TestMethod]
        public void PeriodicSystemClockChangeMonitor()
        {
            Run(() =>
            {
                var provider = new FakeClockPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var clock = (FakeClock)provider.GetService<ISystemClock>();
                clock._now = new DateTimeOffset(2012, 4, 26, 12, 0, 0, TimeSpan.Zero);

                var cal = (FakeClockCAL)provider.GetService<IConcurrencyAbstractionLayer>();

                var period = TimeSpan.FromSeconds(1);
                var ptscm = new PeriodicTimerSystemClockMonitor(period);

                var delta = TimeSpan.Zero;
                var n = 0;
                var h = new EventHandler<SystemClockChangedEventArgs>((o, e) =>
                {
                    delta = e.NewTime - e.OldTime;
                    n++;
                });

                ptscm.SystemClockChanged += h;

                Assert.IsNotNull(cal._action);
                Assert.IsTrue(cal._period == period);
                Assert.AreEqual(0, n);

                clock._now += period;
                cal._action();
                Assert.AreEqual(0, n);

                clock._now += period;
                cal._action();
                Assert.AreEqual(0, n);

                var diff1 = TimeSpan.FromSeconds(3);
                clock._now += period + diff1;
                cal._action();
                Assert.AreEqual(1, n);
                Assert.IsTrue(delta == diff1);

                clock._now += period;
                cal._action();
                Assert.AreEqual(1, n);

                clock._now += period;
                cal._action();
                Assert.AreEqual(1, n);

                var diff2 = TimeSpan.FromSeconds(-5);
                clock._now += period + diff2;
                cal._action();
                Assert.AreEqual(2, n);
                Assert.IsTrue(delta == diff2);

                clock._now += period;
                cal._action();
                Assert.AreEqual(2, n);

                ptscm.SystemClockChanged -= h;

                Assert.IsNull(cal._action);
            });
        }

        [TestMethod]
        public void ClockChanged_RefCounting()
        {
            Run(() =>
            {
                var provider = new MyPlatformEnlightenmentProvider();
                PlatformEnlightenmentProvider.Current = provider;

                var scm = (ClockChanged)provider.GetService<INotifySystemClockChanged>();

                var cal = provider._cal;
                var now = new DateTimeOffset(2012, 4, 25, 12, 0, 0, TimeSpan.Zero);

                var s = new MyScheduler();
                s.SetTime(now);

                var due1 = now + TimeSpan.FromSeconds(5);
                var due2 = now + TimeSpan.FromSeconds(8);
                var due3 = now + TimeSpan.FromMinutes(1);
                var due4 = now + TimeSpan.FromMinutes(2);
                var due5 = now + TimeSpan.FromMinutes(3);
                var due6 = now + TimeSpan.FromMinutes(3) + TimeSpan.FromSeconds(2);

                var done1 = false;
                var done2 = false;
                var done3 = false;
                var done4 = false;
                var done5 = false;
                var done6 = false;

                var d1 = s.Schedule(due1, () => { done1 = true; });
                var d5 = s.Schedule(due5, () => { done5 = true; });
                var d3 = s.Schedule(due3, () => { done3 = true; throw new Exception(); });
                var d2 = s.Schedule(due2, () => { done2 = true; });
                var d4 = s.Schedule(due4, () => { done4 = true; });

                d2.Dispose();
                d4.Dispose();

                Assert.AreEqual(1, scm.n);

                s.SetTime(due1);
                var i1 = s._queue.Deq();
                i1.Invoke();
                Assert.IsTrue(done1);

                Assert.AreEqual(1, scm.n);

                s.SetTime(due2);
                var i2 = s._queue.Deq();
                i2.Invoke();
                Assert.IsFalse(done2);

                Assert.AreEqual(1, scm.n);

                var l1 = cal._queue.Deq();
                var l1d = now + l1.Interval;
                s.SetTime(l1d);
                l1.Value._action(l1.Value._state);

                s.SetTime(due3);
                var i3 = s._queue.Deq();
                try
                {
                    i3.Invoke();
                    Assert.Fail();
                }
                catch { }
                Assert.IsTrue(done3);

                Assert.AreEqual(1, scm.n);

                var l2 = cal._queue.Deq();
                var l2d = l1d + l2.Interval;
                s.SetTime(l2d);
                l2.Value._action(l2.Value._state);

                s.SetTime(due4);
                var i4 = s._queue.Deq();
                i4.Invoke();
                Assert.IsFalse(done4);

                Assert.AreEqual(1, scm.n);

                var l3 = cal._queue.Deq();
                var l3d = l2d + l3.Interval;
                s.SetTime(l3d);
                l3.Value._action(l3.Value._state);

                s.SetTime(due5);
                var i5 = s._queue.Deq();
                i5.Invoke();
                Assert.IsTrue(done5);

                Assert.AreEqual(0, scm.n);

                var d6 = s.Schedule(due6, () => { done6 = true; });

                Assert.AreEqual(1, scm.n);

                s.SetTime(due6);
                var i6 = s._queue.Deq();
                i6.Invoke();
                Assert.IsTrue(done6);

                Assert.AreEqual(0, scm.n);
            });
        }

        class MyScheduler : LocalScheduler
        {
            internal List<ScheduledItem<TimeSpan>> _queue = new List<ScheduledItem<TimeSpan>>();

            private DateTimeOffset _now;

            public void SetTime(DateTimeOffset now)
            {
                _now = now;
            }

            public override DateTimeOffset Now
            {
                get { return _now; }
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                var s = new ScheduledItem<TimeSpan, TState>(this, state, action, dueTime);
                _queue.Add(s);
                return Disposable.Create(() => _queue.Remove(s));
            }
        }

        class MyPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
        {
            internal MyCAL _cal;

            public MyPlatformEnlightenmentProvider()
            {
                _cal = new MyCAL();
            }

            public T GetService<T>(params object[] args) where T : class
            {
                if (typeof(T) == typeof(IConcurrencyAbstractionLayer))
                {
                    return (T)(object)_cal;
                }
                else if (typeof(T) == typeof(INotifySystemClockChanged))
                {
                    return (T)(object)ClockChanged.Instance;
                }

                return null;
            }
        }

        class FakeClockPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
        {
            internal FakeClockCAL _cal;
            internal FakeClock _clock;

            public FakeClockPlatformEnlightenmentProvider()
            {
                _cal = new FakeClockCAL();
                _clock = new FakeClock();
            }

            public T GetService<T>(params object[] args) where T : class
            {
                if (typeof(T) == typeof(IConcurrencyAbstractionLayer))
                {
                    return (T)(object)_cal;
                }
                else if (typeof(T) == typeof(ISystemClock))
                {
                    return (T)(object)_clock;
                }

                return null;
            }
        }

        class Work
        {
            internal readonly Action<object> _action;
            internal readonly object _state;

            public Work(Action<object> action, object state)
            {
                _action = action;
                _state = state;
            }
        }

        class MyCAL : IConcurrencyAbstractionLayer
        {
            internal List<TimeInterval<Work>> _queue = new List<TimeInterval<Work>>();

            public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
            {
                var t = new TimeInterval<Work>(new Work(action, state), dueTime);
                _queue.Add(t);
                return Disposable.Create(() => _queue.Remove(t));
            }

            public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
            {
                throw new NotImplementedException();
            }

            public IDisposable QueueUserWorkItem(Action<object> action, object state)
            {
                throw new NotImplementedException();
            }

            public void Sleep(TimeSpan timeout)
            {
                throw new NotImplementedException();
            }

            public IStopwatch StartStopwatch()
            {
                throw new NotImplementedException();
            }

            public bool SupportsLongRunning
            {
                get { throw new NotImplementedException(); }
            }

            public void StartThread(Action<object> action, object state)
            {
                throw new NotImplementedException();
            }
        }

        class FakeClockCAL : IConcurrencyAbstractionLayer
        {
            internal Action _action;
            internal TimeSpan _period;

            public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
            {
                throw new NotImplementedException();
            }

            public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
            {
                _action = action;
                _period = period;
                return Disposable.Create(() => _action = null);
            }

            public IDisposable QueueUserWorkItem(Action<object> action, object state)
            {
                throw new NotImplementedException();
            }

            public void Sleep(TimeSpan timeout)
            {
                throw new NotImplementedException();
            }

            public IStopwatch StartStopwatch()
            {
                throw new NotImplementedException();
            }

            public bool SupportsLongRunning
            {
                get { throw new NotImplementedException(); }
            }

            public void StartThread(Action<object> action, object state)
            {
                throw new NotImplementedException();
            }
        }

        class FakeClock : ISystemClock
        {
            internal DateTimeOffset _now;

            public DateTimeOffset UtcNow
            {
                get { return _now; }
            }
        }

        class ClockChanged : INotifySystemClockChanged
        {
            private static ClockChanged s_instance = new ClockChanged();

            private EventHandler<SystemClockChangedEventArgs> _systemClockChanged;

            internal int n = 0;

            public event EventHandler<SystemClockChangedEventArgs> SystemClockChanged
            {
                add
                {
                    _systemClockChanged += value;
                    n++;
                }

                remove
                {
                    _systemClockChanged -= value;
                    n--;
                }
            }

            public static ClockChanged Instance
            {
                get
                {
                    return s_instance;
                }
            }

            public void OnSystemClockChanged()
            {
                var scc = _systemClockChanged;
                if (scc != null)
                    scc(this, new SystemClockChangedEventArgs());
            }
        }
    }
}
#endif