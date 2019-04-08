// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_REMOTING && !XUNIT
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using Xunit;

namespace ReactiveTests.Tests
{
    internal static class Exts
    {
        public static T Deq<T>(this List<T> l)
        {
            var t = l[0];
            l.RemoveAt(0);
            return t;
        }
    }


    public class SystemClockTest
    {
        private void Run(CrossAppDomainDelegate a)
        {
            var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
            domain.DoCallBack(a);
            AppDomain.Unload(domain);
        }

        [Fact]
        public void PastWork()
        {
            Run(PastWork_Callback);
        }

        private static void PastWork_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == now);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void ImmediateWork()
        {
            Run(ImmediateWork_Callback);
        }

        private static void ImmediateWork_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void ShortTermWork()
        {
            Run(ShortTermWork_Callback);
        }

        private static void ShortTermWork_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void ShortTermWork_Dispose()
        {
            Run(ShortTermWork_Dispose_Callback);
        }

        private static void ShortTermWork_Dispose_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            d.Dispose();

            s.SetTime(due);
            next.Invoke();

            Assert.False(done);
        }

        [Fact]
        public void ShortTermWork_InaccurateClock()
        {
            Run(ShortTermWork_InaccurateClock_Callback);
        }

        private static void ShortTermWork_InaccurateClock_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var nxt1 = s._queue.Deq();
            Assert.True(s.Now + nxt1.DueTime == due);

            s.SetTime(due - TimeSpan.FromMilliseconds(500) /* > RETRYSHORT */);
            nxt1.Invoke();

            Assert.Equal(1, s._queue.Count);

            var nxt2 = s._queue.Deq();
            Assert.True(s.Now + nxt2.DueTime == due);

            s.SetTime(due);
            nxt2.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void LongTermWork1()
        {
            Run(LongTermWork1_Callback);
        }

        private static void LongTermWork1_Callback()
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

            Assert.Equal(1, cal._queue.Count);

            var work = cal._queue.Deq();
            Assert.True(work.Interval < rel);

            s.SetTime(s.Now + work.Interval);
            work.Value._action(work.Value._state);

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void LongTermWork2()
        {
            Run(LongTermWork2_Callback);
        }

        private static void LongTermWork2_Callback()
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

            Assert.Equal(1, cal._queue.Count);

            var wrk1 = cal._queue.Deq();
            Assert.True(wrk1.Interval < rel);

            s.SetTime(s.Now + wrk1.Interval);
            wrk1.Value._action(wrk1.Value._state);

            // Begin of second long term scheduling
            Assert.Equal(1, cal._queue.Count);

            var wrk2 = cal._queue.Deq();
            Assert.True(wrk2.Interval < rel);

            s.SetTime(s.Now + wrk2.Interval);
            wrk2.Value._action(wrk2.Value._state);
            // End of second long term scheduling

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void LongTerm_Multiple()
        {
            Run(LongTerm_Multiple_Callback);
        }

        private static void LongTerm_Multiple_Callback()
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
            Assert.Equal(1, cal._queue.Count);
            var wrk1 = cal._queue.Deq();
            var fst = s.Now + wrk1.Interval;
            Assert.True(fst < due1);

            // First TRN
            s.SetTime(fst);
            wrk1.Value._action(wrk1.Value._state);

            // First SHT
            Assert.Equal(1, s._queue.Count);
            var sh1 = s._queue.Deq();

            // Second CHK
            Assert.Equal(1, cal._queue.Count);
            var wrk2 = cal._queue.Deq();
            var snd = s.Now + wrk2.Interval;
            Assert.True(snd < due2);

            // First RUN
            s.SetTime(due1);
            sh1.Invoke();
            Assert.True(done1);

            // Second TRN
            s.SetTime(snd);
            wrk2.Value._action(wrk2.Value._state);

            // Second SHT
            Assert.Equal(1, s._queue.Count);
            var sh2 = s._queue.Deq();

            // Third CHK
            Assert.Equal(1, cal._queue.Count);
            var wrk3 = cal._queue.Deq();
            var trd = s.Now + wrk3.Interval;
            Assert.True(trd < due3);

            // Second RUN
            s.SetTime(due2);
            sh2.Invoke();
            Assert.True(done2);

            // Third TRN
            s.SetTime(trd);
            wrk3.Value._action(wrk3.Value._state);

            // Third SHT
            Assert.Equal(1, s._queue.Count);
            var sh3 = s._queue.Deq();

            // Third RUN
            s.SetTime(due3);
            sh3.Invoke();
            Assert.True(done3);
        }

        [Fact]
        public void LongTerm_Multiple_Dispose()
        {
            Run(LongTerm_Multiple_Dispose_Callback);
        }

        private static void LongTerm_Multiple_Dispose_Callback()
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
            Assert.Equal(1, cal._queue.Count);
            var wrk1 = cal._queue.Deq();
            var fst = s.Now + wrk1.Interval;
            Assert.True(fst < due1);

            // First TRN
            s.SetTime(fst);
            wrk1.Value._action(wrk1.Value._state);

            // First DIS
            d1.Dispose();

            // First SHT
            Assert.Equal(1, s._queue.Count);
            var sh1 = s._queue.Deq();

            // Second CHK
            Assert.Equal(1, cal._queue.Count);
            var wrk2 = cal._queue.Deq();
            var snd = s.Now + wrk2.Interval;
            Assert.True(snd < due2);

            // First RUN
            s.SetTime(due1);
            sh1.Invoke();
            Assert.False(done1);

            // Second DIS
            // Third DIS
            d2.Dispose();
            d3.Dispose();

            // Second TRN
            s.SetTime(snd);
            wrk2.Value._action(wrk2.Value._state);

            // Second SHT
            Assert.Equal(1, s._queue.Count);
            var sh2 = s._queue.Deq();

            // Third CHK
            Assert.Equal(1, cal._queue.Count);
            var wrk3 = cal._queue.Deq();
            var trd = s.Now + wrk3.Interval;
            Assert.True(trd < due3);

            // Second RUN
            s.SetTime(due2);
            sh2.Invoke();
            Assert.False(done2);

            // Third TRN
            s.SetTime(trd);
            wrk3.Value._action(wrk3.Value._state);

            // Third SHT
            Assert.Equal(1, s._queue.Count);
            var sh3 = s._queue.Deq();

            // Third RUN
            s.SetTime(due3);
            sh3.Invoke();
            Assert.False(done3);
        }

        [Fact]
        public void ClockChanged_FalsePositive()
        {
            Run(ClockChanged_FalsePositive_Callback);
        }

        private static void ClockChanged_FalsePositive_Callback()
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

            Assert.Equal(1, cal._queue.Count);

            s.SetTime(now);
            scm.OnSystemClockChanged();

            var work = cal._queue.Deq();
            Assert.True(work.Interval < rel);

            s.SetTime(s.Now + work.Interval);
            work.Value._action(work.Value._state);

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(s.Now + next.DueTime == due);

            s.SetTime(due);
            next.Invoke();

            Assert.True(done);
        }

        [Fact]
        public void ClockChanged_Forward1()
        {
            Run(ClockChanged_Forward1_Callback);
        }

        private static void ClockChanged_Forward1_Callback()
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

            Assert.Equal(1, cal._queue.Count);
            Assert.Equal(0, s._queue.Count);

            s.SetTime(due + err);
            scm.OnSystemClockChanged();

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(next.DueTime == TimeSpan.Zero);
            next.Invoke();
            Assert.True(done);

            var tmr = cal._queue.Deq();
            tmr.Value._action(tmr.Value._state);

            Assert.Equal(0, cal._queue.Count);
            Assert.Equal(0, s._queue.Count);
        }

        [Fact]
        public void ClockChanged_Forward2()
        {
            Run(ClockChanged_Forward2_Callback);
        }

        private static void ClockChanged_Forward2_Callback()
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

            Assert.Equal(1, s._queue.Count);

            var wrk = s._queue.Deq();
            Assert.True(wrk.DueTime == rel);

            s.SetTime(due + err);
            scm.OnSystemClockChanged();

            Assert.Equal(1, s._queue.Count);

            var next = s._queue.Deq();
            Assert.True(next.DueTime == TimeSpan.Zero);
            next.Invoke();
            Assert.Equal(1, n);

            wrk.Invoke(); // Bad schedulers may not grant cancellation immediately.
            Assert.Equal(1, n); // Invoke shouldn't cause double execution of the work.
        }

        [Fact]
        public void ClockChanged_Backward1()
        {
            Run(ClockChanged_Backward1_Callback);
        }

        private static void ClockChanged_Backward1_Callback()
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

            Assert.Equal(1, cal._queue.Count);
            Assert.True(cal._queue[0].Interval < rel);

            Assert.Equal(0, s._queue.Count);

            s.SetTime(due + err);
            scm.OnSystemClockChanged();

            Assert.Equal(1, cal._queue.Count);

            var tmr = cal._queue.Deq();
            Assert.True(tmr.Interval > rel);
            Assert.True(tmr.Interval < -err);

            s.SetTime(s.Now + tmr.Interval);
            tmr.Value._action(tmr.Value._state);

            Assert.False(done);

            Assert.Equal(0, cal._queue.Count);
            Assert.Equal(1, s._queue.Count);

            s.SetTime(due);
            s._queue.Deq().Invoke();

            Assert.True(done);
        }

        [Fact]
        public void ClockChanged_Backward2()
        {
            Run(ClockChanged_Backward2_Callback);
        }

        private static void ClockChanged_Backward2_Callback()
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

            Assert.Equal(0, cal._queue.Count);
            Assert.Equal(1, s._queue.Count);
            var wrk = s._queue[0];
            Assert.True(wrk.DueTime == rel);

            s.SetTime(due + err);
            scm.OnSystemClockChanged();

            Assert.Equal(1, cal._queue.Count);

            var tmr = cal._queue.Deq();
            Assert.True(tmr.Interval > rel);
            Assert.True(tmr.Interval < -err);

            s.SetTime(s.Now + tmr.Interval);
            tmr.Value._action(tmr.Value._state);

            Assert.Equal(0, n);

            Assert.Equal(0, cal._queue.Count);
            Assert.Equal(1, s._queue.Count);

            s.SetTime(due);
            s._queue.Deq().Invoke();

            Assert.Equal(1, n);

            wrk.Invoke(); // Bad schedulers may not grant cancellation immediately.
            Assert.Equal(1, n); // Invoke shouldn't cause double execution of the work.
        }

        [Fact]
        public void PeriodicSystemClockChangeMonitor()
        {
            Run(PeriodicSystemClockChangeMonitor_Callback);
        }

        private static void PeriodicSystemClockChangeMonitor_Callback()
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

            Assert.NotNull(cal._action);
            Assert.True(cal._period == period);
            Assert.Equal(0, n);

            clock._now += period;
            cal._action();
            Assert.Equal(0, n);

            clock._now += period;
            cal._action();
            Assert.Equal(0, n);

            var diff1 = TimeSpan.FromSeconds(3);
            clock._now += period + diff1;
            cal._action();
            Assert.Equal(1, n);
            Assert.True(delta == diff1);

            clock._now += period;
            cal._action();
            Assert.Equal(1, n);

            clock._now += period;
            cal._action();
            Assert.Equal(1, n);

            var diff2 = TimeSpan.FromSeconds(-5);
            clock._now += period + diff2;
            cal._action();
            Assert.Equal(2, n);
            Assert.True(delta == diff2);

            clock._now += period;
            cal._action();
            Assert.Equal(2, n);

            ptscm.SystemClockChanged -= h;

            Assert.Null(cal._action);
        }

        [Fact]
        public void ClockChanged_RefCounting()
        {
            Run(ClockChanged_RefCounting_Callback);
        }

        private static void ClockChanged_RefCounting_Callback()
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

            Assert.Equal(1, scm.N);

            s.SetTime(due1);
            var i1 = s._queue.Deq();
            i1.Invoke();
            Assert.True(done1);

            Assert.Equal(1, scm.N);

            s.SetTime(due2);
            var i2 = s._queue.Deq();
            i2.Invoke();
            Assert.False(done2);

            Assert.Equal(1, scm.N);

            var l1 = cal._queue.Deq();
            var l1d = now + l1.Interval;
            s.SetTime(l1d);
            l1.Value._action(l1.Value._state);

            s.SetTime(due3);
            var i3 = s._queue.Deq();
            try
            {
                i3.Invoke();
                Assert.True(false);
            }
            catch { }
            Assert.True(done3);

            Assert.Equal(1, scm.N);

            var l2 = cal._queue.Deq();
            var l2d = l1d + l2.Interval;
            s.SetTime(l2d);
            l2.Value._action(l2.Value._state);

            s.SetTime(due4);
            var i4 = s._queue.Deq();
            i4.Invoke();
            Assert.False(done4);

            Assert.Equal(1, scm.N);

            var l3 = cal._queue.Deq();
            var l3d = l2d + l3.Interval;
            s.SetTime(l3d);
            l3.Value._action(l3.Value._state);

            s.SetTime(due5);
            var i5 = s._queue.Deq();
            i5.Invoke();
            Assert.True(done5);

            Assert.Equal(0, scm.N);

            var d6 = s.Schedule(due6, () => { done6 = true; });

            Assert.Equal(1, scm.N);

            s.SetTime(due6);
            var i6 = s._queue.Deq();
            i6.Invoke();
            Assert.True(done6);

            Assert.Equal(0, scm.N);
        }

        [Fact]
        public void SystemClockChange_SignalNoInvalidOperationExceptionDueToRemove()
        {
            var local = new RemoveScheduler();
            SystemClock.SystemClockChanged.Add(new WeakReference<LocalScheduler>(local));

            SystemClock.OnSystemClockChanged(this, new SystemClockChangedEventArgs());
        }

        private class RemoveScheduler : LocalScheduler
        {
            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            internal override void SystemClockChanged(object sender, SystemClockChangedEventArgs args)
            {
                var target = default(WeakReference<LocalScheduler>);
                foreach (var entries in SystemClock.SystemClockChanged)
                {
                    if (entries.TryGetTarget(out var local) && local == this)
                    {
                        target = entries;
                        break;
                    }
                }
                SystemClock.SystemClockChanged.Remove(target);
            }
        }

        private class MyScheduler : LocalScheduler
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

        private class MyPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
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

        private class FakeClockPlatformEnlightenmentProvider : IPlatformEnlightenmentProvider
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

        private class Work
        {
            internal readonly Action<object> _action;
            internal readonly object _state;

            public Work(Action<object> action, object state)
            {
                _action = action;
                _state = state;
            }
        }

        private class MyCAL : IConcurrencyAbstractionLayer
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
                return Disposable.Empty;
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

        private class FakeClockCAL : IConcurrencyAbstractionLayer
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

        private class FakeClock : ISystemClock
        {
            internal DateTimeOffset _now;

            public DateTimeOffset UtcNow
            {
                get { return _now; }
            }
        }

        private class ClockChanged : INotifySystemClockChanged
        {
            private EventHandler<SystemClockChangedEventArgs> _systemClockChanged;

            internal int N = 0;

            public event EventHandler<SystemClockChangedEventArgs> SystemClockChanged
            {
                add
                {
                    _systemClockChanged += value;
                    N++;
                }

                remove
                {
                    _systemClockChanged -= value;
                    N--;
                }
            }

            public static ClockChanged Instance { get; } = new ClockChanged();

            public void OnSystemClockChanged()
            {
                _systemClockChanged?.Invoke(this, new SystemClockChangedEventArgs());
            }
        }
    }
}
#endif
