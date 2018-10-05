// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AllTest : ReactiveTest
    {
        [Fact]
        public void All_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.All(DummyObservable<int>.Instance, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.All(default(IObservable<int>), x => true));
        }

        [Fact]
        public void All_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, true),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void All_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, true),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void All_ReturnNotMatch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, -2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(210, false),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void All_SomeNoneMatch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, -2),
                OnNext(220, -3),
                OnNext(230, -4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(210, false),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void All_SomeMatch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, -2),
                OnNext(220, 3),
                OnNext(230, -4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(210, false),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void All_SomeAllMatch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, true),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void All_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void All_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.All(x => x > 0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void All_PredicateThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.All(x => { if (x < 4) { return true; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<bool>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }
    }
}
