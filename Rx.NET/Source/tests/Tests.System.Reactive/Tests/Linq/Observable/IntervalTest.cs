// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class IntervalTest : ReactiveTest
    {

        [Fact]
        public void Interval_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Interval(TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Interval(TimeSpan.Zero, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Interval(TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Interval(TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
        }

        [Fact]
        public void Interval_TimeSpan_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Interval(TimeSpan.FromTicks(100), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L),
                OnNext(800, 5L),
                OnNext(900, 6L)
            );
        }

        [Fact]
        public void Interval_TimeSpan_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Interval(TimeSpan.FromTicks(0), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnNext(202, 1L),
                OnNext(203, 2L),
                OnNext(204, 3L),
                OnNext(205, 4L),
                OnNext(206, 5L),
                OnNext(207, 6L),
                OnNext(208, 7L),
                OnNext(209, 8L)
            );
        }

        [Fact]
        public void Interval_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();

            var completed = new ManualResetEvent(false);

            Observable.Interval(TimeSpan.Zero).TakeWhile(i => i < 10).Subscribe(observer.OnNext, () => completed.Set());

            completed.WaitOne();

            Assert.Equal(10, observer.Messages.Count);
        }

        [Fact]
        public void Interval_TimeSpan_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(
                () => Observable.Interval(TimeSpan.FromTicks(1000), scheduler)
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Interval_TimeSpan_ObserverThrows()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Interval(TimeSpan.FromTicks(1), scheduler);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
        }

        [Fact]
        public void Interval_TimeSpan_DefaultScheduler()
        {
            Assert.True(Observable.Interval(TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(3).SequenceEqual(new[] { 0L, 1L, 2L }));
        }

    }
}
