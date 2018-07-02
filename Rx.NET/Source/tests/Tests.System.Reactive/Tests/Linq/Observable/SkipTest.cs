// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SkipTest : ReactiveTest
    {

        #region + Count +

        [Fact]
        public void Skip_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Skip(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Skip(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Skip(0).Subscribe(null));
        }

        [Fact]
        public void Skip_Complete_After()
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
                xs.Skip(20)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Skip_Complete_Same()
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
                xs.Skip(17)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(690)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Skip_Complete_Before()
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
                xs.Skip(10)
            );

            res.Messages.AssertEqual(
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
        public void Skip_Complete_Zero()
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
                xs.Skip(0)
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
        public void Skip_Error_After()
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
                xs.Skip(20)
            );

            res.Messages.AssertEqual(
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Skip_Error_Same()
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
                xs.Skip(17)
            );

            res.Messages.AssertEqual(
                OnError<int>(690, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 690)
            );
        }

        [Fact]
        public void Skip_Error_Before()
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
                xs.Skip(3)
            );

            res.Messages.AssertEqual(
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
        public void Skip_Dispose_Before()
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
                xs.Skip(3),
                250
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Skip_Dispose_After()
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
                xs.Skip(3),
                400
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(300, -1),
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Skip_Skip1()
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
                xs.Skip(3).Skip(2)
            );

            res.Messages.AssertEqual(
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion

        #region + Timed +

        [Fact]
        public void Skip_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [Fact]
        public void Skip_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.Zero, scheduler)
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
        public void Skip_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Skip_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Skip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Skip_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Skip_Twice1()
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
                xs.Skip(TimeSpan.FromTicks(15), scheduler).Skip(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Skip_Twice2()
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
                xs.Skip(TimeSpan.FromTicks(30), scheduler).Skip(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void Skip_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.Skip(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.Count == 0);
        }

        #endregion

    }
}
