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
    public class TakeLastTest : ReactiveTest
    {

        #region + Count +

        [Fact]
        public void TakeLast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(DummyObservable<int>.Instance, -1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast<int>(null, 0, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(DummyObservable<int>.Instance, -1, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(DummyObservable<int>.Instance, 0, default));
        }

        [Fact]
        public void TakeLast_Zero_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_Zero_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_Zero_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLast_One_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(650, 9),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_One_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_One_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLast_Three_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(650, 7),
                OnNext(650, 8),
                OnNext(650, 9),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_Three_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLast_Three_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLast_LongRunning_Regular()
        {
            var res = Observable.Range(0, 100, Scheduler.Default).TakeLast(10, NewThreadScheduler.Default);

            var lst = new List<int>();
            res.ForEach(lst.Add);

            Assert.True(Enumerable.Range(90, 10).SequenceEqual(lst));
        }

        #endregion

        #region + Timed +

        [Fact]
        public void TakeLast_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), Scheduler.Default, default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default, Scheduler.Default));
        }

        [Fact]
        public void TakeLast_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_Zero1_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(231)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_Zero2_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(231)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 2),
                OnNext(240, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void TakeLast_Some1_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(241, 2),
                OnNext(242, 3),
                OnCompleted<int>(243)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void TakeLast_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLast_Some2_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(301)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLast_Some3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(300, 6),
                OnNext(300, 7),
                OnNext(300, 8),
                OnNext(300, 9),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLast_Some3_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(45), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(301, 6),
                OnNext(302, 7),
                OnNext(303, 8),
                OnNext(304, 9),
                OnCompleted<int>(305)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLast_Some4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void TakeLast_Some4_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(351)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void TakeLast_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(230, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_All_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(231, 1),
                OnNext(232, 2),
                OnCompleted<int>(233)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeLast_Error_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeLast_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLast_Never_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLast_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void TakeLast_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void TakeLast_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.Zero);

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.Count == 0);
        }

        [Fact]
        public void TakeLast_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.Zero, Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.Count == 0);
        }

        [Fact]
        public void TakeLast_Timed_LongRunning_Regular()
        {
            var res = Observable.Range(0, 10, Scheduler.Default).TakeLast(TimeSpan.FromSeconds(60), Scheduler.Default, NewThreadScheduler.Default);

            var lst = new List<int>();
            res.ForEach(lst.Add);

            Assert.True(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        #endregion

    }
}
