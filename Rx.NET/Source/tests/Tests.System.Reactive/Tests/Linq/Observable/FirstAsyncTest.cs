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
    public class FirstAsyncTest : ReactiveTest
    {
        [Fact]
        public void FirstAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void FirstAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void FirstAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void FirstAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void FirstAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void FirstAsync_Predicate()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnCompleted<int>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void FirstAsync_Predicate_None()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void FirstAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void FirstAsync_PredicateThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstAsync(x => { if (x < 4) { return false; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

    }
}
