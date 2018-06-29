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

    public class SynchronizationContextSchedulerTest
    {
        [Fact]
        public void SynchronizationContext_ArgumentChecking()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(null, true));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, TimeSpan.Zero, default));
        }

        [Fact]
        public void SynchronizationContext_Now()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var res = s.Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [Fact]
        public void SynchronizationContext_ScheduleAction()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var ran = false;
            s.Schedule(() => { ran = true; });
            Assert.True(ms.Count == 1);
            Assert.True(ran);
        }

        [Fact]
        public void SynchronizationContext_ScheduleAction_TimeSpan()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var e = new ManualResetEvent(false);
            s.Schedule(TimeSpan.FromMilliseconds(1), () => { e.Set(); });

            e.WaitOne();
            Assert.True(ms.Count == 1);
        }

        [Fact]
        public void SynchronizationContext_ScheduleAction_DateTimeOffset()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var e = new ManualResetEvent(false);
            s.Schedule(DateTimeOffset.Now.AddMilliseconds(100), () => { e.Set(); });

            e.WaitOne();
            Assert.True(ms.Count >= 1); // Can be > 1 in case of timer queue retry operations.
        }

        [Fact]
        public void SynchronizationContext_ScheduleActionError()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var ex = new Exception();

            try
            {
                s.Schedule(() => { throw ex; });
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(e, ex);
            }

            Assert.True(ms.Count == 1);
        }

        [Fact]
        public void SynchronizationContext_ScheduleActionDue()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var evt = new ManualResetEvent(false);
            var sw = new Stopwatch();
            sw.Start();
            s.Schedule(TimeSpan.FromSeconds(0.2), () => { sw.Stop(); evt.Set(); });
            evt.WaitOne();
            Assert.True(sw.ElapsedMilliseconds > 180, "due " + sw.ElapsedMilliseconds);
            Assert.True(ms.Count == 1);
        }

        private class MySync : SynchronizationContext
        {
            public int Count { get; private set; }

            public override void Post(SendOrPostCallback d, object state)
            {
                Count++;
                d(state);
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotImplementedException();
            }

            public int Started { get; private set; }

            public override void OperationStarted()
            {
                base.OperationStarted();
                Started++;
            }

            public int Completed { get; private set; }

            public override void OperationCompleted()
            {
                base.OperationCompleted();
                Completed++;
            }
        }

        [Fact]
        public void SynchronizationContext_StartedCompleted()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms);

            var started = 0;
            s.Schedule(42, TimeSpan.Zero, (self, x) => { started = ms.Started; return Disposable.Empty; });

            Assert.True(started == 1);
            Assert.True(ms.Count == 1);
            Assert.True(ms.Completed == 1);
        }

        [Fact]
        public void SynchronizationContext_DontPost_Different()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms, false);

            var ran = false;
            s.Schedule(() => { ran = true; });
            Assert.True(ms.Count == 1);
            Assert.True(ran);
        }

#if !NO_THREAD
        [Fact]
        public void SynchronizationContext_DontPost_Same()
        {
            var count = 0;
            var ran = false;

            var t = new Thread(() =>
            {
                var ms = new MySync();
                SynchronizationContext.SetSynchronizationContext(ms);

                var s = new SynchronizationContextScheduler(ms, false);

                s.Schedule(() => { ran = true; });
                count = ms.Count;
            });

            t.Start();
            t.Join();

            Assert.True(count == 0 /* no post */);
            Assert.True(ran);
        }
#endif

        [Fact]
        public void SynchronizationContext_AlwaysPost_Different()
        {
            var ms = new MySync();
            var s = new SynchronizationContextScheduler(ms, true);

            var ran = false;
            s.Schedule(() => { ran = true; });
            Assert.True(ms.Count == 1);
            Assert.True(ran);
        }

#if !NO_THREAD
        [Fact]
        public void SynchronizationContext_AlwaysPost_Same()
        {
            var count = 0;
            var ran = false;

            var t = new Thread(() =>
            {
                var ms = new MySync();
                SynchronizationContext.SetSynchronizationContext(ms);

                var s = new SynchronizationContextScheduler(ms, true);

                s.Schedule(() => { ran = true; });
                count = ms.Count;
            });

            t.Start();
            t.Join();

            Assert.True(count == 1 /* post */);
            Assert.True(ran);
        }
#endif
    }
}
