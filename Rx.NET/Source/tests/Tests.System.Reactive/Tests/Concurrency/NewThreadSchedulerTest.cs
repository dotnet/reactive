// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
#if !NO_THREAD
    public class NewThreadSchedulerTest
    {
        [Fact]
        public void NewThread_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new NewThreadScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, TimeSpan.Zero, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.ScheduleLongRunning(42, default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => NewThreadScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [Fact]
        public void NewThread_Now()
        {
            var res = NewThreadScheduler.Default.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }
#if !NO_THREAD
        [Fact]
        public void NewThread_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [Fact(Skip = "")]
        public void NewThread_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            var sw = new Stopwatch();
            sw.Start();
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
            Assert.True(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif

#if !NO_PERF
        [Fact]
        public void Stopwatch()
        {
            StopwatchTest.Run(NewThreadScheduler.Default);
        }
#endif

        [Fact]
        public void NewThread_Periodic()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = NewThreadScheduler.Default.SchedulePeriodic(TimeSpan.FromMilliseconds(25), () =>
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

#if !NO_THREAD
        [Fact]
        public void NewThread_Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = NewThreadScheduler.Default.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
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
    }
#endif
}
