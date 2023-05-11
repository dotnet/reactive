// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class NewThreadSchedulerTest
    {
        [TestMethod]
        public void NewThread_ArgumentChecking()
        {
#pragma warning disable CA1806 // (Unused new instance.) We expect the constructor to throw.
            ReactiveAssert.Throws<ArgumentNullException>(() => new NewThreadScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(42, TimeSpan.Zero, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.ScheduleLongRunning(42, default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => NewThreadScheduler.Default.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
#pragma warning restore CA1806
        }

        [TestMethod]
        public void NewThread_Now()
        {
            var res = NewThreadScheduler.Default.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [TestMethod]
        public void NewThread_ScheduleAction()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [TestMethod]
        public void NewThread_ScheduleActionDue()
        {
            var id = Environment.CurrentManagedThreadId;
            var nt = NewThreadScheduler.Default;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.NotEqual(id, Environment.CurrentManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

#if !NO_PERF
        [TestMethod]
        public void Stopwatch()
        {
            StopwatchTest.Run(NewThreadScheduler.Default);
        }
#endif

        [TestMethod]
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
    }
}
