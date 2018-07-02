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
    #region + Count +

    public class SkipLastTest : ReactiveTest
    {

        [Fact]
        public void SkipLast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(DummyObservable<int>.Instance, -1));
        }

        [Fact]
        public void SkipLast_Zero_Completed()
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
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
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

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_Zero_Error()
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
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
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

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_Zero_Disposed()
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
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void SkipLast_One_Completed()
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
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_One_Error()
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
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8),
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_One_Disposed()
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
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void SkipLast_Three_Completed()
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
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_Three_Error()
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
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6),
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void SkipLast_Three_Disposed()
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
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + Timed +

        [Fact]
        public void SkipLast_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [Fact]
        public void SkipLast_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.Zero, scheduler)
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
        public void SkipLast_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void SkipLast_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void SkipLast_Some2()
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
                xs.SkipLast(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(260, 1),
                OnNext(270, 2),
                OnNext(280, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SkipLast_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void SkipLast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SkipLast_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void SkipLast_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.FromSeconds(60));

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
        public void SkipLast_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations());

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
        public void SkipLast_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.Zero);

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
        public void SkipLast_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.Zero, Scheduler.Default.DisableOptimizations());

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
