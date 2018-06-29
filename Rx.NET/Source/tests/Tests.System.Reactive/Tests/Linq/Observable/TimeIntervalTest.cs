// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class TimeIntervalTest : ReactiveTest
    {

        [Fact]
        public void TimeInterval_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(default(IObservable<int>), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(someObservable, null));
        }

        [Fact]
        public void TimeInterval_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
                OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
                OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
                OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
                OnCompleted<TimeInterval<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void TimeInterval_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnCompleted<TimeInterval<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TimeInterval_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnError<TimeInterval<int>>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TimeInterval_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void TimeInterval_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).TimeInterval().Count().First() == 1);
        }

        [Fact]
        public void TimeInterval_WithStopwatch_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
                OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
                OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
                OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
                OnCompleted<TimeInterval<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void TimeInterval_WithStopwatch_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<TimeInterval<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TimeInterval_WithStopwatch_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<TimeInterval<int>>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void TimeInterval_WithStopwatch_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

    }
}
