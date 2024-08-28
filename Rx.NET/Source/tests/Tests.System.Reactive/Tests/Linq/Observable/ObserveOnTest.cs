// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if HAS_DISPATCHER
using System.Windows.Threading;
#endif

#if HAS_WINFORMS
using System.Windows.Forms;
#endif

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObserveOnTest : TestBase
    {
        #region + TestBase +

        [TestMethod]
        public void ObserveOn_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

#if HAS_WINFORMS
#pragma warning disable IDE0034 // (Simplify 'default'.) Want to be explicit about overloads being tested.
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new ControlScheduler(new Label())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(someObservable, default(ControlScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.ObserveOn<int>(default(IObservable<int>), new Label()));
            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.ObserveOn<int>(someObservable, default(Label)));
#pragma warning restore IDE0034
#endif

#if HAS_DISPATCHER
#pragma warning disable IDE0034 // (Simplify 'default'.) Want to be explicit about overloads being tested.
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new DispatcherScheduler(Dispatcher.CurrentDispatcher)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(someObservable, default(DispatcherScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOn<int>(default(IObservable<int>), Dispatcher.CurrentDispatcher));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOn<int>(someObservable, default(Dispatcher)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOnDispatcher<int>(default(IObservable<int>)));
#pragma warning restore IDE0034
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default, new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn(someObservable, default(SynchronizationContext)));
        }

#if HAS_WINFORMS
        [TestMethod]
        public void ObserveOn_Control()
        {
            var okay = true;

            using (WinFormsTestUtils.RunTest(out var lbl))
            {
                var evt = new ManualResetEvent(false);

                Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(lbl).Subscribe(x =>
                {
                    lbl.Text = x.ToString();
                    okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                }, () => evt.Set());

                evt.WaitOne();
            }

            Assert.True(okay);
        }

        [TestMethod]
        public void ObserveOn_ControlScheduler()
        {
            var okay = true;

            using (WinFormsTestUtils.RunTest(out var lbl))
            {
                var evt = new ManualResetEvent(false);
                
                Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(new ControlScheduler(lbl)).Subscribe(x =>
                {
                    lbl.Text = x.ToString();
                    okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                }, () => evt.Set());

                evt.WaitOne();
            }

            Assert.True(okay);
        }
#endif
#if HAS_DISPATCHER
        [TestMethod]
        [Asynchronous]
        public void ObserveOn_Dispatcher()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var okay = true;
                    Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(dispatcher).Subscribe(x =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    }, () =>
                    {
                        Assert.True(okay);
                        evt.Set();
                    });
                });
            }
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_DispatcherScheduler()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var okay = true;
                    Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(new DispatcherScheduler(dispatcher)).Subscribe(x =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    }, () =>
                    {
                        Assert.True(okay);
                        evt.Set();
                    });
                });
            }
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_CurrentDispatcher()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var okay = true;
                    dispatcher.BeginInvoke(new Action(() =>
                    {
                        Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOnDispatcher().Subscribe(x =>
                        {
                            okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        },  () =>
                        {
                            Assert.True(okay);
                            evt.Set();
                        });
                    }));
                });
            }
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_Error()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var ex = new Exception();
                    var okay = true;

                    dispatcher.BeginInvoke(new Action(() =>
                    {
                        Observable.Throw<int>(ex).ObserveOnDispatcher().Subscribe(x =>
                        {
                            okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        },
                        e =>
                        {
                            Assert.True(okay);
                            Assert.Same(ex, e);
                            evt.Set();
                        },
                        () =>
                        {
                            Assert.True(false);
                            evt.Set();
                        });
                    }));
                });
            }
        }
#endif
        #endregion + TestBase +

    }

    [TestClass]
    public class ObserveOnReactiveTest : ReactiveTest
    {
        private static readonly TimeSpan MaxWaitTime = TimeSpan.FromSeconds(10);

        [TestMethod]
        public void ObserveOn_Scheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn(default(IObservable<int>), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn(DummyObservable<int>.Instance, default(IScheduler)));
        }

        [TestMethod]
        public void ObserveOn_Scheduler_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnCompleted<int>(530)
            );

            var results = scheduler.Start(() =>
                xs.ObserveOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(231, 3),
                OnNext(241, 4),
                OnNext(311, 5),
                OnNext(471, 6),
                OnCompleted<int>(531)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 531)
            );
#else
            //
            // TODO: Check platform discrepancies
            //

            //xs.Subscriptions.AssertEqual(
            //    Subscribe(200, 1000)
            //);
#endif
        }

        [TestMethod]
        public void ObserveOn_Scheduler_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnError<int>(530, ex)
            );

            var results = scheduler.Start(() =>
                xs.ObserveOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(231, 3),
                OnNext(241, 4),
                OnNext(311, 5),
                OnNext(471, 6),
                OnError<int>(531, ex)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 531)
            );
#else
            //
            // TODO: Check platform discrepancies
            //
            //xs.Subscriptions.AssertEqual(
            //    Subscribe(200, 1000)
            //);
#endif
        }

        [TestMethod]
        public void ObserveOn_Scheduler_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6)
            );

            var results = scheduler.Start(() =>
                xs.ObserveOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(231, 3),
                OnNext(241, 4),
                OnNext(311, 5),
                OnNext(471, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ObserveOn_Scheduler_SameTime()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(210, 2)
            );

            var results = scheduler.Start(() =>
                xs.ObserveOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(211, 1),
                OnNext(212, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ObserveOn_Scheduler_OnNextThrows()
        {
            var e = new ManualResetEvent(false);

            var scheduler = new MyScheduler(e);

            Observable.Range(0, 10, Scheduler.Default).ObserveOn(scheduler).Subscribe(
                x =>
                {
                    if (x == 5)
                    {
                        throw new Exception();
                    }
                }
            );

            e.WaitOne();
            Assert.NotNull(scheduler._exception);
        }

        private class MyScheduler : IScheduler
        {
            internal Exception _exception;
            private readonly ManualResetEvent _evt;

            public MyScheduler(ManualResetEvent e)
            {
                _evt = e;
            }

            public DateTimeOffset Now
            {
                get { throw new NotImplementedException(); }
            }

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                try
                {
                    return action(this, state);
                }
                catch (Exception ex)
                {
                    _exception = ex;
                    _evt.Set();
                    return Disposable.Empty;
                }
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

#if !NO_PERF
        [TestMethod]
        public void ObserveOn_LongRunning_Simple()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e);

            var s = new Subject<int>();

            var end = new ManualResetEvent(false);
            var lst = new List<int>();
            s.ObserveOn(scheduler).Subscribe(lst.Add, () => end.Set());

            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            s.OnCompleted();

            end.WaitOne();

            Assert.True(lst.SequenceEqual([1, 2, 3]));
        }

        [TestMethod]
        public void ObserveOn_LongRunning_Error()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e);

            var s = new Subject<int>();

            var end = new ManualResetEvent(false);
            var err = default(Exception);
            s.ObserveOn(scheduler).Subscribe(_ => { }, ex => { err = ex; end.Set(); });

            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);

            var ex_ = new Exception();
            s.OnError(ex_);

            end.WaitOne();

            Assert.Same(ex_, err);
        }

        [TestMethod]
        public void ObserveOn_LongRunning_TimeVariance()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e);

            var s = new Subject<int>();

            var end = new ManualResetEvent(false);
            s.ObserveOn(scheduler).Subscribe(_ => { }, () => end.Set());

            s.OnNext(1); // Ensure active
            started.WaitOne();
            Thread.Sleep(100); // Try to enter the dispatcher event wait state

            for (var i = 0; i < 1000; i++)
            {
                if (i % 100 == 0)
                {
                    Thread.Sleep(10);
                }

                s.OnNext(i);
            }

            s.OnCompleted();

            end.WaitOne();
        }

        [TestMethod]
        public void ObserveOn_LongRunning_HoldUpDuringDispatchAndFail()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e);

            var s = new Subject<int>();

            var onNext = new ManualResetEvent(false);
            var resume = new ManualResetEvent(false);
            var lst = new List<int>();
            var err = default(Exception);
            var end = new ManualResetEvent(false);
            s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); onNext.Set(); resume.WaitOne(); }, ex_ => { err = ex_; end.Set(); });

            s.OnNext(1);
            onNext.WaitOne();

            s.OnNext(2);
            s.OnNext(3);

            var ex = new Exception();
            s.OnError(ex);

            resume.Set();

            end.WaitOne();
            Assert.True(lst.SequenceEqual([1, 2, 3]));
            Assert.Same(ex, err);
        }

        [TestMethod]
        public void ObserveOn_LongRunning_Cancel()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e);

            var s = new Subject<int>();

            var lst = new List<int>();
            var end = new ManualResetEvent(false);

            var running = new ManualResetEvent(false);
            var d = s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); running.Set(); });

            s.OnNext(0);
            started.WaitOne();

            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            running.WaitOne();

            d.Dispose();
            stopped.WaitOne();

            s.OnNext(4);

            Assert.True(lst.Count > 0 && !lst.Contains(4));
        }

        [TestMethod]
        public void ObserveOn_LongRunning_OnNextThrows()
        {
            var started = default(ManualResetEvent);
            var stopped = default(ManualResetEvent);
            var exception = default(Exception);

            var scheduler = new TestLongRunningScheduler(e => started = e, e => stopped = e, ex => exception = ex);

            var s = new Subject<int>();

            var lst = new List<int>();
            var end = new ManualResetEvent(false);

            var running = new ManualResetEvent(false);
            var d = s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); running.Set(); if (x == 3) { throw new Exception(); } });

            s.OnNext(0);
            started.WaitOne();

            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            running.WaitOne();

            s.OnNext(4);

            stopped.WaitOne();

            Assert.NotNull(exception);
        }
#endif

        [TestMethod]
        public void ObserveOn_SynchronizationContext_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(120, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnCompleted<int>(530)
            );

            var results = scheduler.Start(() =>
                xs.ObserveOn(new MyCtx(scheduler))
            );

            results.Messages.AssertEqual(
                OnNext(231, 3),
                OnNext(241, 4),
                OnNext(311, 5),
                OnNext(471, 6),
                OnCompleted<int>(531)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 531)
            );
        }

        [TestMethod]
        public void ObserveOn_EventLoop_Long()
        {
            using var _scheduler1 = new EventLoopScheduler();
            var N = 1_000_000;

            var cde = new CountdownEvent(1);

            Observable.Range(1, N).ObserveOn(_scheduler1)
                .Subscribe(v => { }, () => cde.Signal());

            Assert.True(cde.Wait(MaxWaitTime), "Timeout!");
        }

        [TestMethod]
        public void ObserveOn_LongRunning_SameThread()
        {
            var scheduler = TaskPoolScheduler.Default;

            Assert.NotNull(scheduler.AsLongRunning());

            var N = 1_000_000;
            var threads = new HashSet<long>();
            var cde = new CountdownEvent(1);

            Observable.Range(1, N)
                .ObserveOn(scheduler)
                .Subscribe(
                    v => threads.Add(Environment.CurrentManagedThreadId), 
                    e => cde.Signal(), 
                    () => cde.Signal()
                );

            Assert.True(cde.Wait(MaxWaitTime), "Timeout!");

            Assert.Equal(1, threads.Count);
        }

        [TestMethod]
        public void ObserveOn_LongRunning_DisableOptimizations()
        {
            var scheduler = TaskPoolScheduler.Default.DisableOptimizations();

            Assert.Null(scheduler.AsLongRunning());

            var N = 1_000_000;
            var threads = new HashSet<long>();
            var cde = new CountdownEvent(1);

            Observable.Range(1, N)
                .ObserveOn(scheduler)
                .Subscribe(
                    v => threads.Add(Environment.CurrentManagedThreadId),
                    e => cde.Signal(),
                    () => cde.Signal()
                );

            Assert.True(cde.Wait(MaxWaitTime), "Timeout!");

            Assert.True(threads.Count >= 1);
        }
    }

    internal class MyCtx : SynchronizationContext
    {
        private readonly IScheduler _scheduler;

        public MyCtx(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            _scheduler.Schedule(state, (self, s) => { d(s); return Disposable.Empty; });
        }
    }
}
