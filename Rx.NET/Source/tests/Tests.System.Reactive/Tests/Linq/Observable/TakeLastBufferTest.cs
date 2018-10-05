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
    public class TakeLastBufferTest : ReactiveTest
    {

        #region + Count +

        [Fact]
        public void TakeLastBuffer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(DummyObservable<int>.Instance, -1));
        }

        [Fact]
        public void TakeLastBuffer_Zero_Completed()
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
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.Count == 0),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_Zero_Error()
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
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_Zero_Disposed()
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
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLastBuffer_One_Completed()
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
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.SequenceEqual(new[] { 9 })),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_One_Error()
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
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_One_Disposed()
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
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLastBuffer_Three_Completed()
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
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.SequenceEqual(new[] { 7, 8, 9 })),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_Three_Error()
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
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [Fact]
        public void TakeLastBuffer_Three_Disposed()
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
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + Timed +

        [Fact]
        public void TakeLastBuffer_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [Fact]
        public void TakeLastBuffer_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.Count == 0),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLastBuffer_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.Count == 0),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLastBuffer_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(240, lst => lst.SequenceEqual(new[] { 2, 3 })),
                OnCompleted<IList<int>>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void TakeLastBuffer_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, lst => lst.Count == 0),
                OnCompleted<IList<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLastBuffer_Some3()
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
                xs.TakeLastBuffer(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, lst => lst.SequenceEqual(new[] { 6, 7, 8, 9 })),
                OnCompleted<IList<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeLastBuffer_Some4()
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
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(350, lst => lst.Count == 0),
                OnCompleted<IList<int>>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void TakeLastBuffer_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.SequenceEqual(new[] { 1, 2 })),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void TakeLastBuffer_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeLastBuffer_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TakeLastBuffer_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.FromSeconds(60)).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void TakeLastBuffer_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations()).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void TakeLastBuffer_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.Zero).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.Count == 0);
        }

        [Fact]
        public void TakeLastBuffer_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.Zero, Scheduler.Default.DisableOptimizations()).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.True(lst.Count == 0);
        }

        #endregion

    }
}
