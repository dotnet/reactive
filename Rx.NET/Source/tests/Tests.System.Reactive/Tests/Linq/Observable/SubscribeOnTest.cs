// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

#if HAS_DISPATCHER
using System.Windows.Threading;
using System.Reactive;
using System.Reactive.Subjects;

#endif

#if HAS_WINFORMS
using System.Windows.Forms;
#endif


namespace ReactiveTests.Tests
{
    public class SubscribeOnTest : TestBase
    {

        #region + TestBase +

        [Fact]
        public void SubscribeOn_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

#if HAS_WINFORMS
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new ControlScheduler(new Label())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(ControlScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(default(IObservable<int>), new Label()));
            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(someObservable, default(Label)));
#endif
#if HAS_DISPATCHER
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
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOnDispatcher<int>(default(IObservable<int>)));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default, new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(someObservable, default(SynchronizationContext)));
        }

#if HAS_WINFORMS
        [Fact]
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
            Assert.True(okay);
        }

        [Fact]
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
            Assert.True(okay);
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

#if HAS_DISPATCHER
        [Fact]
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
                        Assert.True(okay);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    };
                })
                .SubscribeOn(dispatcher)
                .Subscribe(_ => { });

                s.Subscribe(_ => d.Dispose());
            });
        }

        [Fact]
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
                        Assert.True(okay);
                        dispatcher.InvokeShutdown();
                        evt.Set();
                    };
                })
                .SubscribeOn(new DispatcherScheduler(dispatcher))
                .Subscribe(_ => { });

                s.Subscribe(_ => d.Dispose());
            });
        }

        [Fact]
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
                            Assert.True(okay);
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
#endif

        #endregion + TestBase +

    }

    public class SubscribeOnReactiveTest : ReactiveTest
    {

        [Fact]
        public void SubscribeOn_Scheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(default(IObservable<int>), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(DummyObservable<int>.Instance, default(IScheduler)));
        }

        [Fact]
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

            Assert.Equal(201, s);
            Assert.Equal(1001, d);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

    }
}
