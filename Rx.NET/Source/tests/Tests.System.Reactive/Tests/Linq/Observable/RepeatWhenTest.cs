// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RepeatWhenTest : ReactiveTest
    {

        [Fact]
        public void RepeatWhen_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RepeatWhen<int, object>(null, v => v));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RepeatWhen<int, object>(Observable.Return(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.RepeatWhen(v => v).Subscribe(null));
        }

        [Fact]
        public void RepeatWhen_Handler_Crash()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnCompleted<int>(10)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                xs.RepeatWhen<int, object>(v => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void RepeatWhen_Handler_Error()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnCompleted<int>(10)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v => v.Select<object, object>(w => throw ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void RepeatWhen_Handler_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnCompleted<int>(10)
            );

            var ex = new InvalidOperationException();

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v => v.Take(1).Skip(1))
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void RepeatWhen_Disposed()
        {
            var main = new Subject<int>();
            var inner = new Subject<int>();

            var d = main.RepeatWhen(v => inner).Subscribe();

            Assert.True(main.HasObservers);
            Assert.True(inner.HasObservers);

            d.Dispose();

            Assert.False(main.HasObservers);
            Assert.False(inner.HasObservers);
        }

        [Fact]
        public void RepeatWhen_Handler_Completed_Disposes_Main()
        {
            var main = new Subject<int>();
            var inner = new Subject<int>();

            var end = 0;
            var items = 0;
            var errors = 0;

            main.RepeatWhen(v => inner).Subscribe(
                onNext: v => items++,
                onError: e => errors++,
                onCompleted: () => end++);

            Assert.True(main.HasObservers);
            Assert.True(inner.HasObservers);

            inner.OnCompleted();

            Assert.False(main.HasObservers);
            Assert.False(inner.HasObservers);

            Assert.Equal(0, items);
            Assert.Equal(0, errors);
            Assert.Equal(1, end);
        }

        [Fact]
        public void RepeatWhen_Handler_Error_Disposes_Main()
        {
            var main = new Subject<int>();
            var inner = new Subject<int>();

            var end = 0;
            var items = 0;
            var errors = 0;

            main.RepeatWhen(v => inner).Subscribe(
                onNext: v => items++,
                onError: e => errors++,
                onCompleted: () => end++);

            Assert.True(main.HasObservers);
            Assert.True(inner.HasObservers);

            inner.OnError(new InvalidOperationException());

            Assert.False(main.HasObservers);
            Assert.False(inner.HasObservers);

            Assert.Equal(0, items);
            Assert.Equal(1, errors);
            Assert.Equal(0, end);
        }

        [Fact]
        public void RepeatWhen_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v => v)
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
                OnNext(900, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1000)
            );
        }

        [Fact]
        public void RepeatWhen_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v => v)
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
        public void RepeatWhen_Error()
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
                xs.RepeatWhen(v => v)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void RepeatWhen_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).RepeatWhen(v => v);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).RepeatWhen(v => v);

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).RepeatWhen(v => v);

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(210, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).RepeatWhen(v => v);

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [Fact]
        public void RepeatWhen_RepeatCount_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v =>
                {
                    var count = 0;
                    return v.TakeWhile(w => ++count < 3);
                })
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
                OnCompleted<int>(260)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 240),
                Subscribe(240, 260)
            );
        }

        [Fact]
        public void RepeatWhen_RepeatCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v =>
                {
                    var count = 0;
                    return v.TakeWhile(w => ++count < 3);
                }), 231
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
        public void RepeatWhen_RepeatCount_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v =>
                {
                    var count = 0;
                    return v.TakeWhile(w => ++count < 3);
                })
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
        public void RepeatWhen_RepeatCount_Error()
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
                xs.RepeatWhen(v =>
                {
                    var count = 0;
                    return v.TakeWhile(w => ++count < 3);
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void RepeatWhen_RepeatCount_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            });

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            });

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 100);
            });

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(10, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            });

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [Fact]
        public void RepeatWhen_Observable_Repeat_Delayed()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.RepeatWhen(v =>
                {
                    int[] count = { 0 };
                    return v.SelectMany(w =>
                    {
                        var c = ++count[0];
                        if (c == 3)
                        {
                            return Observable.Throw<int>(ex);
                        }
                        return Observable.Return(1).Delay(TimeSpan.FromTicks(c * 100), scheduler);
                    });
                })
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(325, 1),
                OnNext(330, 2),
                OnNext(335, 3),
                OnNext(545, 1),
                OnNext(550, 2),
                OnNext(555, 3),
                OnError<int>(560, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(320, 340),
                Subscribe(540, 560)
            );
        }
    }
}
