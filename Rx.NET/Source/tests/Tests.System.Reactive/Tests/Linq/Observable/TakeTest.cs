// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class TakeTest : ReactiveTest
    {
        #region + Count +

        [Fact]
        public void Take_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Take(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Take(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Take(1).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Take(0, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Take(0, default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Take(-1, Scheduler.Immediate));
        }

        [Fact]
        public void Take_Complete_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(20)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Take_Complete_Same()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(17)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(630)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 630)
            );
        }

        [Fact]
        public void Take_Complete_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(690)
            );

            var res = scheduler.Start(() =>
                xs.Take(10)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnCompleted<int>(415)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 415)
            );
        }

        [Fact]
        public void Take_Error_After()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            var res = scheduler.Start(() =>
                xs.Take(20)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Take_Error_Same()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Take(17)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnCompleted<int>(630)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 630)
            );
        }

        [Fact]
        public void Take_Error_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10),
                OnError<int>(690, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Take(3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Take_Dispose_Before()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Take(3),
                250
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Take_Dispose_After()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnNext(410, 15),
                OnNext(415, 16),
                OnNext(460, 72),
                OnNext(510, 76),
                OnNext(560, 32),
                OnNext(570, -100),
                OnNext(580, -3),
                OnNext(590, 5),
                OnNext(630, 10)
            );

            var res = scheduler.Start(() =>
                xs.Take(3),
                400
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Take_0_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(0, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200 + 1) // Extra scheduling call by Empty
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Take_0_DefaultScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(0)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200) // Immediate
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Take_Non0_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13)
            );

            var res = scheduler.Start(() =>
                xs.Take(1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Take_Take1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Take(3).Take(4)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Take_Take2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 6),
                OnNext(150, 4),
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Take(4).Take(3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Take_DecrementsCountFirst()
        {
            var k = new BehaviorSubject<bool>(true);
            k.Take(1).Subscribe(b => k.OnNext(!b));

            //
            // No assert needed; test will stack overflow for failure.
            //
        }

        #endregion

        #region + Timed +

        [Fact]
        public void Take_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Take(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(xs, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Take(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [Fact]
        public void Take_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [Fact]
        public void Take_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(225)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Take_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Take_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Take_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Take_Twice1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(55), scheduler).Take(TimeSpan.FromTicks(35), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [Fact]
        public void Take_Twice2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(35), scheduler).Take(TimeSpan.FromTicks(55), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [Fact]
        public void Take_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.Take(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        #endregion

    }
}
