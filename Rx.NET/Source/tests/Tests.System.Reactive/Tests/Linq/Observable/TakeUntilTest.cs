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
    public class TakeUntilTest : ReactiveTest
    {
        #region + Observable +

        [Fact]
        public void TakeUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil<int, int>(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil<int, int>(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void TakeUntil_Preempt_SomeData_Next()
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
                OnNext(225, 99),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void TakeUntil_Preempt_SomeData_Error()
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
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
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
        public void TakeUntil_NoPreempt_SomeData_Empty()
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
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
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
        public void TakeUntil_NoPreempt_SomeData_Never()
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
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void TakeUntil_Preempt_Never_Next()
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
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void TakeUntil_Preempt_Never_Error()
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
                l.TakeUntil(r)
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
        public void TakeUntil_NoPreempt_Never_Empty()
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
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void TakeUntil_NoPreempt_Never_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
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
        public void TakeUntil_Preempt_BeforeFirstProduced()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //!
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeUntil_Preempt_BeforeFirstProduced_RemainSilentAndProperDisposed()
        {
            var scheduler = new TestScheduler();

            var sourceNotDisposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(215, new Exception()), // should not come
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //!
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                l.Do(_ => sourceNotDisposed = true).TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            Assert.False(sourceNotDisposed);
        }

        [Fact]
        public void TakeUntil_NoPreempt_AfterLastProduced_ProperDisposedSignal()
        {
            var scheduler = new TestScheduler();

            var signalNotDisposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r.Do(_ => signalNotDisposed = true))
            );

            res.Messages.AssertEqual(
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            Assert.False(signalNotDisposed);
        }

        [Fact]
        public void TakeUntil_Error_Some()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 2)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
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
        public void TakeUntil_Immediate()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Return(1);
            var ys = Observable.Return("bar");

            var res = scheduler.Start(() =>
                xs.TakeUntil(ys)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }
        #endregion

        #region + Timed +

        [Fact]
        public void TakeUntil_Timed_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(default(IObservable<int>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(xs, DateTimeOffset.Now, default));
        }

        [Fact]
        public void TakeUntil_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [Fact]
        public void TakeUntil_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(225, TimeSpan.Zero), scheduler)
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
        public void TakeUntil_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
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
        public void TakeUntil_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeUntil_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void TakeUntil_Twice1()
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
                xs.TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler).TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler)
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
        public void TakeUntil_Twice2()
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
                xs.TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler).TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler)
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
        public void TakeUntil_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeUntil(DateTimeOffset.Now.AddMinutes(1));

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

        #region + Predicate +

        [Fact]
        public void TakeUntil_Predicate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil<int>(null, v => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void TakeUntil_Predicate_Basic()
        {
            var scheduler = new TestScheduler();

            var source = scheduler.CreateColdObservable(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3),
                    OnNext(40, 4),
                    OnNext(50, 5),
                    OnNext(60, 6),
                    OnNext(70, 7),
                    OnNext(80, 8),
                    OnNext(90, 9),
                    OnCompleted<int>(100)
                );

            var result = scheduler.Start(() => source.TakeUntil(v => v == 5));

            result.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnCompleted<int>(250)
            );

            source.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void TakeUntil_Predicate_True()
        {
            var scheduler = new TestScheduler();

            var source = scheduler.CreateColdObservable(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3),
                    OnNext(40, 4),
                    OnNext(50, 5),
                    OnNext(60, 6),
                    OnNext(70, 7),
                    OnNext(80, 8),
                    OnNext(90, 9),
                    OnCompleted<int>(100)
                );

            var result = scheduler.Start(() => source.TakeUntil(v => true));

            result.Messages.AssertEqual(
                OnNext(210, 1),
                OnCompleted<int>(210)
            );

            source.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void TakeUntil_Predicate_False()
        {
            var scheduler = new TestScheduler();

            var source = scheduler.CreateColdObservable(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3),
                    OnNext(40, 4),
                    OnNext(50, 5),
                    OnNext(60, 6),
                    OnNext(70, 7),
                    OnNext(80, 8),
                    OnNext(90, 9),
                    OnCompleted<int>(100)
                );

            var result = scheduler.Start(() => source.TakeUntil(v => false));

            result.Messages.AssertEqual(
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

            source.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TakeUntil_Predicate_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException();

            var source = scheduler.CreateColdObservable(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3),
                    OnError<int>(40, ex)
                );

            var result = scheduler.Start(() => source.TakeUntil(v => false));

            result.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            source.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void TakeUntil_Predicate_Crash()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException();

            var source = scheduler.CreateColdObservable(
                    OnNext(10, 1),
                    OnNext(20, 2),
                    OnNext(30, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnCompleted<int>(260)
                );

            var result = scheduler.Start(() => source.TakeUntil(v =>
            {
                if (v == 3)
                {
                    throw ex;
                }
                return false;
            }));

            result.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(230, ex)
            );

            source.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

    }
}
