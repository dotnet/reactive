// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableExtensionsTest : ReactiveTest
    {
        #region Subscribe

        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, (Exception _) => { }, default(Action)));
        }

        [TestMethod]
        public void Subscribe_None_Return()
        {
            Observable.Return(1, Scheduler.Immediate).Subscribe();
        }

        [TestMethod]
        public void Subscribe_None_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe());
        }

        [TestMethod]
        public void Subscribe_None_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); });
        }

        [TestMethod]
        public void Subscribe_OnNext_Return()
        {
            int _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; });
            Assert.AreEqual(42, _x);
        }

        [TestMethod]
        public void Subscribe_OnNext_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.Fail(); }));
        }

        [TestMethod]
        public void Subscribe_OnNext_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); });
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Return()
        {
            bool finished = false;
            int _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; }, () => { finished = true; });
            Assert.AreEqual(42, _x);
            Assert.IsTrue(finished);
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.Fail(); }, () => { Assert.Fail(); }));
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Empty()
        {
            bool finished = false;
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); }, () => { finished = true; });
            Assert.IsTrue(finished);
        }

        #endregion

        #region Subscribe with CancellationToken

#if !NO_TPL

        [TestMethod]
        public void Subscribe_CT_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var someObserver = Observer.Create<int>(_ => { });
            var ct = CancellationToken.None;

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), someObserver, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(IObserver<int>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>), () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, (Exception _) => { }, default(Action), ct));
        }

        [TestMethod]
        public void Subscribe_CT_None()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, CancellationToken.None));

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [TestMethod]
        public void Subscribe_CT_CancelBeforeBegin()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(150, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Subscribe_CT_CancelMiddle()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(225, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void Subscribe_CT_CancelAfterEnd()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(250, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [TestMethod]
        public void Subscribe_CT_NeverCancel()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [TestMethod]
        public void Subscribe_CT_Overloads_AlreadyCancelled()
        {
            var xs = Observable.Defer<int>(() =>
            {
                Assert.Fail();
                return Observable.Return(42, Scheduler.Immediate);
            });

            var cts = new CancellationTokenSource();
            cts.Cancel();

            xs.Subscribe(cts.Token);
            xs.Subscribe(_ => { }, cts.Token);
            xs.Subscribe(_ => { }, ex => { }, cts.Token);
            xs.Subscribe(_ => { }, () => { }, cts.Token);
            xs.Subscribe(_ => { }, ex => { }, () => { }, cts.Token);
            xs.Subscribe(Observer.Create<int>(_ => { }, ex => { }, () => { }), cts.Token);
        }

        [TestMethod]
        public void Subscribe_CT_Overloads_None()
        {
            var i = 0;
            var n = 0;
            var e = 0;
            var c = 0;

            var xs = Observable.Defer<int>(() =>
            {
                i++;
                return Observable.Return(42, Scheduler.Immediate);
            });

            xs.Subscribe(CancellationToken.None);
            xs.Subscribe(_ => { n++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, ex => { e++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, () => { c++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, ex => { e++; }, () => { c++; }, CancellationToken.None);
            xs.Subscribe(Observer.Create<int>(_ => { n++; }, ex => { e++; }, () => { c++; }), CancellationToken.None);

            Assert.AreEqual(6, i);
            Assert.AreEqual(5, n);
            Assert.AreEqual(0, e);
            Assert.AreEqual(3, c);
        }

        [TestMethod]
        public void Subscribe_CT_CancelDuringCallback()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var n = 0;

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(x =>
            {
                n++;

                if (x == 2)
                    cts.Cancel();
            }, cts.Token));

            scheduler.Start();

            Assert.AreEqual(2, n);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

#endif

        #endregion
    }
}
