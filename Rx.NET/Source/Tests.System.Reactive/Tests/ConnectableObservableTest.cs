// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ConnectableObservableTest : ReactiveTest
    {
        [TestMethod]
        public void ConnectableObservable_Creation()
        {
            var y = 0;

            var s2 = new Subject<int>();
            var co2 = new ConnectableObservable<int>(Observable.Return<int>(1), s2);

            co2.Subscribe(x => y = x);
            Assert.AreNotEqual(1, y);
            
            co2.Connect();
            Assert.AreEqual(1, y);
        }

        [TestMethod]
        public void ConnectableObservable_Connected()
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
            var disconnect = conn.Connect();

            var res = scheduler.Start(() => conn);

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ConnectableObservable_NotConnected()
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

            var res = scheduler.Start(() => conn);

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void ConnectableObservable_Disconnected()
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
            var disconnect = conn.Connect();
            disconnect.Dispose();

            var res = scheduler.Start(() => conn);

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void ConnectableObservable_DisconnectFuture()
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
            subject.DisposeOn(3, conn.Connect());

            var res = scheduler.Start(() => conn);

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3)
            );
        }

        [TestMethod]
        public void ConnectableObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Publish().Subscribe(default(IObserver<int>)));
        }

        [TestMethod]
        public void ConnectableObservable_MultipleNonOverlappedConnections()
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

            var subject = new Subject<int>();

            var conn = xs.Multicast(subject);

            var c1 = default(IDisposable);
            scheduler.ScheduleAbsolute(225, () => { c1 = conn.Connect(); });
            scheduler.ScheduleAbsolute(241, () => { c1.Dispose(); });
            scheduler.ScheduleAbsolute(245, () => { c1.Dispose(); }); // idempotency test
            scheduler.ScheduleAbsolute(251, () => { c1.Dispose(); }); // idempotency test
            scheduler.ScheduleAbsolute(260, () => { c1.Dispose(); }); // idempotency test

            var c2 = default(IDisposable);
            scheduler.ScheduleAbsolute(249, () => { c2 = conn.Connect(); });
            scheduler.ScheduleAbsolute(255, () => { c2.Dispose(); });
            scheduler.ScheduleAbsolute(265, () => { c2.Dispose(); }); // idempotency test
            scheduler.ScheduleAbsolute(280, () => { c2.Dispose(); }); // idempotency test

            var c3 = default(IDisposable);
            scheduler.ScheduleAbsolute(275, () => { c3 = conn.Connect(); });
            scheduler.ScheduleAbsolute(295, () => { c3.Dispose(); });

            var res = scheduler.Start(() => conn);

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(280, 8),
                OnNext(290, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(225, 241),
                Subscribe(249, 255),
                Subscribe(275, 295)
            );
        }
    }
}
