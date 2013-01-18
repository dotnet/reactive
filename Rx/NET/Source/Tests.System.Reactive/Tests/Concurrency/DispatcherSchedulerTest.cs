// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if SILVERLIGHT && !SILVERLIGHTM7
using Microsoft.Silverlight.Testing;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public class DispatcherSchedulerTest : TestBase
    {
        [TestMethod]
        public void Ctor_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new DispatcherScheduler(null));
        }

        [TestMethod]
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

        [TestMethod]
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

            Assert.IsTrue(e != null && e is InvalidOperationException);
        }

        [TestMethod]
        public void Dispatcher()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            Assert.AreSame(disp.Dispatcher, new DispatcherScheduler(disp).Dispatcher);
        }

        [TestMethod]
        public void Now()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var res = new DispatcherScheduler(disp).Now - DateTime.Now;
            Assert.IsTrue(res.Seconds < 1);
        }

        [TestMethod]
        public void Schedule_ArgumentChecking()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var s = new DispatcherScheduler(disp);
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => s.Schedule(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
        }

        [TestMethod]
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
#if SILVERLIGHT
                    Assert.AreEqual(id, Thread.CurrentThread.ManagedThreadId); // Single-threaded test framework
#else
                    Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);
#endif
                    disp.InvokeShutdown();
                    evt.Set();
                });
            });
        }

#if !USE_SL_DISPATCHER
        [TestMethod]
        public void ScheduleError()
        {
            var ex = new Exception();

            var id = Thread.CurrentThread.ManagedThreadId;
            var disp = DispatcherHelpers.EnsureDispatcher();
            var evt = new ManualResetEvent(false);
            disp.UnhandledException += (o, e) =>
            {
#if DESKTOPCLR40 || DESKTOPCLR45
                Assert.AreSame(ex, e.Exception); // CHECK
#else
                Assert.AreSame(ex, e.Exception.InnerException); // CHECK
#endif
                evt.Set();
                e.Handled = true;
            };
            var sch = new DispatcherScheduler(disp);
            sch.Schedule(() => { throw ex; });
            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [TestMethod]
        public void ScheduleRelative()
        {
            ScheduleRelative_(TimeSpan.FromSeconds(0.2));
        }

        [TestMethod]
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
                Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);

                sch.Schedule(delay, () =>
                {
                    Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);
                    evt.Set();
                });
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [TestMethod]
        public void ScheduleRelative_Cancel()
        {
            var evt = new ManualResetEvent(false);
            
            var id = Thread.CurrentThread.ManagedThreadId;

            var disp = DispatcherHelpers.EnsureDispatcher();
            var sch = new DispatcherScheduler(disp);
            
            sch.Schedule(TimeSpan.FromSeconds(0.1), () =>
            {
                Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);

                var d = sch.Schedule(TimeSpan.FromSeconds(0.1), () =>
                {
                    Assert.Fail();
                    evt.Set();
                });

                sch.Schedule(() =>
                {
                    d.Dispose();
                });

                sch.Schedule(TimeSpan.FromSeconds(0.2), () =>
                {
                    Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);
                    evt.Set();
                });
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }

        [TestMethod]
        public void SchedulePeriodic_ArgumentChecking()
        {
            var disp = DispatcherHelpers.EnsureDispatcher();
            var s = new DispatcherScheduler(disp);

            ReactiveAssert.Throws<ArgumentNullException>(() => s.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => s.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), x => x));
        }

        [TestMethod]
        public void SchedulePeriodic()
        {
            var evt = new ManualResetEvent(false);

            var id = Thread.CurrentThread.ManagedThreadId;

            var disp = DispatcherHelpers.EnsureDispatcher();
            var sch = new DispatcherScheduler(disp);

            var d = new SingleAssignmentDisposable();

            d.Disposable = sch.SchedulePeriodic(1, TimeSpan.FromSeconds(0.1), n =>
            {
                Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);

                if (n == 3)
                {
                    d.Dispose();

                    sch.Schedule(TimeSpan.FromSeconds(0.2), () =>
                    {
                        Assert.AreNotEqual(id, Thread.CurrentThread.ManagedThreadId);
                        evt.Set();
                    });
                }

                if (n > 3)
                {
                    Assert.Fail();
                }

                return n + 1;
            });

            evt.WaitOne();
            disp.InvokeShutdown();
        }
#endif
    }
}
