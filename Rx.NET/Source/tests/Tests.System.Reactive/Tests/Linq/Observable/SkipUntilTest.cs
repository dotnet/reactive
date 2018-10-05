// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SkipUntilTest : ReactiveTest
    {
        #region + Observable +
        [Fact]
        public void SkipUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil<int, int>(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil<int, int>(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void SkipUntil_SomeData_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4), //!
                OnNext(240, 5), //!
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 99),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_SomeData_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_Error_SomeData()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void SkipUntil_SomeData_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_Never_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 2), //!
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_Never_Error1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_SomeData_Error2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SkipUntil_SomeData_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
            );
        }

        [Fact]
        public void SkipUntil_Never_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void SkipUntil_Never_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void SkipUntil_HasCompletedCausesDisposal()
        {
            var scheduler = new TestScheduler();

            var disposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = Observable.Create<int>(obs => () => { disposed = true; });

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            Assert.True(disposed, "disposed");
        }

        [Fact]
        public void SkipUntil_Immediate()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Return(1);
            var ys = Observable.Return("bar");

            var res = scheduler.Start(() =>
                xs.SkipUntil(ys)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnCompleted<int>(200)
            );
        }

        [Fact] // Asserts behaviour considered buggy. A fix is desirable but breaking.
        public void SkipUntil_Empty_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnCompleted(250, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnCompleted(250, 1)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                );

            //Desired behaviour:
            //res.Messages.AssertEqual(
            //    OnCompleted(250, 1));
        }
        #endregion

        #region + Timed +

        [Fact]
        public void SkipUntil_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(default(IObservable<int>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(xs, DateTimeOffset.Now, default));
        }

        [Fact]
        public void SkipUntil_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(), scheduler)
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
        public void SkipUntil_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
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
        public void SkipUntil_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void SkipUntil_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SkipUntil_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void SkipUntil_Twice1()
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
                xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler).SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
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
        public void SkipUntil_Twice2()
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
                xs.SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler).SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
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
        public void SkipUntil_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipUntil(DateTimeOffset.UtcNow.AddMinutes(1));

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
