// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AnyTest : ReactiveTest
    {
        [Fact]
        public void Any_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(DummyObservable<int>.Instance, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(default(IObservable<int>), x => true));
        }

        [Fact]
        public void Any_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any()
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Any_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any()
            );

            res.Messages.AssertEqual(
                OnNext(210, true),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Any_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Any()
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Any_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Any()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Any_Predicate_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Any_Predicate_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(210, true),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Any_Predicate_ReturnNotMatch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, -2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Any_Predicate_SomeNoneMatch()
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
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Any_Predicate_SomeMatch()
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
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnNext(220, true),
                OnCompleted<bool>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Any_Predicate_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Any_Predicate_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => x > 0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Any_Predicate_PredicateThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, -2),
                OnNext(220, 3),
                OnNext(230, -4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Any(x => { if (x != -4) { return false; } throw ex; })
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
