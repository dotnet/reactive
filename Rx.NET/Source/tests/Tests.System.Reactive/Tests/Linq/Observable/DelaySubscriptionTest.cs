// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DelaySubscriptionTest : ReactiveTest
    {

        [Fact]
        public void DelaySubscription_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, DateTimeOffset.Now, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.Zero, default));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.FromSeconds(-1), Scheduler.Immediate));
        }

        [Fact]
        public void DelaySubscription_TimeSpan_Default()
        {
            var lst = new List<int>();
            Observable.Range(0, 10).DelaySubscription(TimeSpan.FromMilliseconds(1)).ForEach(lst.Add);
            Assert.True(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [Fact]
        public void DelaySubscription_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [Fact]
        public void DelaySubscription_TimeSpan_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnError<int>(70, ex)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [Fact]
        public void DelaySubscription_DateTimeOffset_Default()
        {
            var lst = new List<int>();
            Observable.Range(0, 10).DelaySubscription(DateTimeOffset.UtcNow.AddMilliseconds(1)).ForEach(lst.Add);
            Assert.True(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [Fact]
        public void DelaySubscription_DateTimeOffset_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [Fact]
        public void DelaySubscription_DateTimeOffset_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnError<int>(70, ex)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

    }
}
