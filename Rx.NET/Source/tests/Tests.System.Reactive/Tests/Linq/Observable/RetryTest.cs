// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RetryTest : ReactiveTest
    {

        [Fact]
        public void Retry_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry().Subscribe(null));
        }

        [Fact]
        public void Retry_Observable_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Retry()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void Retry_Observable_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Retry()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Retry_Observable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Retry(), 1100
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnNext(550, 1),
                OnNext(600, 2),
                OnNext(650, 3),
                OnNext(800, 1),
                OnNext(850, 2),
                OnNext(900, 3),
                OnNext(1050, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1100)
            );
        }

        [Fact]
        public void Retry_Observable_Throws1()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Retry();

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [Fact]
        public void Retry_Observable_Throws2()
        {
            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Retry();

            var d = ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            scheduler2.ScheduleAbsolute(210, () => d.Dispose());

            scheduler2.Start();
        }

        [Fact]
        public void Retry_Observable_Throws3()
        {
            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Retry();

            zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler3.Start());
        }

        /*
         * BREAKING CHANGE v2.0 > v1.x - The code below will loop endlessly, trying to repeat the failing subscription,
         *                               whose exception is propagated through OnError starting from v2.0.
         * 
        [Fact]
        public void Retry_Observable_Throws4()
        {
            var xss = Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Retry();

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }
         */

        [Fact]
        public void Retry_Observable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry().Subscribe(null));
        }

        [Fact]
        public void Retry_Observable_RetryCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry(0).Subscribe(null));
        }

        [Fact]
        public void Retry_Observable_RetryCount_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, ex)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2),
                OnNext(235, 3),
                OnNext(245, 1),
                OnNext(250, 2),
                OnNext(255, 3),
                OnError<int>(260, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 240),
                Subscribe(240, 260)
            );
        }

        [Fact]
        public void Retry_Observable_RetryCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Retry(3), 231
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 231)
            );
        }

        [Fact]
        public void Retry_Observable_RetryCount_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Retry_Observable_RetryCount_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void Retry_Observable_RetryCount_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Retry(3);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Retry(100);

            var d = ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            scheduler2.ScheduleAbsolute(10, () => d.Dispose());

            scheduler2.Start();

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Retry(100);

            zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler3.Start());

            var xss = Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Retry(3);

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [Fact]
        public void Retry_Observable_RetryCount_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(default, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry(0).Subscribe(null));
        }

        [Fact]
        public void Retry_Observable_RetryCount_Default()
        {
            Observable.Range(1, 3).Retry(3).AssertEqual(Observable.Range(1, 3).Retry(3));
        }

    }
}
