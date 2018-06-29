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
    public class AggregateTest : ReactiveTest
    {
        [Fact]
        public void Aggregate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int>(default, 1, (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate(DummyObservable<int>.Instance, 1, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int>(default, (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate(DummyObservable<int>.Instance, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int, int>(default, 1, (x, y) => x + y, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate(DummyObservable<int>.Instance, 1, default, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int, int>(DummyObservable<int>.Instance, 1, (x, y) => x + y, default));
        }

        [Fact]
        public void AggregateWithSeed_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(250, 42),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithSeed_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 24),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(250, 42 + 24),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithSeed_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void AggregateWithSeed_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void AggregateWithSeed_Range()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(260, 42 + Enumerable.Range(0, 5).Sum()),
                OnCompleted<int>(260)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 260)
            );
        }

        [Fact]
        public void AggregateWithSeed_AccumulatorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(0, (acc, x) => { if (x < 3) { return acc + x; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnNext(250, 42 * 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 24),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnNext(250, (42 + 24) * 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x, x => x * 5)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_Range()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(42, (acc, x) => acc + x, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnNext(260, (42 + Enumerable.Range(0, 5).Sum()) * 5),
                OnCompleted<int>(260)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 260)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_AccumulatorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate(0, (acc, x) => { if (x < 3) { return acc + x; } throw ex; }, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }

        [Fact]
        public void AggregateWithSeedAndResult_ResultSelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate<int, int, int>(0, (acc, x) => acc + x, x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(260, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 260)
            );
        }

        [Fact]
        public void AggregateWithoutSeed_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithoutSeed_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 24),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(250, 24),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void AggregateWithoutSeed_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void AggregateWithoutSeed_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void AggregateWithoutSeed_Range()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(260, Enumerable.Range(0, 5).Sum()),
                OnCompleted<int>(260)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 260)
            );
        }

        [Fact]
        public void AggregateWithoutSeed_AccumulatorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 0),
                OnNext(220, 1),
                OnNext(230, 2),
                OnNext(240, 3),
                OnNext(250, 4),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Aggregate((acc, x) => { if (x < 3) { return acc + x; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }
    }
}
