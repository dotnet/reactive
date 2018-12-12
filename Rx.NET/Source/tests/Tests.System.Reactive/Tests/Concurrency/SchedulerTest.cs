// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using System.Runtime.CompilerServices;
using System.Threading;
using Xunit;
using Microsoft.Reactive.Testing;

#if HAS_WINFORMS
using System.Windows.Forms;
#endif

using System.Threading.Tasks;


namespace ReactiveTests.Tests
{

    public class SchedulerTest : ReactiveTest
    {
        #region IScheduler

        [Fact]
        public void Scheduler_ArgumentChecks()
        {
            var ms = new MyScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAction(default, new object(), state => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, 1, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, default(Action<Action>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, DateTimeOffset.Now, a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, DateTimeOffset.Now, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, 1, DateTimeOffset.Now, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, DateTimeOffset.Now, default(Action<Action<DateTimeOffset>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, DateTimeOffset.Now, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, TimeSpan.Zero, a => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, TimeSpan.Zero, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(default, 1, TimeSpan.Zero, (a, s) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, TimeSpan.Zero, default(Action<Action<TimeSpan>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Schedule(ms, 1, TimeSpan.Zero, default));
        }

        [Fact]
        public void Schedulers_ArgumentChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.CurrentThread.Schedule(DateTimeOffset.MaxValue, default(Action)));
#if DESKTOPCLR
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherScheduler.Instance.Schedule(DateTimeOffset.MaxValue, default(Action)));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Immediate.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => NewThreadScheduler.Default.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskPoolScheduler.Default.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.Schedule(DateTimeOffset.MaxValue, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DefaultScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromSeconds(-1), _ => _));
#if HAS_WINFORMS
            var lbl = new Label();
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).ScheduleAction(new object(), default(Action<object>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ControlScheduler(lbl).Schedule(DateTimeOffset.MaxValue, default(Action)));
#endif
            var ctx = new SynchronizationContext();
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(default(Action)));

            /* Unmerged change from project 'Tests.System.Reactive(netcoreapp2.0)'
            Before:
                        ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).ScheduleAction(new object(), default(Action<object>)));
            After:
                        ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).ScheduleAction(new object(), default)));
            */

            /* Unmerged change from project 'Tests.System.Reactive(net46)'
            Before:
                        ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).ScheduleAction(new object(), default(Action<object>)));
            After:
                        ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).ScheduleAction(new object(), default)));
            */
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).ScheduleAction(new object(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(TimeSpan.Zero, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => new SynchronizationContextScheduler(ctx).Schedule(DateTimeOffset.MaxValue, default(Action)));
        }

        [Fact]
        public void Scheduler_ScheduleNonRecursive()
        {
            var ms = new MyScheduler();
            var res = false;
            Scheduler.Schedule(ms, a => { res = true; });
            Assert.True(res);
        }

        [Fact]
        public void Scheduler_ScheduleRecursive()
        {
            var ms = new MyScheduler();
            var i = 0;
            Scheduler.Schedule(ms, a => { if (++i < 10) { a(); } });
            Assert.Equal(10, i);
        }

        [Fact]
        public void Scheduler_Schedule_With_State()
        {
            var ms = new MyScheduler();
            var res = false;
            Scheduler.ScheduleAction(ms, "state", state => { Assert.Equal("state", state); res = true; });
            Assert.True(res);
        }

        [Fact]
        public void Scheduler_ScheduleWithTimeNonRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.True(t == TimeSpan.Zero); } };
            var res = false;
            Scheduler.Schedule(ms, now, a => { res = true; });
            Assert.True(res);
            Assert.True(ms.WaitCycles == 0);
        }

        [Fact]
        public void Scheduler_ScheduleWithTimeRecursive()
        {
            var now = DateTimeOffset.Now;
            var i = 0;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.True(t == TimeSpan.Zero); } };
            Scheduler.Schedule(ms, now, a => { if (++i < 10) { a(now); } });
            Assert.True(ms.WaitCycles == 0);
            Assert.Equal(10, i);
        }

        [Fact]
        public void Scheduler_ScheduleWithTimeSpanNonRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.True(t == TimeSpan.Zero); } };
            var res = false;
            Scheduler.Schedule(ms, TimeSpan.Zero, a => { res = true; });
            Assert.True(res);
            Assert.True(ms.WaitCycles == 0);
        }

        [Fact]
        public void Scheduler_ScheduleWithTimeSpanRecursive()
        {
            var now = DateTimeOffset.Now;
            var ms = new MyScheduler(now) { Check = (a, s, t) => { Assert.True(t < TimeSpan.FromTicks(10)); } };
            var i = 0;
            Scheduler.Schedule(ms, TimeSpan.Zero, a => { if (++i < 10) { a(TimeSpan.FromTicks(i)); } });
            Assert.True(ms.WaitCycles == Enumerable.Range(1, 9).Sum());
            Assert.Equal(10, i);
        }

        [Fact]
        public void Scheduler_StateThreading()
        {
            var lst = new List<int>();
            Scheduler.Immediate.Schedule(0, (i, a) =>
            {
                lst.Add(i);
                if (i < 9)
                {
                    a(i + 1);
                }
            });

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Scheduler_Builtins()
        {
            // Default
            {
                var e = new ManualResetEvent(false);
                Scheduler.Default.Schedule(() => e.Set());
                e.WaitOne();
            }

            Scheduler_Builtins_NoPlib();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Scheduler_Builtins_NoPlib()
        {
            // ThreadPool
            {
                var e = new ManualResetEvent(false);
                Scheduler.ThreadPool.Schedule(() => e.Set());
                e.WaitOne();
            }

#if !NO_THREAD
            // NewThread
            {
                var e = new ManualResetEvent(false);
                Scheduler.NewThread.Schedule(() => e.Set());
                e.WaitOne();
            }
#endif

            // TaskPool
            {
                var e = new ManualResetEvent(false);
                Scheduler.TaskPool.Schedule(() => e.Set());
                e.WaitOne();
            }
        }

        #endregion

        #region ISchedulerLongRunning

#if !NO_PERF

#if !WINDOWS && !NO_THREAD
        [Fact]
        public void Scheduler_LongRunning_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(null, c => { }));

            /* Unmerged change from project 'Tests.System.Reactive(net46)'
            Before:
                        ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(ThreadPoolScheduler.Instance, default(Action<ICancelable>)));
            After:
                        ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(ThreadPoolScheduler.Instance, default));
            */
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleLongRunning(ThreadPoolScheduler.Instance, default));
        }

        [Fact]
        public void Scheduler_Periodic_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(null, TimeSpan.FromSeconds(1), () => { }));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, TimeSpan.FromSeconds(-1), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, TimeSpan.FromSeconds(1), default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(null, 42, TimeSpan.FromSeconds(1), _ => { }));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(-1), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(1), default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(null, 42, TimeSpan.FromSeconds(1), _ => _));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(-1), _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.SchedulePeriodic(ThreadPoolScheduler.Instance, 42, TimeSpan.FromSeconds(1), default));
        }
#endif

        [Fact]
        public void Scheduler_Stopwatch_Emulation()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.StartStopwatch(null));
        }

        [Fact]
        public void Scheduler_LongRunning1()
        {
            var s = TaskPoolScheduler.Default;

            var x = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var d = s.ScheduleLongRunning(42, (state, cancel) =>
            {
                while (!cancel.IsDisposed)
                {
                    x.Set();
                }

                e.Set();
            });

            x.WaitOne();
            d.Dispose();

            e.WaitOne();
        }

        [Fact]
        public void Scheduler_LongRunning2()
        {
            var s = TaskPoolScheduler.Default;

            var x = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var d = s.ScheduleLongRunning(cancel =>
            {
                while (!cancel.IsDisposed)
                {
                    x.Set();
                }

                e.Set();
            });

            x.WaitOne();
            d.Dispose();

            e.WaitOne();
        }
#endif

        #endregion

        #region ISchedulerPeriodic

#if !NO_PERF

#if !WINDOWS && !NO_THREAD
        [Fact]
        public void Scheduler_Periodic1()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(TimeSpan.FromMilliseconds(50), () =>
            {
                if (n++ == 10)
                {
                    e.Set();
                }
            });

            e.WaitOne();
            d.Dispose();
        }

        [Fact]
        public void Scheduler_Periodic2()
        {
            var n = 0;
            var e = new ManualResetEvent(false);

            var d = ThreadPoolScheduler.Instance.SchedulePeriodic(42, TimeSpan.FromMilliseconds(50), x =>
            {
                Assert.Equal(42, x);

                if (n++ == 10)
                {
                    e.Set();
                }
            });

            e.WaitOne();
            d.Dispose();
        }
#endif

#if DESKTOPCLR && NET46
        [Fact]
        public void Scheduler_Periodic_HostLifecycleManagement()
        {
            var cur = AppDomain.CurrentDomain.BaseDirectory;

            var domain = AppDomain.CreateDomain("HLN", null, new AppDomainSetup { ApplicationBase = cur });

            domain.DoCallBack(Scheduler_Periodic_HostLifecycleManagement_Callback);
        }

        private static void Scheduler_Periodic_HostLifecycleManagement_Callback()
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
                    {
                        e.Set();
                    }
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
        }

        private class PEP : IPlatformEnlightenmentProvider
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
                    return (T)_hln;
                }

                return _old.GetService<T>(args);
            }
        }

        private class HLN : IHostLifecycleNotifications
        {
            public event EventHandler<HostSuspendingEventArgs> Suspending;
            public event EventHandler<HostResumingEventArgs> Resuming;

            public void OnSuspending()
            {
                Suspending?.Invoke(this, null);
            }

            public void OnResuming()
            {
                Resuming?.Invoke(this, null);
            }
        }
#endif

#endif

        #endregion

        #region DisableOptimizations

#if !NO_PERF
        [Fact]
        public void DisableOptimizations_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(default, new Type[0]));
#if !WINDOWS && !NO_THREAD
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(ThreadPoolScheduler.Instance, default));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.DisableOptimizations(Scheduler.Default).Schedule(42, DateTimeOffset.Now, default));
        }

        [Fact]
        public void DisableOptimizations1()
        {
            var s = TaskPoolScheduler.Default;
            Assert.True(s is IServiceProvider);

            var t = s.DisableOptimizations();

            var d = t.Now - s.Now;
            Assert.True(d.TotalSeconds < 1);

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

        [Fact]
        public void DisableOptimizations2()
        {
            var s = TaskPoolScheduler.Default;
            Assert.True(s is IServiceProvider);

            var lr1 = ((IServiceProvider)s).GetService(typeof(ISchedulerLongRunning));
            Assert.NotNull(lr1);

            var e1 = new ManualResetEvent(false);
            s.Schedule(42, (self, state) =>
            {
                Assert.True(self is IServiceProvider);

                var lrr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.NotNull(lrr1);

                e1.Set();
                return Disposable.Empty;
            });
            e1.WaitOne();

            var t = s.DisableOptimizations();
            Assert.True(t is IServiceProvider);

            var lr2 = ((IServiceProvider)t).GetService(typeof(ISchedulerLongRunning));
            Assert.Null(lr2);

            var e2 = new ManualResetEvent(false);
            t.Schedule(42, (self, state) =>
            {
                Assert.True(self is IServiceProvider);

                var lrr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.Null(lrr2);

                e2.Set();
                return Disposable.Empty;
            });
            e2.WaitOne();
        }

        [Fact]
        public void DisableOptimizations3()
        {
            var s = TaskPoolScheduler.Default;
            Assert.True(s is IServiceProvider);

            var lr1 = ((IServiceProvider)s).GetService(typeof(ISchedulerLongRunning));
            Assert.NotNull(lr1);

            var p1 = ((IServiceProvider)s).GetService(typeof(ISchedulerPeriodic));
            Assert.NotNull(p1);

            var e1 = new ManualResetEvent(false);
            s.Schedule(42, (self, state) =>
            {
                Assert.True(self is IServiceProvider);

                var lrr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.NotNull(lrr1);

                var pr1 = ((IServiceProvider)self).GetService(typeof(ISchedulerPeriodic));
                Assert.NotNull(pr1);

                e1.Set();
                return Disposable.Empty;
            });
            e1.WaitOne();

            var t = s.DisableOptimizations(typeof(ISchedulerLongRunning));
            Assert.True(t is IServiceProvider);

            var lr2 = ((IServiceProvider)t).GetService(typeof(ISchedulerLongRunning));
            Assert.Null(lr2);

            var p2 = ((IServiceProvider)t).GetService(typeof(ISchedulerPeriodic));
            Assert.NotNull(p2);

            var e2 = new ManualResetEvent(false);
            t.Schedule(42, (self, state) =>
            {
                Assert.True(self is IServiceProvider);

                var lrr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerLongRunning));
                Assert.Null(lrr2);

                var pr2 = ((IServiceProvider)self).GetService(typeof(ISchedulerPeriodic));
                Assert.NotNull(pr2);

                e2.Set();
                return Disposable.Empty;
            });
            e2.WaitOne();
        }
#endif

        [Fact]
        public void DisableOptimizations_UnknownService()
        {
            var s = new MyScheduler();
            Assert.False(s is IServiceProvider);

            var d = s.DisableOptimizations();
            Assert.True(d is IServiceProvider);
            Assert.Null(((IServiceProvider)d).GetService(typeof(bool)));
        }

        private class MyScheduler : IScheduler
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

        [Fact]
        public void Catch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(default, _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Catch<Exception>(Scheduler.Default, _ => true).Schedule(42, DateTimeOffset.Now, default));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Catch_Custom_Unhandled()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            scheduler.Catch<InvalidOperationException>(_ => true).Schedule(() =>
            {
                throw new InvalidOperationException();
            });
            Assert.Null(err);

            var ex = new ArgumentException();
            scheduler.Catch<InvalidOperationException>(_ => true).Schedule(() =>
            {
                throw ex;
            });
            Assert.Same(ex, err);
        }

        [Fact]
        public void Catch_Custom_Rethrow()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_);

            var ex = new InvalidOperationException();
            scheduler.Catch<InvalidOperationException>(_ => false).Schedule(() =>
            {
                throw ex;
            });
            Assert.Same(ex, err);
        }

        [Fact]
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
            Assert.True(caught);
            Assert.Null(err);

            var ex = new ArgumentException();
            slr.ScheduleLongRunning(cancel =>
            {
                throw ex;
            });
            Assert.Same(ex, err);
        }

        [Fact]
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
            Assert.True(done);

            var ex = new InvalidOperationException();
            slr.ScheduleLongRunning(cancel =>
            {
                throw ex;
            });
            Assert.Same(ex, err);
        }

        [Fact]
        public void Catch_Custom_Periodic_Regular()
        {
            var scheduler = new MyExceptionScheduler(_ => { })
            {
                PeriodicStopped = new ManualResetEvent(false)
            };

            var @catch = scheduler.Catch<InvalidOperationException>(_ => true);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var madeProgress = new ManualResetEvent(false);
            var d = per.SchedulePeriodic(0, TimeSpan.Zero, x =>
            {
                if (x > 10)
                {
                    madeProgress.Set();
                }

                return x + 1;
            });

            madeProgress.WaitOne();
            d.Dispose();
            scheduler.PeriodicStopped.WaitOne();
        }

        [Fact]
        public void Catch_Custom_Periodic_Uncaught1()
        {
            var err = default(Exception);
            var done = new ManualResetEvent(false);
            var scheduler = new MyExceptionScheduler(ex_ => { err = ex_; done.Set(); })
            {
                PeriodicStopped = new ManualResetEvent(false)
            };

            var @catch = scheduler.Catch<InvalidOperationException>(_ => true);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var ex = new ArgumentException();
            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw ex;
            });

            scheduler.PeriodicStopped.WaitOne();
            done.WaitOne();
            Assert.Same(ex, err);
        }

        [Fact]
        public void Catch_Custom_Periodic_Uncaught2()
        {
            var err = default(Exception);
            var done = new ManualResetEvent(false);
            var scheduler = new MyExceptionScheduler(ex_ => { err = ex_; done.Set(); })
            {
                PeriodicStopped = new ManualResetEvent(false)
            };

            var @catch = scheduler.Catch<InvalidOperationException>(_ => false);
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            var ex = new InvalidOperationException();
            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw ex;
            });

            scheduler.PeriodicStopped.WaitOne();
            done.WaitOne();
            Assert.Same(ex, err);
        }

        [Fact]
        public void Catch_Custom_Periodic_Caught()
        {
            var err = default(Exception);
            var scheduler = new MyExceptionScheduler(ex_ => err = ex_)
            {
                PeriodicStopped = new ManualResetEvent(false)
            };

            var caught = new ManualResetEvent(false);
            var @catch = scheduler.Catch<InvalidOperationException>(_ => { caught.Set(); return true; });
            var per = (ISchedulerPeriodic)((IServiceProvider)@catch).GetService(typeof(ISchedulerPeriodic));

            per.SchedulePeriodic(42, TimeSpan.Zero, x =>
            {
                throw new InvalidOperationException();
            });

            scheduler.PeriodicStopped.WaitOne();
            caught.WaitOne();
            Assert.Null(err);
        }

        private class MyExceptionScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
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
                        for (var i = 0; true; i++)
                        {
                            if (i > 100 /* mimic delayed cancellation */ && b.IsDisposed)
                            {
                                break;
                            }

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

        [Fact]
        public void InvalidService_Null()
        {
            var s = new MySchedulerWithoutServices();
            Assert.Null(((IServiceProvider)s).GetService(typeof(IAsyncResult)));
        }

        private class MySchedulerWithoutServices : LocalScheduler
        {
            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void DetectServices_Null_1()
        {
            var s = new MyDumbScheduler1();
            Assert.Null(Scheduler.AsLongRunning(s));
            Assert.Null(Scheduler.AsPeriodic(s));
            Assert.Null(Scheduler.AsStopwatchProvider(s));
        }

        private class MyDumbScheduler1 : IScheduler
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

        [Fact]
        public void DetectServices_Null_2()
        {
            var s = new MyDumbScheduler2(new Dictionary<Type, object>());
            Assert.Null(Scheduler.AsLongRunning(s));
            Assert.Null(Scheduler.AsPeriodic(s));
            Assert.Null(Scheduler.AsStopwatchProvider(s));
        }

        [Fact]
        public void DetectServices_Found()
        {
            {
                var x = new MyLongRunning();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(ISchedulerLongRunning), x }
                });

                Assert.Equal(x, Scheduler.AsLongRunning(s));
            }

            {
                var x = new MyStopwatchProvider();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(IStopwatchProvider), x }
                });

                Assert.Equal(x, Scheduler.AsStopwatchProvider(s));
            }

            {
                var x = new MyPeriodic();

                var s = new MyDumbScheduler2(new Dictionary<Type, object>
                {
                    { typeof(ISchedulerPeriodic), x }
                });

                Assert.Equal(x, Scheduler.AsPeriodic(s));
            }
        }

        private class MyDumbScheduler2 : IScheduler, IServiceProvider
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
                if (_services.TryGetValue(serviceType, out var res))
                {
                    return res;
                }

                return null;
            }
        }

        private class MyLongRunning : ISchedulerLongRunning
        {
            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                throw new NotImplementedException();
            }
        }

        private class MyStopwatchProvider : IStopwatchProvider
        {
            public IStopwatch StartStopwatch()
            {
                throw new NotImplementedException();
            }
        }

        private class MyPeriodic : ISchedulerPeriodic
        {
            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        [Fact]
        public void SchedulerAsync_Yield_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Yield(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Yield(default, CancellationToken.None));
        }

        [Fact]
        public void SchedulerAsync_Sleep_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Sleep(default, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Sleep(default, TimeSpan.Zero, CancellationToken.None));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Sleep(default, DateTimeOffset.MinValue));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.Sleep(default, DateTimeOffset.MinValue, CancellationToken.None));
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_ArgumentChecking()
        {
            var tcs = new TaskCompletionSource<IDisposable>();
            var t = tcs.Task;

            var d = default(IScheduler);
            var s = Scheduler.Immediate;

            var rt = TimeSpan.Zero;
            var at = DateTimeOffset.MinValue;

            var a1 = new Func<IScheduler, int, CancellationToken, Task>((_, __, ___) => t);
            var d1 = default(Func<IScheduler, int, CancellationToken, Task>);

            var a2 = new Func<IScheduler, int, CancellationToken, Task<IDisposable>>((_, __, ___) => t);
            var d2 = default(Func<IScheduler, int, CancellationToken, Task<IDisposable>>);

            var a3 = new Func<IScheduler, CancellationToken, Task>((_, __) => t);
            var d3 = default(Func<IScheduler, CancellationToken, Task>);

            var a4 = new Func<IScheduler, CancellationToken, Task<IDisposable>>((_, __) => t);
            var d4 = default(Func<IScheduler, CancellationToken, Task<IDisposable>>);

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, a1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, d1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, rt, a1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, rt, d1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, at, a1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, at, d1));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, a2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, d2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, rt, a2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, rt, d2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, 42, at, a2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, 42, at, d2));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, a3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, d3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, rt, a3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, rt, d3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, at, a3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, at, d3));

            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, a4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, d4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, rt, a4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, rt, d4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(d, at, a4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Scheduler.ScheduleAsync(s, at, d4));
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_Overloads1()
        {
            var tcsI = new TaskCompletionSource<int>();
            var t = tcsI.Task;
            tcsI.SetResult(0);

            var tcsD = new TaskCompletionSource<IDisposable>();
            var d = tcsD.Task;
            tcsD.SetResult(Disposable.Empty);

            var s = new TestScheduler();

            var o = s.CreateObserver<int>();

            s.ScheduleAsync((_, ct) =>
            {
                o.OnNext(42);
                return t;
            });

            s.ScheduleAsync((_, ct) =>
            {
                o.OnNext(43);
                return d;
            });

            s.ScheduleAsync(44, (_, x, ct) =>
            {
                o.OnNext(x);
                return t;
            });

            s.ScheduleAsync(45, (_, x, ct) =>
            {
                o.OnNext(45);
                return d;
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(1, 42),
                OnNext(1, 43),
                OnNext(1, 44),
                OnNext(1, 45)
            );
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_Overloads2()
        {
            var tcsI = new TaskCompletionSource<int>();
            var t = tcsI.Task;
            tcsI.SetResult(0);

            var tcsD = new TaskCompletionSource<IDisposable>();
            var d = tcsD.Task;
            tcsD.SetResult(Disposable.Empty);

            var s = new TestScheduler();

            var o = s.CreateObserver<int>();

            s.ScheduleAsync(TimeSpan.FromTicks(50), (_, ct) =>
            {
                o.OnNext(42);
                return t;
            });

            s.ScheduleAsync(TimeSpan.FromTicks(60), (_, ct) =>
            {
                o.OnNext(43);
                return d;
            });

            s.ScheduleAsync(44, TimeSpan.FromTicks(70), (_, x, ct) =>
            {
                o.OnNext(x);
                return t;
            });

            s.ScheduleAsync(45, TimeSpan.FromTicks(80), (_, x, ct) =>
            {
                o.OnNext(45);
                return d;
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(50, 42),
                OnNext(60, 43),
                OnNext(70, 44),
                OnNext(80, 45)
            );
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_Overloads3()
        {
            var tcsI = new TaskCompletionSource<int>();
            var t = tcsI.Task;
            tcsI.SetResult(0);

            var tcsD = new TaskCompletionSource<IDisposable>();
            var d = tcsD.Task;
            tcsD.SetResult(Disposable.Empty);

            var s = new TestScheduler();

            var o = s.CreateObserver<int>();

            s.ScheduleAsync(new DateTimeOffset(50, TimeSpan.Zero), (_, ct) =>
            {
                o.OnNext(42);
                return t;
            });

            s.ScheduleAsync(new DateTimeOffset(60, TimeSpan.Zero), (_, ct) =>
            {
                o.OnNext(43);
                return d;
            });

            s.ScheduleAsync(44, new DateTimeOffset(70, TimeSpan.Zero), (_, x, ct) =>
            {
                o.OnNext(x);
                return t;
            });

            s.ScheduleAsync(45, new DateTimeOffset(80, TimeSpan.Zero), (_, x, ct) =>
            {
                o.OnNext(45);
                return d;
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(50, 42),
                OnNext(60, 43),
                OnNext(70, 44),
                OnNext(80, 45)
            );
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_NoCancellation1()
        {
            var s = new TestScheduler();

            var o = s.CreateObserver<int>();

            s.ScheduleAsync(async (_, ct) =>
            {
                o.OnNext(42);

                await _.Yield();

                o.OnNext(43);

                await _.Sleep(TimeSpan.FromTicks(10));

                o.OnNext(44);

                await _.Sleep(new DateTimeOffset(250, TimeSpan.Zero));

                o.OnNext(45);
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(1, 42),
                OnNext(2, 43),
                OnNext(12, 44),
                OnNext(250, 45)
            );
        }

        [Fact]
        public void SchedulerAsync_ScheduleAsync_NoCancellation2()
        {
            var s = new TestScheduler();

            var o = s.CreateObserver<int>();

            s.ScheduleAsync(async (_, ct) =>
            {
                o.OnNext(42);

                await _.Yield(ct);

                o.OnNext(43);

                await _.Sleep(TimeSpan.FromTicks(10), ct);

                o.OnNext(44);

                await _.Sleep(new DateTimeOffset(250, TimeSpan.Zero), ct);

                o.OnNext(45);
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(1, 42),
                OnNext(2, 43),
                OnNext(12, 44),
                OnNext(250, 45)
            );
        }

        [Fact]
        public void SchedulerAsync_Awaiters()
        {
            var op = Scheduler.Immediate.Yield();
            var aw = op.GetAwaiter();

            ReactiveAssert.Throws<ArgumentNullException>(() => aw.OnCompleted(null));

            aw.OnCompleted(() => { });

            ReactiveAssert.Throws<InvalidOperationException>(() => aw.OnCompleted(() => { }));
        }

        [Fact]
        public void SchedulerAsync_Yield_Cancel1()
        {
            var cts = new CancellationTokenSource();

            var op = Scheduler.Immediate.Yield(cts.Token);
            var aw = op.GetAwaiter();

            cts.Cancel();

            Assert.True(aw.IsCompleted);

            ReactiveAssert.Throws<OperationCanceledException>(() => aw.GetResult());
        }

        [Fact]
        public void SchedulerAsync_Yield_Cancel2()
        {
            var cts = new CancellationTokenSource();

            var op = Scheduler.Immediate.Yield(cts.Token);
            var aw = op.GetAwaiter();

            Assert.False(aw.IsCompleted);

            aw.OnCompleted(() =>
            {
                //
                // TODO: Not needed for await pattern, but maybe should be wired up?
                //
                // Assert.True(aw.IsCompleted);

                cts.Cancel();

                ReactiveAssert.Throws<OperationCanceledException>(() => aw.GetResult());
            });
        }

        [Fact]
        public void SchedulerAsync_Sleep_Cancel()
        {
            var cts = new CancellationTokenSource();

            var op = Scheduler.Default.Sleep(TimeSpan.FromHours(1), cts.Token);
            var aw = op.GetAwaiter();

            Assert.False(aw.IsCompleted);

            var e = new ManualResetEvent(false);

            aw.OnCompleted(() =>
            {
                ReactiveAssert.Throws<OperationCanceledException>(() => aw.GetResult());

                e.Set();
            });

            cts.Cancel();

            e.WaitOne();
        }

#if !NO_SYNCCTX

        [Fact]
        public void SchedulerAsync_ScheduleAsync_SyncCtx()
        {
            var old = SynchronizationContext.Current;

            try
            {
                var ctx = new MySyncCtx();
                SynchronizationContext.SetSynchronizationContext(ctx);

                var s = new TestScheduler();

                var o = s.CreateObserver<int>();

                s.ScheduleAsync(async (_, ct) =>
                {
                    Assert.Same(ctx, SynchronizationContext.Current);

                    o.OnNext(42);

                    await _.Yield(ct).ConfigureAwait(true);

                    Assert.Same(ctx, SynchronizationContext.Current);

                    o.OnNext(43);

                    await _.Sleep(TimeSpan.FromTicks(10), ct).ConfigureAwait(true);

                    Assert.Same(ctx, SynchronizationContext.Current);

                    o.OnNext(44);

                    await _.Sleep(new DateTimeOffset(250, TimeSpan.Zero), ct).ConfigureAwait(true);

                    Assert.Same(ctx, SynchronizationContext.Current);

                    o.OnNext(45);
                });

                s.Start();

                o.Messages.AssertEqual(
                    OnNext(1, 42),
                    OnNext(2, 43),
                    OnNext(12, 44),
                    OnNext(250, 45)
                );
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(old);
            }
        }

        private class MySyncCtx : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state)
            {
                d(state);
            }
        }

#endif

    }
}
