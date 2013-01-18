// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_AWAIT

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObservableAwaiterTest : ReactiveTest
    {
        [TestMethod]
        public void Await_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetAwaiter<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetAwaiter(Observable.Empty<int>()).OnCompleted(null));
        }

        [TestMethod]
        public void Await()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3),
                OnCompleted<int>(410)
            );

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; result = awaiter.GetResult(); }));

            scheduler.Start();

            Assert.AreEqual(410, t);
            Assert.AreEqual(3, result);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [TestMethod]
        public void Await_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3),
                OnError<int>(410, ex)
            );

            var awaiter = default(AsyncSubject<int>);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; ReactiveAssert.Throws(ex, () => awaiter.GetResult()); }));

            scheduler.Start();

            Assert.AreEqual(410, t);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [TestMethod]
        public void Await_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3)
            );

            var awaiter = default(AsyncSubject<int>);
            var hasValue = default(bool);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; awaiter.GetResult(); hasValue = true; }));

            scheduler.Start();

            Assert.AreEqual(long.MaxValue, t);
            Assert.IsFalse(hasValue);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [TestMethod]
        public void Await_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(300)
            );

            var awaiter = default(AsyncSubject<int>);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; ReactiveAssert.Throws<InvalidOperationException>(() => awaiter.GetResult()); }));

            scheduler.Start();

            Assert.AreEqual(300, t);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }
    }
}

#endif
