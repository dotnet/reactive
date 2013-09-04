// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

#if HAS_WINFORMS
using System.Windows.Forms;
#endif

#if SILVERLIGHT && !SILVERLIGHTM7
using Microsoft.Silverlight.Testing;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableConcurrencyTest : TestBase
    {
        #region + ObserveOn +

        [TestMethod]
        public void ObserveOn_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

#if HAS_WINFORMS
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new ControlScheduler(new Label())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(someObservable, default(ControlScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.ObserveOn<int>(default(IObservable<int>), new Label()));
            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.ObserveOn<int>(someObservable, default(Label)));
#endif

#if USE_SL_DISPATCHER
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new DispatcherScheduler(System.Windows.Deployment.Current.Dispatcher)));
#else
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new DispatcherScheduler(Dispatcher.CurrentDispatcher)));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(someObservable, default(DispatcherScheduler)));

#if USE_SL_DISPATCHER
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOn<int>(default(IObservable<int>), System.Windows.Deployment.Current.Dispatcher));
#else
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOn<int>(default(IObservable<int>), Dispatcher.CurrentDispatcher));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOn<int>(someObservable, default(Dispatcher)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(default(IObservable<int>), new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ObserveOn<int>(someObservable, default(SynchronizationContext)));

            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.ObserveOnDispatcher<int>(default(IObservable<int>)));
        }

#if HAS_WINFORMS
        [TestMethod]
        public void ObserveOn_Control()
        {
            var lbl = CreateLabel();

            var evt = new ManualResetEvent(false);
            bool okay = true;
            Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(lbl).Subscribe(x =>
            {
                lbl.Text = x.ToString();
                okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
            }, () => evt.Set());

            evt.WaitOne();
            Application.Exit();
            Assert.IsTrue(okay);
        }

        [TestMethod]
        public void ObserveOn_ControlScheduler()
        {
            var lbl = CreateLabel();

            var evt = new ManualResetEvent(false);
            bool okay = true;
            Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(new ControlScheduler(lbl)).Subscribe(x =>
            {
                lbl.Text = x.ToString();
                okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
            }, () => evt.Set());

            evt.WaitOne();
            Application.Exit();
            Assert.IsTrue(okay);
        }

        private Label CreateLabel()
        {
            var loaded = new ManualResetEvent(false);
            var lbl = default(Label);

            var t = new Thread(() =>
            {
                lbl = new Label();
                var frm = new Form { Controls = { lbl }, Width = 0, Height = 0, FormBorderStyle = FormBorderStyle.None, ShowInTaskbar = false };
                frm.Load += (_, __) =>
                {
                    loaded.Set();
                };
                Application.Run(frm);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            loaded.WaitOne();
            return lbl;
        }
#endif

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_Dispatcher()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                bool okay = true;
                Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(dispatcher).Subscribe(x =>
                {
                    okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                }, () =>
                {
                    Assert.IsTrue(okay);
                    dispatcher.InvokeShutdown();
                    evt.Set();
                });
            });
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_DispatcherScheduler()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                bool okay = true;
                Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOn(new DispatcherScheduler(dispatcher)).Subscribe(x =>
                {
                    okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                }, () =>
                {
                    Assert.IsTrue(okay);
                    dispatcher.InvokeShutdown();
                    evt.Set();
                });
            });
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_CurrentDispatcher()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                bool okay = true;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    Observable.Range(0, 10, NewThreadScheduler.Default).ObserveOnDispatcher().Subscribe(x =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    },  () =>
                    {
                        Assert.IsTrue(okay);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    });
                }));
            });
        }

        [TestMethod]
        [Asynchronous]
        public void ObserveOn_Error()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                var ex = new Exception();
                bool okay = true;

                dispatcher.BeginInvoke(new Action(() =>
                {
                    Observable.Throw<int>(ex).ObserveOnDispatcher().Subscribe(x =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    },
                    e =>
                    {
                        Assert.IsTrue(okay);
                        Assert.AreSame(ex, e);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    },
                    () =>
                    {
                        Assert.Fail();
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    });
                }));
            });
        }

        #endregion

        #region SubscribeOn

        [TestMethod]
        public void SubscribeOn_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

#if HAS_WINFORMS
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new ControlScheduler(new Label())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(ControlScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(default(IObservable<int>), new Label()));
            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(someObservable, default(Label)));
#endif

#if USE_SL_DISPATCHER
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new DispatcherScheduler(System.Windows.Deployment.Current.Dispatcher)));
#else
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new DispatcherScheduler(Dispatcher.CurrentDispatcher)));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(DispatcherScheduler)));

#if USE_SL_DISPATCHER
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOn<int>(default(IObservable<int>), System.Windows.Deployment.Current.Dispatcher));
#else
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOn<int>(default(IObservable<int>), Dispatcher.CurrentDispatcher));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOn<int>(someObservable, default(Dispatcher)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(SynchronizationContext)));

            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOnDispatcher<int>(default(IObservable<int>)));
        }

#if HAS_WINFORMS
        [TestMethod]
        public void SubscribeOn_Control()
        {
            var lbl = CreateLabel();

            var evt2 = new ManualResetEvent(false);
            var evt = new ManualResetEvent(false);
            bool okay = true;
            var d = Observable.Create<int>(obs =>
            {
                lbl.Text = "Subscribe";
                okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                evt2.Set();

                return () =>
                {
                    lbl.Text = "Unsubscribe";
                    okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                    evt.Set();
                };
            })
            .SubscribeOn(lbl)
            .Subscribe(_ => {});

            evt2.WaitOne();
            d.Dispose();

            evt.WaitOne();
            Application.Exit();
            Assert.IsTrue(okay);
        }

        [TestMethod]
        public void SubscribeOn_ControlScheduler()
        {
            var lbl = CreateLabel();

            var evt2 = new ManualResetEvent(false);
            var evt = new ManualResetEvent(false);
            bool okay = true;
            var d = Observable.Create<int>(obs =>
            {
                lbl.Text = "Subscribe";
                okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                evt2.Set();

                return () =>
                {
                    lbl.Text = "Unsubscribe";
                    okay &= (SynchronizationContext.Current is System.Windows.Forms.WindowsFormsSynchronizationContext);
                    evt.Set();
                };
            })
            .SubscribeOn(new ControlScheduler(lbl))
            .Subscribe(_ => { });

            evt2.WaitOne();
            d.Dispose();

            evt.WaitOne();
            Application.Exit();
            Assert.IsTrue(okay);
        }
#endif

        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_Dispatcher()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                var s = new AsyncSubject<Unit>();
                bool okay = true;
                var d = Observable.Create<int>(obs =>
                {
                    okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    s.OnNext(Unit.Default);
                    s.OnCompleted();

                    return () =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        Assert.IsTrue(okay);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    };
                })
                .SubscribeOn(dispatcher)
                .Subscribe(_ => { });

                s.Subscribe(_ => d.Dispose());
            });
        }

        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_DispatcherScheduler()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                var s = new AsyncSubject<Unit>();
                bool okay = true;
                var d = Observable.Create<int>(obs =>
                {
                    okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                    s.OnNext(Unit.Default);
                    s.OnCompleted();

                    return () =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        Assert.IsTrue(okay);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    };
                })
                .SubscribeOn(new DispatcherScheduler(dispatcher))
                .Subscribe(_ => { });

                s.Subscribe(_ => d.Dispose());
            });
        }

        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_CurrentDispatcher()
        {
            var dispatcher = DispatcherHelpers.EnsureDispatcher();

            RunAsync(evt =>
            {
                var s = new AsyncSubject<Unit>();
                bool okay = true;

                dispatcher.BeginInvoke(new Action(() =>
                {
                    var d = Observable.Create<int>(obs =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        s.OnNext(Unit.Default);
                        s.OnCompleted();

                        return () =>
                        {
                            okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                            Assert.IsTrue(okay);
                            dispatcher.InvokeShutdown();
                            evt.Set();
                        };
                    })
                    .SubscribeOnDispatcher()
                    .Subscribe(_ => { });

                    s.Subscribe(_ => d.Dispose());
                }));
            });
        }

        #endregion

        #region + Synchronize +

        [TestMethod]
        public void Synchronize_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(default(IObservable<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Synchronize<int>(someObservable, null));
        }

        [TestMethod]
        public void Synchronize_Range()
        {
            int i = 0;
            bool outsideLock = true;

            var gate = new object();
            lock (gate)
            {
                outsideLock = false;
                Observable.Range(0, 100, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => i++, () => { Assert.IsTrue(outsideLock); });
                Thread.Sleep(100);
                Assert.AreEqual(0, i);
                outsideLock = true;
            }

            while (i < 100)
            {
                Thread.Sleep(10);
                lock (gate)
                {
                    int start = i;
                    Thread.Sleep(100);
                    Assert.AreEqual(start, i);
                }
            }
        }

        [TestMethod]
        public void Synchronize_Throw()
        {
            var ex = new Exception();
            var resLock = new object();
            var e = default(Exception);
            bool outsideLock = true;

            var gate = new object();
            lock (gate)
            {
                outsideLock = false;
                Observable.Throw<int>(ex, NewThreadScheduler.Default).Synchronize(gate).Subscribe(x => { Assert.Fail(); }, err => { lock (resLock) { e = err; } }, () => { Assert.IsTrue(outsideLock); });
                Thread.Sleep(100);
                Assert.IsNull(e);
                outsideLock = true;
            }

            while (true)
            {
                lock (resLock)
                {
                    if (e != null)
                        break;
                }
            }

            Assert.AreSame(ex, e);
        }

        [TestMethod]
        public void Synchronize_BadObservable()
        {
            var o = Observable.Create<int>(obs =>
            {
                var t1 = new Thread(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        obs.OnNext(i);
                    }
                });

                new Thread(() =>
                {
                    t1.Start();

                    for (int i = 100; i < 200; i++)
                    {
                        obs.OnNext(i);
                    }

                    t1.Join();
                    obs.OnCompleted();
                }).Start();

                return () => { };
            });

            var evt = new ManualResetEvent(false);

            int sum = 0;
            o.Synchronize().Subscribe(x => sum += x, () => { evt.Set(); });

            evt.WaitOne();

            Assert.AreEqual(Enumerable.Range(0, 200).Sum(), sum);
        }

        #endregion
    }

    [TestClass]
    public class ObservableConcurrencyReactiveTest : ReactiveTest
    {
        #region + ObserveOn +

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
                OnNext( 90, 1),
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

#if !NO_PERF && !NO_CDS
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

#if !NO_PERF && !NO_CDS
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
                        throw new Exception();
                }
            );

            e.WaitOne();
            Assert.IsNotNull(scheduler._exception);
        }

        class MyScheduler : IScheduler
        {
            internal Exception _exception;
            private ManualResetEvent _evt;

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

#if !NO_PERF && !NO_CDS
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

            Assert.IsTrue(lst.SequenceEqual(new[] { 1, 2, 3 }));
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

            Assert.AreSame(ex_, err);
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

            for (int i = 0; i < 1000; i++)
            {
                if (i % 100 == 0)
                    Thread.Sleep(10);

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
            Assert.IsTrue(lst.SequenceEqual(new[] { 1, 2, 3 }));
            Assert.AreSame(ex, err);
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

            Assert.IsTrue(lst.Count > 0 && !lst.Contains(4));
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
            var d = s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); running.Set(); if (x == 3) throw new Exception(); });

            s.OnNext(0);
            started.WaitOne();

            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            running.WaitOne();

            s.OnNext(4);

            stopped.WaitOne();

            Assert.IsNotNull(exception);
        }
#endif

#if !NO_SYNCCTX
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
#endif

        #endregion

        #region SubscribeOn

        [TestMethod]
        public void SubscribeOn_Scheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(default(IObservable<int>), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(DummyObservable<int>.Instance, default(IScheduler)));
        }

        [TestMethod]
        public void SubscribeOn_Scheduler_Sleep()
        {
            var scheduler = new TestScheduler();

            var s = 0L;
            var d = 0L;

            var xs = Observable.Create<long>(observer =>
                {
                    s = scheduler.Clock;
                    return () => d = scheduler.Clock;
                });

            var results = scheduler.Start(() =>
                xs.SubscribeOn(scheduler)
            );

            results.Messages.AssertEqual(
            );

            Assert.AreEqual(201, s);
            Assert.AreEqual(1001, d);
        }

        [TestMethod]
        public void SubscribeOn_Scheduler_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<long>(300)
            );

            var results = scheduler.Start(() =>
                xs.SubscribeOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnCompleted<long>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 301)
            );
        }

        [TestMethod]
        public void SubscribeOn_Scheduler_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(300, ex)
            );

            var results = scheduler.Start(() =>
                xs.SubscribeOn(scheduler)
            );

            results.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 301)
            );
        }

        [TestMethod]
        public void SubscribeOn_Scheduler_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var results = scheduler.Start(() =>
                xs.SubscribeOn(scheduler)
            );

            results.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 1001)
            );
        }

#if !NO_SYNCCTX
        [TestMethod]
        public void SubscribeOn_SynchronizationContext_Simple()
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
                xs.SubscribeOn(new MyCtx(scheduler))
            );

            results.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(310, 5),
                OnNext(470, 6),
                OnCompleted<int>(530)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 531)
            );
        }
#endif

        #endregion

        #region |> Helpers <|

#if !NO_SYNCCTX
        class MyCtx : SynchronizationContext
        {
            private IScheduler scheduler;

            public MyCtx(IScheduler scheduler)
            {
                this.scheduler = scheduler;
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                scheduler.Schedule(state, (self, s) => { d(s); return Disposable.Empty; });
            }
        }
#endif

        #endregion
    }
}
