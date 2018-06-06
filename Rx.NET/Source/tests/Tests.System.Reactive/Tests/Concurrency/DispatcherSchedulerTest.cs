// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NET45
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    
    public class DispatcherSchedulerTest : TestBase
    {
        [Fact]
        public void Ctor_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new DispatcherScheduler(null));
        }

        [Fact]
        public void Current()
        {
            var d = DispatcherHelpers.EnsureDispatcher();
            var e = new ManualResetEvent(false);

            d.BeginInvoke(() =>
            {
                var c = DispatcherScheduler.Current;
                c.Schedule(() => { e.Set(); });
            });

            e.WaitOne();
        }

        [Fact]
        public void Current_None()
        {
            var e = default(Exception);

            var t = new Thread(() =>
            {
                try
                {
                    var ignored = DispatcherScheduler.Current;
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            });

            t.Start();
            t.Join();

            Assert.True(e != null && e is InvalidOperationException);
        }

        [Fact]
        public void Dispatcher()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            Assert.Same(disp.Dispatcher, new DispatcherScheduler(disp).Dispatcher);
        }

        [Fact]
        public void Now()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var res = new DispatcherScheduler(disp).Now - DateTime.Now;
            Assert.True(res.Seconds < 1);
        }

        [Fact]
        public void Schedule_ArgumentChecking()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var s = new DispatcherScheduler(disp);
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
        }

        [Fact]
        [Asynchronous]
        public void Schedule()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                var id = Thread.CurrentThread.ManagedThreadId;
                var sch = new DispatcherScheduler(disp);
                sch.Schedule(() =>
                {
                    Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);
                    disp.InvokeShutdown();
                    evt.Set();
                });
            });
        }

        [Fact]
        public void ScheduleError()
        {
            var ex = new Exception();

            var id = Thread.CurrentThread.ManagedThreadId;
            var disp = DispatcherHelpers.EnsureDispatcher();
            var evt = new ManualResetEvent(false);
            disp.UnhandledException += (o, e) =>
            {
#if NET45 || NET46
                Assert.Same(ex, e.Exception); // CHECK
#else
                Assert.Same(ex, e.Exception.InnerException); // CHECK
#endif
                evt.Set();
                e.Handled = true;
            };
            var sch = new DispatcherScheduler(disp);
            sch.Schedule(() => { throw ex; });
            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [Fact]
        public void ScheduleRelative()
        {
            ScheduleRelative_(TimeSpan.FromSeconds(0.2));
        }

        [Fact]
        public void ScheduleRelative_Zero()
        {
            ScheduleRelative_(TimeSpan.Zero);
        }

        private void ScheduleRelative_(TimeSpan delay)
        {
            var evt = new ManualResetEvent(false);

            var id = Thread.CurrentThread.ManagedThreadId;

            var disp = DispatcherHelpers.EnsureDispatcher();
            var sch = new DispatcherScheduler(disp);

            sch.Schedule(delay, () =>
            {
                Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);

                sch.Schedule(delay, () =>
                {
                    Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);
                    evt.Set();
                });
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [Fact]
        public void ScheduleRelative_Cancel()
        {
            var evt = new ManualResetEvent(false);
            
            var id = Thread.CurrentThread.ManagedThreadId;

            var disp = DispatcherHelpers.EnsureDispatcher();
            var sch = new DispatcherScheduler(disp);
            
            sch.Schedule(TimeSpan.FromSeconds(0.1), () =>
            {
                Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);

                var d = sch.Schedule(TimeSpan.FromSeconds(0.1), () =>
                {
                    Assert.True(false);
                    evt.Set();
                });

                sch.Schedule(() =>
                {
                    d.Dispose();
                });

                sch.Schedule(TimeSpan.FromSeconds(0.2), () =>
                {
                    Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);
                    evt.Set();
                });
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [Fact]
        public void SchedulePeriodic_ArgumentChecking()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var s = new DispatcherScheduler(disp);

            ReactiveAssert.Throws<ArgumentNullException>(() => s.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), x => x));
        }

        [Fact]
        public void SchedulePeriodic()
        {
            var evt = new ManualResetEvent(false);

            var id = Thread.CurrentThread.ManagedThreadId;

            var disp = DispatcherHelpers.EnsureDispatcher();
            var sch = new DispatcherScheduler(disp);

            var d = new SingleAssignmentDisposable();

            d.Disposable = sch.SchedulePeriodic(1, TimeSpan.FromSeconds(0.1), n =>
            {
                Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);

                if (n == 3)
                {
                    d.Dispose();

                    sch.Schedule(TimeSpan.FromSeconds(0.2), () =>
                    {
                        Assert.NotEqual(id, Thread.CurrentThread.ManagedThreadId);
                        evt.Set();
                    });
                }

                if (n > 3)
                {
                    Assert.True(false);
                }

                return n + 1;
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }
    }
}
#endif