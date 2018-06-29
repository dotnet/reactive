// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class ImmediateSchedulerTest
    {
        [Fact]
        public void Immediate_Now()
        {
            var res = Scheduler.Immediate.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }
#if !NO_THREAD
        [Fact]
        public void Immediate_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            Scheduler.Immediate.Schedule(() => { Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.True(ran);
        }
#endif

        [Fact]
        public void Immediate_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                Scheduler.Immediate.Schedule(() => { throw ex; });
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(e, ex);
            }
        }

        [Fact]
        public void Immediate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(42, TimeSpan.Zero, default));
        }

        [Fact]
        public void Immediate_Simple1()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule(42, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.Equal(42, _x);
        }

        [Fact]
        public void Immediate_Simple2()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule(42, DateTimeOffset.Now, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.Equal(42, _x);
        }

        [Fact]
        public void Immediate_Simple3()
        {
            var _x = 0;
            Scheduler.Immediate.Schedule(42, TimeSpan.Zero, (self, x) => { _x = x; return Disposable.Empty; });
            Assert.Equal(42, _x);
        }

        [Fact]
        public void Immediate_Recursive1()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule(42, (self, x) => { _x = x; return self.Schedule(43, (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.Equal(42, _x);
            Assert.Equal(43, _y);
        }

        [Fact]
        public void Immediate_Recursive2()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule(42, (self, x) => { _x = x; return self.Schedule(43, DateTimeOffset.Now, (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.Equal(42, _x);
            Assert.Equal(43, _y);
        }

        [Fact]
        public void Immediate_Recursive3()
        {
            var _x = 0;
            var _y = 0;
            Scheduler.Immediate.Schedule(42, (self, x) => { _x = x; return self.Schedule(43, TimeSpan.FromMilliseconds(100), (self2, y) => { _y = y; return Disposable.Empty; }); });
            Assert.Equal(42, _x);
            Assert.Equal(43, _y);
        }

        [Fact]
        public void Immediate_ArgumentChecking_More()
        {
            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, default);
                });

                return Disposable.Empty;
            });

            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, TimeSpan.FromSeconds(1), default);
                });

                return Disposable.Empty;
            });

            Scheduler.Immediate.Schedule(42, (self, state) =>
            {
                ReactiveAssert.Throws<ArgumentNullException>(() =>
                {
                    self.Schedule(43, DateTimeOffset.UtcNow.AddDays(1), default);
                });

                return Disposable.Empty;
            });
        }

#if !NO_THREAD
        [Fact(Skip = "Ignored")]
        public void Immediate_ScheduleActionDue()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var sw = new Stopwatch();
            sw.Start();
            Scheduler.Immediate.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; });
            Assert.True(ran, "ran");
            Assert.True(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
        }
#endif
    }
}
