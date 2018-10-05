// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RepeatTest : ReactiveTest
    {

        [Fact]
        public void Repeat_Value_Count_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 0, default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Repeat(1, -1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 1, DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void Repeat_Value_Count_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 0, scheduler)
            );

#if !NO_PERF
            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
#else
            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
#endif
        }

        [Fact]
        public void Repeat_Value_Count_One()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnCompleted<int>(201)
            );
        }

        [Fact]
        public void Repeat_Value_Count_Ten()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 10, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42),
                OnNext(207, 42),
                OnNext(208, 42),
                OnNext(209, 42),
                OnNext(210, 42),
                OnCompleted<int>(210)
            );
        }

        [Fact]
        public void Repeat_Value_Count_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 10, scheduler),
                207
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42)
            );
        }

        [Fact]
        public void Repeat_Value_Count_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Repeat(1, -1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 1).Subscribe(null));
        }

        [Fact]
        public void Repeat_Value_Count_Default()
        {
            Observable.Repeat(42, 10).AssertEqual(Observable.Repeat(42, 10, DefaultScheduler.Instance));
        }

        [Fact]
        public void Repeat_Value_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(DummyScheduler.Instance, 1).Subscribe(null));
        }

        [Fact]
        public void Repeat_Value()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, scheduler),
                207
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42)
            );
        }

        [Fact]
        public void Repeat_Value_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1).Subscribe(null));
        }

        [Fact]
        public void Repeat_Value_Default()
        {
            Observable.Repeat(42).Take(100).AssertEqual(Observable.Repeat(42, DefaultScheduler.Instance).Take(100));
        }

#if !NO_PERF
        [Fact]
        public void Repeat_Count_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, 100, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.True(lst.SequenceEqual(Enumerable.Repeat(42, 100)));
            Assert.True(done);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void Repeat_Count_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, int.MaxValue, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
            {
                ;
            }

            d.Dispose();
            end.WaitOne();

            Assert.True(true);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void Repeat_Inf_LongRunning()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
            {
                ;
            }

            d.Dispose();
            end.WaitOne();

            Assert.True(true);
        }
#endif

        [Fact]
        public void Repeat_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat().Subscribe(null));
        }

        [Fact]
        public void Repeat_Observable_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Repeat()
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
        public void Repeat_Observable_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Repeat()
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
        public void Repeat_Observable_Error()
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
                xs.Repeat()
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
        public void Repeat_Observable_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Repeat();

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Repeat();

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Repeat();

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(210, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Repeat();

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [Fact]
        public void Repeat_Observable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat().Subscribe(null));
        }

        [Fact]
        public void Repeat_Observable_RepeatCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Repeat(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat(0).Subscribe(null));
        }

        [Fact]
        public void Repeat_Observable_RepeatCount_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3)
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
        public void Repeat_Observable_RepeatCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3), 231
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
        public void Repeat_Observable_RepeatCount_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3)
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
        public void Repeat_Observable_RepeatCount_Error()
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
                xs.Repeat(3)
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
        public void Repeat_Observable_RepeatCount_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Repeat(3);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Repeat(3);

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Repeat(100);

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(10, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Repeat(3);

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [Fact]
        public void Repeat_Observable_RepeatCount_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(default(IObservable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Repeat(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat(0).Subscribe(null));
        }

    }
}
