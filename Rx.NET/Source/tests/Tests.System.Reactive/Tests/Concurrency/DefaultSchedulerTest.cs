// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class DefaultSchedulerTest
    {
        [Fact]
        public void Schedule_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(42, TimeSpan.Zero, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
        }

        [Fact]
        public void Get_Now()
        {
            var res = DefaultScheduler.Instance.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }
#if !NO_THREAD
        [Fact]
        public void ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(() => { Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [Fact]
        public void ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var evt = new ManualResetEvent(false);
            nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId); evt.Set(); });
            evt.WaitOne();
        }

        [Fact]
        public void ScheduleActionCancel()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var nt = DefaultScheduler.Instance;
            var set = false;
            var d = nt.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.True(false); set = true; });
            d.Dispose();
            Thread.Sleep(400);
            Assert.False(set);
        }

        [Fact]
        public void Periodic_NonReentrant()
        {
            var n = 0;
            var fail = false;

            var d = DefaultScheduler.Instance.SchedulePeriodic(0, TimeSpan.FromMilliseconds(50), x =>
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
#if DESKTOPCLR
        [Trait("SkipCI", "true")]
        [Fact]
        public void No_ThreadPool_Starvation_Dispose()
        {
            ThreadPool.GetAvailableThreads(out var bwt, out var bio);

            var N = Environment.ProcessorCount * 2;

            for (var i = 0; i < N; i++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);
                var d = Scheduler.Default.Schedule(TimeSpan.FromMilliseconds(1), () => { e.Set(); f.WaitOne(); });
                e.WaitOne();
                d.Dispose();
                f.Set();
            }

            ThreadPool.GetAvailableThreads(out var ewt, out var eio);

            Assert.False(bwt - ewt >= N);
        }
#endif
    }
}
