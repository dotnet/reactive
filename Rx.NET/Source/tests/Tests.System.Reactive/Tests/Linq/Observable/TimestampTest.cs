// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class TimestampTest : ReactiveTest
    {

        [Fact]
        public void Timestamp_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(default(IObservable<int>), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(someObservable, null));
        }

        [Fact]
        public void Timestamp_Regular()
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
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, new Timestamped<int>(2, new DateTimeOffset(210, TimeSpan.Zero))),
                OnNext(230, new Timestamped<int>(3, new DateTimeOffset(230, TimeSpan.Zero))),
                OnNext(260, new Timestamped<int>(4, new DateTimeOffset(260, TimeSpan.Zero))),
                OnNext(300, new Timestamped<int>(5, new DateTimeOffset(300, TimeSpan.Zero))),
                OnNext(350, new Timestamped<int>(6, new DateTimeOffset(350, TimeSpan.Zero))),
                OnCompleted<Timestamped<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Timestamp_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<Timestamped<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Timestamp_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<Timestamped<int>>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Timestamp_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Timestamp_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).Timestamp().Count().First() == 1);
        }

    }
}
