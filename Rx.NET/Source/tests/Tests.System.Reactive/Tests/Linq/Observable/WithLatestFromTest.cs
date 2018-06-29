// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class WithLatestFromTest : ReactiveTest
    {

        [Fact]
        public void WithLatestFrom_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.WithLatestFrom<int, int, int>(someObservable, someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.WithLatestFrom<int, int, int>(null, someObservable, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.WithLatestFrom<int, int, int>(someObservable, default, (_, __) => 0));
        }

        [Fact]
        public void WithLatestFrom_Simple1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(410, 7),
                OnNext(420, 8),
                OnNext(470, 9),
                OnNext(550, 10),
                OnCompleted<int>(590)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(260, "4bar"),
                OnNext(310, "5bar"),
                OnNext(340, "6foo"),
                OnNext(410, "7qux"),
                OnNext(420, "8qux"),
                OnNext(470, "9qux"),
                OnNext(550, "10qux"),
                OnCompleted<string>(590)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void WithLatestFrom_Simple2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnCompleted<int>(390)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnNext(370, "baz"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(260, "4bar"),
                OnNext(310, "5bar"),
                OnNext(340, "6foo"),
                OnCompleted<string>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void WithLatestFrom_Simple3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnCompleted<int>(390)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(245, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnNext(370, "baz"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(250, "3bar"),
                OnNext(260, "4bar"),
                OnNext(310, "5bar"),
                OnNext(340, "6foo"),
                OnCompleted<string>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void WithLatestFrom_Error1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(410, 7),
                OnNext(420, 8),
                OnNext(470, 9),
                OnNext(550, 10),
                OnError<int>(590, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(260, "4bar"),
                OnNext(310, "5bar"),
                OnNext(340, "6foo"),
                OnNext(410, "7qux"),
                OnNext(420, "8qux"),
                OnNext(470, "9qux"),
                OnNext(550, "10qux"),
                OnError<string>(590, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void WithLatestFrom_Error2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnCompleted<int>(390)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnError<string>(370, ex)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(260, "4bar"),
                OnNext(310, "5bar"),
                OnNext(340, "6foo"),
                OnError<string>(370, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 370)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 370)
            );
        }

        [Fact]
        public void WithLatestFrom_Error3()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnCompleted<int>(390)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, "bar"),
                OnNext(330, "foo"),
                OnNext(350, "qux"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) =>
                {
                    if (x == 5)
                    {
                        throw ex;
                    }

                    return x + y;
                })
            );

            res.Messages.AssertEqual(
                OnNext(260, "4bar"),
                OnError<string>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void WithLatestFrom_immediate()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Return(1);
            var ys = Observable.Return("bar");

            var res = scheduler.Start(() =>
                xs.WithLatestFrom(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(200, "1bar"),
                OnCompleted<string>(200)
            );
        }
    }
}
