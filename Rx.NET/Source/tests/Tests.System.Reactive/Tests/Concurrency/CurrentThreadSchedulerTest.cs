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

    public class CurrentThreadSchedulerTest
    {
        [Fact]
        public void CurrentThread_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(TimeSpan), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(DateTimeOffset), default));
        }

        [Fact]
        public void CurrentThread_Now()
        {
            var res = Scheduler.CurrentThread.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

#if !NO_THREAD
        [Fact]
        public void CurrentThread_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() => { Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.True(ran);
        }
#endif

        [Fact]
        public void CurrentThread_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                Scheduler.CurrentThread.Schedule(() => { throw ex; });
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(e, ex);
            }
        }
#if !NO_THREAD
        [Fact]
        public void CurrentThread_ScheduleActionNested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() =>
            {
                Assert.Equal(id, Thread.CurrentThread.ManagedThreadId);
                Scheduler.CurrentThread.Schedule(() => { ran = true; });
            });
            Assert.True(ran);
        }

        [Fact]
        public void CurrentThread_ScheduleActionNested_TimeSpan()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() =>
            {
                Assert.Equal(id, Thread.CurrentThread.ManagedThreadId);
                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(1), () => { ran = true; });
            });
            Assert.True(ran);
        }

        [Fact(Skip = "")]
        public void CurrentThread_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.True(ran, "ran");
            Assert.True(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }

        [Fact(Skip = "")]
        public void CurrentThread_ScheduleActionDueNested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () =>
            {
                sw.Stop();
                Assert.Equal(id, Thread.CurrentThread.ManagedThreadId);
                sw.Start();
                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () =>
                {
                    sw.Stop();
                    ran = true;
                });
            });
            Assert.True(ran, "ran");
            Assert.True(sw.ElapsedMilliseconds > 380, "due " + sw.ElapsedMilliseconds);
        }
#endif
        [Fact]
        public void CurrentThread_EnsureTrampoline()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.Schedule(() => { ran1 = true; });
                Scheduler.CurrentThread.Schedule(() => { ran2 = true; });
            });
            Assert.True(ran1);
            Assert.True(ran2);
        }

        [Fact]
        public void CurrentThread_EnsureTrampoline_Nested()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.EnsureTrampoline(() => { ran1 = true; });
                Scheduler.CurrentThread.EnsureTrampoline(() => { ran2 = true; });
            });
            Assert.True(ran1);
            Assert.True(ran2);
        }

        [Fact]
        public void CurrentThread_EnsureTrampolineAndCancel()
        {
            var ran1 = false;
            var ran2 = false;
            Scheduler.CurrentThread.EnsureTrampoline(() =>
            {
                Scheduler.CurrentThread.Schedule(() =>
                {
                    ran1 = true;
                    var d = Scheduler.CurrentThread.Schedule(() => { ran2 = true; });
                    d.Dispose();
                });
            });
            Assert.True(ran1);
            Assert.False(ran2);
        }

        [Fact]
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
            Assert.True(ran1);
            Assert.False(ran2);
        }
    }
}
