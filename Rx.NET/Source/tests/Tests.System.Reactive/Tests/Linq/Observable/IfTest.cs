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
    public class IfTest : ReactiveTest
    {

        [Fact]
        public void If_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(null, DummyObservable<int>.Instance, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, DummyObservable<int>.Instance, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(null, DummyObservable<int>.Instance, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, default(IObservable<int>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, DummyObservable<int>.Instance, default(IScheduler)));
        }

        [Fact]
        public void If_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => true, xs, ys));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void If_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => false, xs, ys));

            results.Messages.AssertEqual(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void If_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.If(() => Throw<bool>(ex), xs, ys));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void If_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => true, xs, ys));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(250, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void If_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If<int>(DummyFunc<bool>.Instance, null));
        }

        [Fact]
        public void If_Default_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnCompleted<int>(440)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3),
                OnCompleted<int>(440)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [Fact]
        public void If_Default_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, ex)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [Fact]
        public void If_Default_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void If_Default_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, new Exception())
            );

            var b = true;

            scheduler.ScheduleAbsolute(150, () => b = false);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void If_Default_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, new Exception())
            );

            var results = scheduler.Start(() => Observable.If(() => false, xs, scheduler));

            results.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        private static T Throw<T>(Exception ex)
        {
            throw ex;
        }
    }
}
