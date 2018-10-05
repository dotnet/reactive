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
    public class ExpandTest : ReactiveTest
    {

        [Fact]
        public void Expand_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(null, DummyFunc<int, IObservable<int>>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(null, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void Expand_Default()
        {
            var b = Observable.Return(1).Expand(x => x < 10 ? Observable.Return(x + 1) : Observable.Empty<int>())
                        .SequenceEqual(Observable.Range(1, 10)).First();

            Assert.True(b);
        }

        [Fact]
        public void Expand_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(200, 2),
                    OnCompleted<int>(300)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 300)
            );
        }

        [Fact]
        public void Expand_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100 + x, 2 * x),
                    OnNext(200 + x, 3 * x),
                    OnCompleted<int>(300 + x)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 300)
            );
        }

        [Fact]
        public void Expand_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100 + x, 2 * x),
                    OnNext(200 + x, 3 * x),
                    OnCompleted<int>(300 + x)
                ), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );
        }

        [Fact]
        public void Expand_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(550, 1),
                OnNext(850, 2),
                OnCompleted<int>(950)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100, 2 * x),
                    OnNext(200, 3 * x),
                    OnCompleted<int>(300)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(550, 1),
                OnNext(651, 2),
                OnNext(751, 3),
                OnNext(752, 4),
                OnNext(850, 2),
                OnNext(852, 6),
                OnNext(852, 6),
                OnNext(853, 8),
                OnNext(951, 4),
                OnNext(952, 9),
                OnNext(952, 12),
                OnNext(953, 12),
                OnNext(953, 12),
                OnNext(954, 16)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 950)
            );
        }

        [Fact]
        public void Expand_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(550, 1),
                OnNext(850, 2),
                OnCompleted<int>(950)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Expand(x => { throw ex; }, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(550, 1),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 550)
            );
        }

    }
}
