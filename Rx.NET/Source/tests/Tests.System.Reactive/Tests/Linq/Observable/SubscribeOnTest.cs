// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if HAS_DISPATCHER
using System.Windows.Threading;
using System.Reactive;
using System.Reactive.Subjects;

#endif

#if HAS_WINFORMS
using System.Windows.Forms;
#endif

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class SubscribeOnTest : TestBase
    {

        #region + TestBase +

        [TestMethod]
        public void SubscribeOn_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

#if HAS_WINFORMS
#pragma warning disable IDE0034 // (Simplify 'default'.) Want to be explicit about overload being tested.
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new ControlScheduler(new Label())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(ControlScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(default(IObservable<int>), new Label()));
            ReactiveAssert.Throws<ArgumentNullException>(() => ControlObservable.SubscribeOn<int>(someObservable, default(Label)));
#pragma warning restore IDE0034
#endif
#if HAS_DISPATCHER
#pragma warning disable IDE0034 // (Simplify 'default'.) Want to be explicit about overload being tested.
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default(IObservable<int>), new DispatcherScheduler(Dispatcher.CurrentDispatcher)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(someObservable, default(DispatcherScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOn<int>(default(IObservable<int>), Dispatcher.CurrentDispatcher));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOn<int>(someObservable, default(Dispatcher)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DispatcherObservable.SubscribeOnDispatcher<int>(default(IObservable<int>)));
#pragma warning restore IDE0034
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn<int>(default, new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SubscribeOn(someObservable, default(SynchronizationContext)));
        }

#if HAS_WINFORMS
        [TestMethod]
        public void SubscribeOn_Control()
        {
            var okay = true;

            using (WinFormsTestUtils.RunTest(out var lbl))
            {
                var evt2 = new ManualResetEvent(false);
                var evt = new ManualResetEvent(false);
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
            }

            Assert.True(okay);
        }

        [TestMethod]
        public void SubscribeOn_ControlScheduler()
        {
            var okay = true;

            using (WinFormsTestUtils.RunTest(out var lbl))
            {
                var evt2 = new ManualResetEvent(false);
                var evt = new ManualResetEvent(false);
                
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
            }

            Assert.True(okay);
        }
#endif

#if HAS_DISPATCHER
        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_Dispatcher()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var s = new AsyncSubject<Unit>();
                    var okay = true;
                    var d = Observable.Create<int>(obs =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        s.OnNext(Unit.Default);
                        s.OnCompleted();

                        return () =>
                        {
                            okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                            Assert.True(okay);
                            evt.Set();
                        };
                    })
                    .SubscribeOn(dispatcher)
                    .Subscribe(_ => { });

                    s.Subscribe(_ => d.Dispose());
                });
            }
        }

        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_DispatcherScheduler()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var s = new AsyncSubject<Unit>();
                    var okay = true;
                    var d = Observable.Create<int>(obs =>
                    {
                        okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                        s.OnNext(Unit.Default);
                        s.OnCompleted();

                        return () =>
                        {
                            okay &= (SynchronizationContext.Current is System.Windows.Threading.DispatcherSynchronizationContext);
                            Assert.True(okay);
                            evt.Set();
                        };
                    })
                    .SubscribeOn(new DispatcherScheduler(dispatcher))
                    .Subscribe(_ => { });

                    s.Subscribe(_ => d.Dispose());
                });
            }
        }

        [TestMethod]
        [Asynchronous]
        public void SubscribeOn_CurrentDispatcher()
        {
            using (DispatcherHelpers.RunTest(out var dispatcher))
            {
                RunAsync(evt =>
                {
                    var s = new AsyncSubject<Unit>();
                    var okay = true;

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
                                evt.Set();
                            };
                        })
                        .SubscribeOnDispatcher()
                        .Subscribe(_ => { });

                        s.Subscribe(_ => d.Dispose());
                    }));
                });
            }
        }
#endif

        #endregion + TestBase +

    }

    [TestClass]
    public class SubscribeOnReactiveTest : ReactiveTest
    {

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

            Assert.Equal(201, s);
            Assert.Equal(1001, d);
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

    }
}
