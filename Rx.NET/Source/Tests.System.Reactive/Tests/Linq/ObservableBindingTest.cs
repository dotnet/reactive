// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableBindingTest : ReactiveTest
    {
        #region Multicast

        [TestMethod]
        public void Multicast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int>(null, new Subject<int>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int, int>(null, () => new Subject<int>(), xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int, int>(DummyObservable<int>.Instance, null, xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int, int>(DummyObservable<int>.Instance, () => new Subject<int>(), null));
        }

        [TestMethod]
        public void Multicast_Hot_1()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(200, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(300, () => d1.Dispose());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Multicast_Hot_2()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d1.Dispose());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [TestMethod]
        public void Multicast_Hot_3()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d2.Dispose());
            scheduler.ScheduleAbsolute(335, () => d2 = c.Connect());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 300),
                Subscribe(335, 390)
            );
        }

        [TestMethod]
        public void Multicast_Hot_4()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d2.Dispose());
            scheduler.ScheduleAbsolute(335, () => d2 = c.Connect());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 300),
                Subscribe(335, 390)
            );
        }

        [TestMethod]
        public void Multicast_Hot_5()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(400, () => d1 = c.Subscribe(o));

            scheduler.Start();

            o.Messages.AssertEqual(
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [TestMethod]
        public void Multicast_Hot_6()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(400, () => d1 = c.Subscribe(o));

            scheduler.Start();

            o.Messages.AssertEqual(
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [TestMethod]
        public void Multicast_Cold_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Multicast_Cold_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Multicast_Cold_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Multicast_Cold_Zip()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys.Zip(ys, (a, b) => a + b))
            );

            res.Messages.AssertEqual(
                OnNext(210, 6),
                OnNext(240, 8),
                OnNext(270, 10),
                OnNext(330, 12),
                OnNext(340, 14),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Multicast_SubjectSelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Multicast<int, int, int>(() => { throw ex; }, _ => _)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Multicast_SelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Multicast<int, int, int>(() => new Subject<int>(), _ => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region Publish

        [TestMethod]
        public void Publish_Cold_Zip()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Publish(ys => ys.Zip(ys, (a, b) => a + b))
            );

            res.Messages.AssertEqual(
                OnNext(210, 6),
                OnNext(240, 8),
                OnNext(270, 10),
                OnNext(330, 12),
                OnNext(340, 14),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Publish_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish<int, int>(someObservable, null));
        }

        [TestMethod]
        public void Publish_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void Publish_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void Publish_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void Publish_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(350, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void Publish_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Publish();

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.AreSame(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.AreNotSame(connection1, connection3);

            connection3.Dispose();
        }

        [TestMethod]
        public void PublishLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev))
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev))
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev)),
                470
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [TestMethod]
        public void PublishWithInitialValue_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>), x => x, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(someObservable, default(Func<IObservable<int>, IObservable<int>>), 1));
        }

        [TestMethod]
        public void PublishWithInitialValue_SanityCheck()
        {
            var someObservable = Observable.Empty<int>();

            Observable.Publish(Observable.Range(1, 10), x => x, 0).AssertEqual(Observable.Range(0, 11));
        }

        [TestMethod]
        public void PublishWithInitialValue_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void PublishWithInitialValue_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void PublishWithInitialValue_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void PublishWithInitialValue_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(350, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void PublishWithInitialValue_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Publish(1979);

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.AreSame(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.AreNotSame(connection1, connection3);

            connection3.Dispose();
        }

        [TestMethod]
        public void PublishWithInitialValueLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979)
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishWithInitialValueLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979)
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishWithInitialValueLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979),
                470
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        #endregion

        #region PublishLast

        [TestMethod]
        public void PublishLast_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var scheduler = new TestScheduler();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast<int, int>(someObservable, null));
        }

        [TestMethod]
        public void PublishLast_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void PublishLast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void PublishLast_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(600, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void PublishLast_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(350, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void PublishLast_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.PublishLast();

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.AreSame(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.AreNotSame(connection1, connection3);

            connection3.Dispose();
        }

        [TestMethod]
        public void PublishLastLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y))
            );

            res.Messages.AssertEqual(
                OnNext(600, 40),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishLastLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y))
            );

            res.Messages.AssertEqual(
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void PublishLastLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y)),
                470
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        #endregion

        #region RefCount

        [TestMethod]
        public void RefCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null));
        }

        [TestMethod]
        public void RefCount_ConnectsOnFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);

            var res = scheduler.Start(() =>
                conn.RefCount()
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            Assert.IsTrue(subject.Disposed);
        }

        [TestMethod]
        public void RefCount_NotConnected()
        {
            var disconnected = false;
            var count = 0;
            var xs = Observable.Defer(() =>
            {
                count++;
                return Observable.Create<int>(obs =>
                {
                    return () => { disconnected = true; };
                });
            });

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);
            var refd = conn.RefCount();

            var dis1 = refd.Subscribe();
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, subject.SubscribeCount);
            Assert.IsFalse(disconnected);

            var dis2 = refd.Subscribe();
            Assert.AreEqual(1, count);
            Assert.AreEqual(2, subject.SubscribeCount);
            Assert.IsFalse(disconnected);

            dis1.Dispose();
            Assert.IsFalse(disconnected);
            dis2.Dispose();
            Assert.IsTrue(disconnected);
            disconnected = false;

            var dis3 = refd.Subscribe();
            Assert.AreEqual(2, count);
            Assert.AreEqual(3, subject.SubscribeCount);
            Assert.IsFalse(disconnected);

            dis3.Dispose();
            Assert.IsTrue(disconnected);
        }

        [TestMethod]
        public void RefCount_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount();

            res.Subscribe(_ => { Assert.Fail(); }, ex_ => { Assert.AreSame(ex, ex_); }, () => { Assert.Fail(); });
            res.Subscribe(_ => { Assert.Fail(); }, ex_ => { Assert.AreSame(ex, ex_); }, () => { Assert.Fail(); });
        }

        [TestMethod]
        public void RefCount_Publish()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = xs.Publish().RefCount();

            var d1 = default(IDisposable);
            var o1 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(215, () => { d1 = res.Subscribe(o1); });
            scheduler.ScheduleAbsolute(235, () => { d1.Dispose(); });

            var d2 = default(IDisposable);
            var o2 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(225, () => { d2 = res.Subscribe(o2); });
            scheduler.ScheduleAbsolute(275, () => { d2.Dispose(); });

            var d3 = default(IDisposable);
            var o3 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(255, () => { d3 = res.Subscribe(o3); });
            scheduler.ScheduleAbsolute(265, () => { d3.Dispose(); });

            var d4 = default(IDisposable);
            var o4 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(285, () => { d4 = res.Subscribe(o4); });
            scheduler.ScheduleAbsolute(320, () => { d4.Dispose(); });

            scheduler.Start();

            o1.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(230, 3)
            );

            o2.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7)
            );

            o3.Messages.AssertEqual(
                OnNext(260, 6)
            );

            o4.Messages.AssertEqual(
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(215, 275),
                Subscribe(285, 300)
            );
        }

        #endregion

        #region Replay

        [TestMethod]
        public void Replay_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var scheduler = new TestScheduler();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int>(null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int>(DummyObservable<int>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(null, DummyFunc<IObservable<int>, IObservable<int>>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(DummyObservable<int>.Instance, DummyFunc<IObservable<int>, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay<int, int>(someObservable, x => x, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, 1, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, -2, scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, 1, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(1), null));
        }

        [TestMethod]
        public void ReplayCount_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void ReplayCount_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void ReplayCount_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnCompleted<int>(601)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void ReplayCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(475, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void ReplayCount_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Replay(3, new TestScheduler());

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.AreSame(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.AreNotSame(connection1, connection3);

            connection3.Dispose();
        }

        [TestMethod]
        public void ReplayCountLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler),
                610
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 9),
                OnNext(563, 11),
                OnNext(564, 20),
                OnNext(602, 9),
                OnNext(603, 11),
                OnNext(604, 20),
                OnNext(606, 9),
                OnNext(607, 11),
                OnNext(608, 20)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void ReplayCountLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 9),
                OnNext(563, 11),
                OnNext(564, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void ReplayCountLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler),
                470
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [TestMethod]
        public void ReplayTime_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(150), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 8),
                OnNext(452, 5),
                OnNext(453, 6),
                OnNext(454, 7),
                OnNext(521, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void ReplayTime_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(75), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void ReplayTime_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(85), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 6),
                OnNext(452, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnCompleted<int>(601)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [TestMethod]
        public void ReplayTime_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(100), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(475, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [TestMethod]
        public void ReplayTime_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Replay(TimeSpan.FromTicks(100), new TestScheduler());

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.AreSame(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.AreNotSame(connection1, connection3);

            connection3.Dispose();
        }

        [TestMethod]
        public void ReplayTimeLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler),
                610
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 11),
                OnNext(563, 20),
                OnNext(602, 20),
                OnNext(604, 20),
                OnNext(606, 20),
                OnNext(608, 20)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void ReplayTimeLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 11),
                OnNext(563, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void ReplayTimeLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler),
                470
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [TestMethod]
        public void Replay_Default1()
        {
            var s = new Subject<int>();
            var xs = s.Replay(100, DefaultScheduler.Instance);
            var ys = s.Replay(100);

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void Replay_Default2()
        {
            var s = new Subject<int>();
            var xs = s.Replay(TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = s.Replay(TimeSpan.FromHours(1));

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void Replay_Default3()
        {
            var s = new Subject<int>();
            var xs = s.Replay(100, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = s.Replay(100, TimeSpan.FromHours(1));

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void Replay_Default4()
        {
            var s = new Subject<int>();
            var xs = s.Replay(DefaultScheduler.Instance);
            var ys = s.Replay();

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void ReplayLambda_Default1()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, 100, DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, 100);

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void ReplayLambda_Default2()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, TimeSpan.FromHours(1));

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void ReplayLambda_Default3()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, 100, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, 100, TimeSpan.FromHours(1));

            xs.AssertEqual(ys);
        }

        [TestMethod]
        public void ReplayLambda_Default4()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs);

            xs.AssertEqual(ys);
        }

        #endregion
    }
}
