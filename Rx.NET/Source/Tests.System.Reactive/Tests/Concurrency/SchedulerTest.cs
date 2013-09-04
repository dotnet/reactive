// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;

#if HAS_WINFORMS
using System.Windows.Forms;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public class SchedulerTest
    {
        #region IScheduler

        [TestMethod]
        public void Scheduler_ArgumentChecks()
        {
            var ms = new MyScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), 1, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, default(Action<Action>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, default(Action<int, Action<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), DateTimeOffset.Now, a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), DateTimeOffset.Now, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), 1, DateTimeOffset.Now, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, DateTimeOffset.Now, default(Action<Action<DateTimeOffset>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, DateTimeOffset.Now, default(Action<int, Action<int, DateTimeOffset>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), TimeSpan.Zero, a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), TimeSpan.Zero, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default(IScheduler), 1, TimeSpan.Zero, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, TimeSpan.Zero, default(Action<Action<TimeSpan>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, TimeSpan.Zero, default(Action<int, Action<int, TimeSpan>>)));
        }

        [TestMethod]
        public void Schedulers_ArgumentChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(DateTimeOffset.MaxValue, default(Action)));
#if !NO_TPL
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(DateTimeOffset.MaxValue, default(Action)));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
#if HAS_WINFORMS
            var lbl = new Label();
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(DateTimeOffset.MaxValue, default(Action)));
#endif
            var ctx = new SynchronizationContext();
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(DateTimeOffset.MaxValue, default(Action)));
        }

        [TestMethod]
        public void Scheduler_ScheduleNonRecursive()
        {
            var ms = new MyScheduler();
            var res = false;
            Scheduler.Schedule(ms, a => { res = true; });
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Scheduler_ScheduleRecursive()
        {
            var ms = new MyScheduler();
            var i = 0;
            Scheduler.Schedule(ms, a => { if (++i < 10) a(); });
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void Scheduler_ScheduleWithTimeNonRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.IsTrue(t == TimeSpan.Zero); } };
            var res = false;
            Scheduler.Schedule(ms, now, a => { res = true; });
            Assert.IsTrue(res);
            Assert.IsTrue(ms.WaitCycles == 0);
        }

        [TestMethod]
        public void Scheduler_ScheduleWithTimeRecursive()
        {
            var now = DateTimeOffset.Now;
            var i = 0;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.IsTrue(t == TimeSpan.Zero); } };
            Scheduler.Schedule(ms, now, a => { if (++i < 10) a(now); });
            Assert.IsTrue(ms.WaitCycles == 0);
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void Scheduler_ScheduleWithTimeSpanNonRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.IsTrue(t == TimeSpan.Zero); } };
            var res = false;
            Scheduler.Schedule(ms, TimeSpan.Zero, a => { res = true; });
            Assert.IsTrue(res);
            Assert.IsTrue(ms.WaitCycles == 0);
        }

        [TestMethod]
        public void Scheduler_ScheduleWithTimeSpanRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.IsTrue(t < TimeSpan.FromTicks(10)); } };
            var i = 0;
            Scheduler.Schedule(ms, TimeSpan.Zero, a => { if (++i < 10) a(TimeSpan.FromTicks(i)); });
            Assert.IsTrue(ms.WaitCycles == Enumerable.Range(1, 9).Sum());
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void Scheduler_StateThreading()
        {
            var lst = new List<int>();
            Scheduler.Immediate.Schedule(0, (i, a) =>
            {
                lst.Add(i);
                if (i < 9)
                    a(i + 1);
            });

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Scheduler_Builtins()
        {
            // Default
            {
                var e = new ManualResetEvent(false);
                Scheduler.Default.Schedule(() => e.Set());
                e.WaitOne();
            }

            if (!Utils.IsRunningWithPortableLibraryBinaries())
            {
                Scheduler_Builtins_NoPlib();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Scheduler_Builtins_NoPlib()
        {
#if !PLIB
            // ThreadPool
            {
                var e = new ManualResetEvent(false);
                Scheduler.ThreadPool.Schedule(() => e.Set());
                e.WaitOne();
            }
#endif

#if !NO_THREAD
            // NewThread
            {
                var e = new ManualResetEvent(false);
                Scheduler.NewThread.Schedule(() => e.Set());
                e.WaitOne();
            }
#endif

#if !NO_TPL
            // TaskPool
            {
                var e = new ManualResetEvent(false);
                Scheduler.TaskPool.Schedule(() => e.Set());
                e.WaitOne();
            }
#endif
        }

        #endregion

        #region ISchedulerLongRunning

#if !NO_PERF
        [TestMethod]
        public void Scheduler_LongRunning_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(null, c => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(ThreadPoolScheduler.Instance, default(Action<ICancelable>)));
        }

        [TestMethod]
        public void Scheduler_Periodic_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(null, TimeSpan.FromSeconds(1), () => { }));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, TimeSpan.FromSeconds(-1), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, TimeSpan.FromSeconds(1), default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic<int>(null, 42, TimeSpan.FromSeconds(1), _ => { }));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic<int>(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(-1), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic<int>(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(1), default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic<int>(null, 42, TimeSpan.FromSeconds(1), _ => _));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic<int>(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(-1), _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic<int>(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(1), default(Func<int, int>)));
        }

        [TestMethod]
        public void Scheduler_Stopwatch_Emulation()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.StartStopwatch(null));
        }

#if !NO_TPL
        [TestMethod]
        public void Scheduler_LongRunning1()
        {
            var s = TaskPoolScheduler.Default;

            var x = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var d = s.ScheduleLongRunning(42, (state, cancel) =>
            {
                while (!cancel.IsDisposed)
                    x.Set();
                e.Set();
            });

            x.WaitOne();
            d.Dispose();

            e.WaitOne();
        }

        [TestMethod]
        public void Scheduler_LongRunning2()
        {
            var s = TaskPoolScheduler.Default;

            var x = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var d = s.ScheduleLongRunning(cancel =>
            {
                while (!cancel.IsDisposed)
                    x.Set();
                e.Set();
            });

            x.WaitOne();
            d.Dispose();

            e.WaitOne();
        }
#endif
#endif

        #endregion

        #region ISchedulerPeriodic

#if !NO_PERF
        [TestMethod]
        public void Scheduler_Periodic1()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(TimeSpan.FromMilliseconds(50), () =>
            {
                if (n++ == 10)
                    e.Set();
            });

            e.WaitOne();
            d.Dispose();
        }

        [TestMethod]
        public void Scheduler_Periodic2()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromMilliseconds(50), x =>
            {
                Assert.AreEqual(42, x);

                if (n++ == 10)
                    e.Set();
            });

            e.WaitOne();
            d.Dispose();
        }

#if DESKTOPCLR
        [TestMethod]
        public void Scheduler_Periodic_HostLifecycleManagement()
        {
            var cur = AppDomain.CurrentDomain.BaseDirectory;

            var domain = AppDomain.CreateDomain("HLN", null, new AppDomainSetup { ApplicationBase = cur });

            domain.DoCallBack(() =>
            {
                var pep = PlatformEnlightenmentProvider.Current;

                try
                {
                    var hln = new HLN();
                    PlatformEnlightenmentProvider.Current = new PEP(hln);

                    var s = ThreadPoolScheduler.Instance.DisableOptimizations(typeof(ISchedulerPeriodic));

                    var n = 0;
                    var e = new ManualResetEvent(false);

                    var d = Observable.Interval(TimeSpan.FromMilliseconds(100), s).Subscribe(_ =>
                    {
                        if (n++ == 10)
                            e.Set();
                    });

                    hln.OnSuspending();
                    hln.OnResuming();

                    Thread.Sleep(250);
                    hln.OnSuspending();

                    Thread.Sleep(150);
                    hln.OnResuming();

                    Thread.Sleep(50);
                    hln.OnSuspending();
                    hln.OnResuming();

                    e.WaitOne();
                    d.Dispose();
                }
                finally
                {
                    PlatformEnlightenmentProvider.Current = pep;
                }
            });
        }

        class PEP : IPlatformEnlightenmentProvider
        {
            private readonly IPlatformEnlightenmentProvider _old;
            private readonly IHostLifecycleNotifications _hln;

            public PEP(HLN hln)
            {
                _old = PlatformEnlightenmentProvider.Current;
                _hln = hln;
            }

            public T GetService<T>(params object[] args) where T : class
            {
                if (typeof(T) == typeof(IHostLifecycleNotifications))
                {
                    return (T)(object)_hln;
                }

                return _old.GetService<T>(args);
            }
        }

        class HLN : IHostLifecycleNotifications
        {
            public event EventHandler<HostSuspendingEventArgs> Suspending;
            public event EventHandler<HostResumingEventArgs> Resuming;

            public void OnSuspending()
            {
                var s = Suspending;
                if (s != null)
                    s(this, null);
            }

            public void OnResuming()
            {
                var s = Resuming;
                if (s != null)
                    s(this, null);
            }
        }
#endif

#endif

        #endregion

        #region DisableOptimizations

#if !NO_PERF
        [TestMethod]
        public void DisableOptimizations_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(default(IScheduler), new Type[0]));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(ThreadPoolScheduler.Instance, default(Type[])));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
        }

#if !NO_TPL
        [TestMethod]
        public void DisableOptimizations1()
        {
            var s = TaskPoolScheduler.Default;
            Assert.IsTrue(s is IServiceProvider);

            var t = s.DisableOptimizations();

            var d = t.Now - s.Now;
            Assert.IsTrue(d.TotalSeconds < 1);

            var e1 = new ManualResetEvent(false);
            t.Schedule(42, (self, state) =>
            {
                e1.Set();
                return Disposable.Empty;
            });
            e1.WaitOne();

            var e2 = new ManualResetEvent(false);
            t.Schedule(42, TimeSpan.FromMilliseconds(1), (self, state) =>
            {
                e2.Set();
                return Disposable.Empty;
            });
            e2.WaitOne();

            var e3 = new ManualResetEvent(false);
            t.Schedule(42, DateTimeOffset.Now.AddMilliseconds(10), (self, state) =>
            {
                e3.Set();
                return Disposable.Empty;
            });
            e3.WaitOne();
        }

        [TestMethod]
        public void DisableOptimizations2()
        {
            var s = TaskPoolScheduler.Default;
            Assert.IsTrue(s is IServiceProvider);

            var lr1 = ((IServiceProvider)s).GetService(typeof(ISchedulerLongRunning));
            Assert.IsNotNull(lr1);

            var e1 = new ManualResetEvent(false);
            s.Schedule(42, (self, state) =>
            {
                Assert.IsTrue(self is IServiceProvider);

                var lrr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.IsNotNull(lrr1);

                e1.Set();
                return Disposable.Empty;
            });
            e1.WaitOne();

            var t = s.DisableOptimizations();
            Assert.IsTrue(t is IServiceProvider);

            var lr2 = ((IServiceProvider)t).GetService(typeof(ISchedulerLongRunning));
            Assert.IsNull(lr2);

            var e2 = new ManualResetEvent(false);
            t.Schedule(42, (self, state) =>
            {
                Assert.IsTrue(self is IServiceProvider);

                var lrr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.IsNull(lrr2);

                e2.Set();
                return Disposable.Empty;
            });
            e2.WaitOne();
        }

        [TestMethod]
        public void DisableOptimizations3()
        {
            var s = TaskPoolScheduler.Default;
            Assert.IsTrue(s is IServiceProvider);

            var lr1 = ((IServiceProvider)s).GetService(typeof(ISchedulerLongRunning));
            Assert.IsNotNull(lr1);

            var p1 = ((IServiceProvider)s).GetService(typeof(ISchedulerPeriodic));
            Assert.IsNotNull(p1);

            var e1 = new ManualResetEvent(false);
            s.Schedule(42, (self, state) =>
            {
                Assert.IsTrue(self is IServiceProvider);

                var lrr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.IsNotNull(lrr1);

                var pr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerPeriodic));
                Assert.IsNotNull(pr1);

                e1.Set();
                return Disposable.Empty;
            });
            e1.WaitOne();

            var t = s.DisableOptimizations(typeof(ISchedulerLongRunning));
            Assert.IsTrue(t is IServiceProvider);

            var lr2 = ((IServiceProvider)t).GetService(typeof(ISchedulerLongRunning));
            Assert.IsNull(lr2);

            var p2 = ((IServiceProvider)t).GetService(typeof(ISchedulerPeriodic));
            Assert.IsNotNull(p2);

            var e2 = new ManualResetEvent(false);
            t.Schedule(42, (self, state) =>
            {
                Assert.IsTrue(self is IServiceProvider);

                var lrr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.IsNull(lrr2);

                var pr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerPeriodic));
                Assert.IsNotNull(pr2);

                e2.Set();
                return Disposable.Empty;
            });
            e2.WaitOne();
        }
#endif
#endif

        [TestMethod]
        public void DisableOptimizations_UnknownService()
        {
            var s = new MyScheduler();
            Assert.IsFalse(s is IServiceProvider);

            var d = s.DisableOptimizations();
            Assert.IsTrue(d is IServiceProvider);
            Assert.IsNull(((IServiceProvider)d).GetService(typeof(bool)));
        }

        class MyScheduler : IScheduler
        {
            public MyScheduler()
                : this(DateTimeOffset.Now)
            {
            }

            public MyScheduler(DateTimeOffset now)
            {
                Now = now;
            }

            public DateTimeOffset Now
            {
                get;
                private set;
            }

            public IDisposable Schedule<T>(T state, Func<IScheduler, T, IDisposable> action)
            {
                return action(this, state);
            }

            public Action<Action<object>, object, TimeSpan> Check { get; set; }
            public long WaitCycles { get; set; }

            public IDisposable Schedule<T>(T state, TimeSpan dueTime, Func<IScheduler, T, IDisposable> action)
            {
                Check(o => action(this, (T)o), state, dueTime);
                WaitCycles += dueTime.Ticks;
                return action(this, state);
            }

            public IDisposable Schedule<T>(T state, DateTimeOffset dueTime, Func<IScheduler, T, IDisposable> action)
            {
                return Schedule(state, dueTime - Now, action);
            }
        }

        #endregion

        #region Catch

        [TestMethod]
        public void Catch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(default(IScheduler), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, default(Func<Exception, bool>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, TimeSpan.FromSeconds(1), default(Func<IScheduler, int, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, DateTimeOffset.Now, default(Func<IScheduler, int, IDisposable>)));
        }

        [TestMethod]
        public void Catch_Builtin_Swallow_Shallow()
        {
            var done = new ManualResetEvent(false);

            var swallow = Scheduler.Default.Catch<InvalidOperationException>(_ => { done.Set(); return true; });

            swallow.Schedule(() =>
            {
                throw new InvalidOperationException();
            });

            done.WaitOne();
        }

        [TestMethod]
        public void Catch_Builtin_Swallow_Recursive()
        {
            var done = new ManualResetEvent(false);

            var swallow = Scheduler.Default.Catch<InvalidOperationException>(_ => { done.Set(); return true; });

            swallow.Schedule(42, (self, state) =>
            {
                return self.Schedule(() =>
                {
                    throw new InvalidOperationException();
                });
            });

            done.WaitOne();
        }

        [TestMethod]
        public void Catch_Custom_Unhandled()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            scheduler.Catch<InvalidOperationException>(_ => true).Schedule(() =>
            {
                throw new InvalidOperationException();
            });
            Assert.IsNull(err);

            var ex = new ArgumentException();
            scheduler.Catch<InvalidOperationException>(_ => true).Schedule(() =>
            {
                throw ex;
            });
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_Rethrow()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            var ex = new InvalidOperationException();
            scheduler.Catch<InvalidOperationException>(_ => false).Schedule(() =>
            {
                throw ex;
            });
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_LongRunning_Caught()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            var caught = false;
            var @catch = scheduler.Catch<InvalidOperationException>(_ => caught = true);
            var slr = (ISchedulerLongRunning)((IServiceProvider)@catch).GetService(typeof(ISchedulerLongRunning));

            slr.ScheduleLongRunning(cancel =>
            {
                throw new InvalidOperationException();
            });
            Assert.IsTrue(caught);
            Assert.IsNull(err);

            var ex = new ArgumentException();
            slr.ScheduleLongRunning(cancel =>
            {
                throw ex;
            });
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_LongRunning_Rethrow()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            var @catch = scheduler.Catch<InvalidOperationException>(_ => false);
            var slr = (ISchedulerLongRunning)((IServiceProvider)@catch).GetService(typeof(ISchedulerLongRunning));

            var done = false;
            slr.ScheduleLongRunning(cancel =>
            {
                done = true;
            });
            Assert.IsTrue(done);

            var ex = new InvalidOperationException();
            slr.ScheduleLongRunning(cancel =>
            {
                throw ex;
            });
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_Periodic_Regular()
        {
            var scheduler = new MyExceptionScheduler(_ => { });
            scheduler.PeriodicStopped = new ManualResetEvent(false);

            var @catch = scheduler.Catch<InvalidOperationException>(_ => true);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var madeProgress = new ManualResetEvent(false);
            var d = per.SchedulePeriodic(0, TimeSpan.Zero, x =>
            {
                if (x > 10)
                    madeProgress.Set();

                return x + 1;
            });

            madeProgress.WaitOne();
            d.Dispose();
            scheduler.PeriodicStopped.WaitOne();
        }

        [TestMethod]
        public void Catch_Custom_Periodic_Uncaught1()
        {
            var err = default(Exception);
            var done = new ManualResetEvent(false);
            var scheduler = new MyExceptionScheduler(ex_ => { err = ex_; done.Set(); });
            scheduler.PeriodicStopped = new ManualResetEvent(false);

            var @catch = scheduler.Catch<InvalidOperationException>(_ => true);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var ex = new ArgumentException();
            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw ex;
            });

            scheduler.PeriodicStopped.WaitOne();
            done.WaitOne();
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_Periodic_Uncaught2()
        {
            var err = default(Exception);
            var done = new ManualResetEvent(false);
            var scheduler = new MyExceptionScheduler(ex_ => { err = ex_; done.Set(); });
            scheduler.PeriodicStopped = new ManualResetEvent(false);

            var @catch = scheduler.Catch<InvalidOperationException>(_ => false);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var ex = new InvalidOperationException();
            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw ex;
            });

            scheduler.PeriodicStopped.WaitOne();
            done.WaitOne();
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Catch_Custom_Periodic_Caught()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);
            scheduler.PeriodicStopped = new ManualResetEvent(false);

            var caught = new ManualResetEvent(false);
            var @catch = scheduler.Catch<InvalidOperationException>(_ => { caught.Set(); return true; });
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw new InvalidOperationException();
            });

            scheduler.PeriodicStopped.WaitOne();
            caught.WaitOne();
            Assert.IsNull(err);
        }

        class MyExceptionScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
        {
            private readonly Action<Exception> _onError;

            public MyExceptionScheduler(Action<Exception> onError)
            {
                _onError = onError;
            }

            public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                try
                {
                    return action(this, state);
                }
                catch (Exception exception)
                {
                    _onError(exception);
                    return Disposable.Empty;
                }
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                var b = new BooleanDisposable();

                try
                {
                    action(state, b);
                }
                catch (Exception exception)
                {
                    _onError(exception);
                    return Disposable.Empty;
                }

                return b;
            }

            public ManualResetEvent PeriodicStopped { get; set; }

            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                var b = new BooleanDisposable();

                Scheduler.Default.Schedule(() =>
                {
                    try
                    {
                        var s = state;
                        for (int i = 0; true; i++)
                        {
                            if (i > 100 /* mimic delayed cancellation */ && b.IsDisposed)
                                break;
                            s = action(s);
                        }
                    }
                    catch (Exception exception)
                    {
                        _onError(exception);
                    }
                    finally
                    {
                        PeriodicStopped.Set();
                    }
                });

                return b;
            }
        }

        #endregion

        #region Services

        [TestMethod]
        public void InvalidService_Null()
        {
            var s = new MySchedulerWithoutServices();
            Assert.IsNull(((IServiceProvider)s).GetService(typeof(IAsyncResult)));
        }

        class MySchedulerWithoutServices : LocalScheduler
        {
            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void DetectServices_Null_1()
        {
            var s = new MyDumbScheduler1();
            Assert.IsNull(Scheduler.AsLongRunning(s));
            Assert.IsNull(Scheduler.AsPeriodic(s));
            Assert.IsNull(Scheduler.AsStopwatchProvider(s));
        }

        class MyDumbScheduler1 : IScheduler
        {
            public DateTimeOffset Now
            {
                get { throw new NotImplementedException(); }
            }

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void DetectServices_Null_2()
        {
            var s = new MyDumbScheduler2(new Dictionary<Type, object>());
            Assert.IsNull(Scheduler.AsLongRunning(s));
            Assert.IsNull(Scheduler.AsPeriodic(s));
            Assert.IsNull(Scheduler.AsStopwatchProvider(s));
        }

        [TestMethod]
        public void DetectServices_Found()
        {
            {
                var x = new MyLongRunning();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(ISchedulerLongRunning), x }
                });

                Assert.AreEqual(x, Scheduler.AsLongRunning(s));
            }

            {
                var x = new MyStopwatchProvider();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(IStopwatchProvider), x }
                });

                Assert.AreEqual(x, Scheduler.AsStopwatchProvider(s));
            }

            {
                var x = new MyPeriodic();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(ISchedulerPeriodic), x }
                });

                Assert.AreEqual(x, Scheduler.AsPeriodic(s));
            }
        }

        class MyDumbScheduler2 : IScheduler, IServiceProvider
        {
            private readonly Dictionary<Type, object> _services;

            public MyDumbScheduler2(Dictionary<Type, object> services)
            {
                _services = services;
            }

            public DateTimeOffset Now
            {
                get { throw new NotImplementedException(); }
            }

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            public object GetService(Type serviceType)
            {
                var res = default(object);
                if (_services.TryGetValue(serviceType, out res))
                    return res;

                return null;
            }
        }

        class MyLongRunning : ISchedulerLongRunning
        {
            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                throw new NotImplementedException();
            }
        }

        class MyStopwatchProvider : IStopwatchProvider
        {
            public IStopwatch StartStopwatch()
            {
                throw new NotImplementedException();
            }
        }

        class MyPeriodic : ISchedulerPeriodic
        {
            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
