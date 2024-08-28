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
    public class CurrentThreadSchedulerTest
    {
        [TestMethod]
        public void CurrentThread_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(TimeSpan), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(42, default(DateTimeOffset), default));
        }

        [TestMethod]
        public void CurrentThread_Now()
        {
            var res = Scheduler.CurrentThread.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [TestMethod]
        public void CurrentThread_ScheduleAction()
        {
            var id = Environment.CurrentManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() => { Assert.Equal(id, Environment.CurrentManagedThreadId); ran = true; });
            Assert.True(ran);
        }

        [TestMethod]
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

        [TestMethod]
        public void CurrentThread_ScheduleActionNested()
        {
            var id = Environment.CurrentManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() =>
            {
                Assert.Equal(id, Environment.CurrentManagedThreadId);
                Scheduler.CurrentThread.Schedule(() => { ran = true; });
            });
            Assert.True(ran);
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionNested_TimeSpan()
        {
            var id = Environment.CurrentManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(() =>
            {
                Assert.Equal(id, Environment.CurrentManagedThreadId);
                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(1), () => { ran = true; });
            });
            Assert.True(ran);
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionDue()
        {
            var id = Environment.CurrentManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () => { Assert.Equal(id, Environment.CurrentManagedThreadId); ran = true; });
            Assert.True(ran, "ran");
        }

        [TestMethod]
        public void CurrentThread_ScheduleActionDueNested()
        {
            var id = Environment.CurrentManagedThreadId;
            var ran = false;
            Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () =>
            {
                Assert.Equal(id, Environment.CurrentManagedThreadId);

                Scheduler.CurrentThread.Schedule(TimeSpan.FromSeconds(0.2), () =>
                {
                    Assert.Equal(id, Environment.CurrentManagedThreadId);
                    ran = true;
                });
            });
            Assert.True(ran, "ran");
        }

        [TestMethod]
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
            Assert.True(ran1);
            Assert.True(ran2);
        }

        [TestMethod]
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
            Assert.True(ran1);
            Assert.False(ran2);
        }
    }
}
