// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableAggregateTest : ReactiveTest
    {
        #region + Aggregate +

        [TestMethod]
        public void Aggregate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int>(default(IObservable<int>), 1, (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int>(DummyObservable<int>.Instance, 1, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int>(default(IObservable<int>), (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int>(DummyObservable<int>.Instance, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int, int>(default(IObservable<int>), 1, (x, y) => x + y, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int, int>(DummyObservable<int>.Instance, 1, default(Func<int, int, int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Aggregate<int, int, int>(DummyObservable<int>.Instance, 1, (x, y) => x + y, default(Func<int, int>)));
        }

        [TestMethod]
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

        [TestMethod]
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
                OnNext<int>(250, 42 + 24),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.Aggregate(0, (acc, x) => { if (x < 3) return acc + x; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }

        [TestMethod]
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

        [TestMethod]
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
                OnNext<int>(250, (42 + 24) * 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.Aggregate(0, (acc, x) => { if (x < 3) return acc + x; throw ex; }, x => x * 5)
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                OnNext<int>(250, 24),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.Aggregate((acc, x) => { if (x < 3) return acc + x; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
               Subscribe(200, 240)
            );
        }

        #endregion

        #region + All +

        [TestMethod]
        public void All_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.All(DummyObservable<int>.Instance, default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.All(default(IObservable<int>), x => true));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.All(x => { if (x < 4) return true; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<bool>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + Any +

        [TestMethod]
        public void Any_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(DummyObservable<int>.Instance, default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Any(default(IObservable<int>), x => true));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.Any(x => { if (x != -4) return false; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<bool>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + Average +

        [TestMethod]
        public void Average_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(int)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));
        }

        [TestMethod]
        public void Average_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 3L),
                OnNext(220, 4L),
                OnNext(230, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 3.0),
                OnNext(220, 4.0),
                OnNext(230, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 3f),
                OnNext(220, 4f),
                OnNext(230, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 3m),
                OnNext(220, 4m),
                OnNext(230, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)3),
                OnNext(220, (int?)null),
                OnNext(230, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)3L),
                OnNext(220, (long?)null),
                OnNext(230, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)3.0),
                OnNext(220, (double?)null),
                OnNext(230, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)null),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)3f),
                OnNext(220, (float?)null),
                OnNext(230, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2.5f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Average_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)null),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)3m),
                OnNext(220, (decimal?)null),
                OnNext(230, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2.5m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Average_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Average_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void Average_InjectOverflow_Int32()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Int64()
        {
            var xs = Observable.Return(42L, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<long>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Double()
        {
            var xs = Observable.Return(42.0, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<double>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Single()
        {
            var xs = Observable.Return(42.0f, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<float>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Decimal()
        {
            var xs = Observable.Return(42.0m, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<decimal>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Int32_Nullable()
        {
            var xs = Observable.Return((int?)42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Int64_Nullable()
        {
            var xs = Observable.Return((long?)42L, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<long?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Double_Nullable()
        {
            var xs = Observable.Return((double?)42.0, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<double?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Single_Nullable()
        {
            var xs = Observable.Return((float?)42.0f, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<float?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [TestMethod]
        public void Average_InjectOverflow_Decimal_Nullable()
        {
            var xs = Observable.Return((decimal?)42.0m, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<decimal?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        class OverflowInjection<T> : IObservable<T>
        {
            private readonly IObservable<T> _source;
            private readonly object _initialCount;

            public OverflowInjection(IObservable<T> source, object initialCount)
            {
                _source = source;
                _initialCount = initialCount;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var f = observer.GetType().GetField("_count", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                f.SetValue(observer, _initialCount);

                return _source.Subscribe(observer);
            }
        }
#endif

        [TestMethod]
        public void Average_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (int)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)2.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Average_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)2.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region + Contains +

        [TestMethod]
        public void Contains_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(default(IObservable<int>), 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(default(IObservable<int>), 42, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Contains(DummyObservable<int>.Instance, 42, null));
        }

        [TestMethod]
        public void Contains_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Contains_ReturnPositive()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(2)
            );

            res.Messages.AssertEqual(
                OnNext(210, true),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Contains_ReturnNegative()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(-2)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Contains_SomePositive()
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
                xs.Contains(3)
            );

            res.Messages.AssertEqual(
                OnNext(220, true),
                OnCompleted<bool>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Contains_SomeNegative()
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
                xs.Contains(-3)
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Contains_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Contains_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Contains_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42, new ContainsComparerThrows())
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, e => e is NotImplementedException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        class ContainsComparerThrows : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Contains_ComparerContainsValue()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 8),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(42, new ContainsComparerMod2())
            );

            res.Messages.AssertEqual(
                OnNext(220, true),
                OnCompleted<bool>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Contains_ComparerDoesNotContainValue()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 4),
                OnNext(230, 8),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Contains(21, new ContainsComparerMod2())
            );

            res.Messages.AssertEqual(
                OnNext(250, false),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        class ContainsComparerMod2 : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion

        #region + Count +

        [TestMethod]
        public void Count_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Count(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void Count_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Some()
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
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Count_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Count()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void Count_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, int.MaxValue).Count();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        [TestMethod]
        public void Count_Predicate_Empty_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Empty_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Return_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Return_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Some_All()
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
                xs.Count(x => x < 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Some_None()
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
                xs.Count(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Some_Even()
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
                xs.Count(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Count_Predicate_Throw_True()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Count_Predicate_Throw_False()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => false)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Count_Predicate_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Count(_ => true)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Count_Predicate_PredicateThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Count(x =>
                {
                    if (x == 3)
                        throw ex;

                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void Count_Predicate_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, int.MaxValue).Count(_ => true);

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        #endregion

        #region + ElementAt +

        [TestMethod]
        public void ElementAt_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ElementAt(default(IObservable<int>), 2));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ElementAt(DummyObservable<int>.Instance, -1));
        }

        [TestMethod]
        public void ElementAt_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAt(0)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnCompleted<int>(280)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 280)
            );
        }

        [TestMethod]
        public void ElementAt_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAt(2)
            );

            res.Messages.AssertEqual(
                OnNext(470, 44),
                OnCompleted<int>(470)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [TestMethod]
        public void ElementAt_OutOfRange()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAt(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(600, e => e is ArgumentOutOfRangeException)
            );
        }

        [TestMethod]
        public void ElementAt_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.ElementAt(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        #endregion

        #region + ElementAtOrDefault +

        [TestMethod]
        public void ElementAtOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ElementAtOrDefault(default(IObservable<int>), 2));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.ElementAtOrDefault(DummyObservable<int>.Instance, -1));
        }

        [TestMethod]
        public void ElementAtOrDefault_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(0)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnCompleted<int>(280)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 280)
            );
        }

        [TestMethod]
        public void ElementAtOrDefault_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(2)
            );

            res.Messages.AssertEqual(
                OnNext(470, 44),
                OnCompleted<int>(470)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [TestMethod]
        public void ElementAtOrDefault_OutOfRange()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnNext(470, 44),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(3)
            );

            res.Messages.AssertEqual(
                OnNext(600, 0),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void ElementAtOrDefault_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(280, 42),
                OnNext(360, 43),
                OnError<int>(420, ex)
            );

            var res = scheduler.Start(() =>
                xs.ElementAtOrDefault(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        #endregion

        #region + FirstAsync +

        [TestMethod]
        public void FirstAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                xs.FirstAsync(x => { if (x < 4) return false; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + FirstOrDefaultAsync +

        [TestMethod]
        public void FirstOrDefaultAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefaultAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefaultAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefaultAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.FirstOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.FirstOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Predicate()
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
                xs.FirstOrDefaultAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnCompleted<int>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Predicate_None()
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
                xs.FirstOrDefaultAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                xs.FirstOrDefaultAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void FirstOrDefaultAsync_PredicateThrows()
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
                xs.FirstOrDefaultAsync(x => { if (x < 4) return false; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + IsEmpty +

        [TestMethod]
        public void IsEmpty_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.IsEmpty(default(IObservable<int>)));
        }

        [TestMethod]
        public void IsEmpty_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.IsEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(250, true),
                OnCompleted<bool>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void IsEmpty_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.IsEmpty()
            );

            res.Messages.AssertEqual(
                OnNext(210, false),
                OnCompleted<bool>(210)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void IsEmpty_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.IsEmpty()
            );

            res.Messages.AssertEqual(
                OnError<bool>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void IsEmpty_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.IsEmpty()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + LastAsync +

        [TestMethod]
        public void LastAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void LastAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LastAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LastAsync_Predicate()
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
                xs.LastAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastAsync_Predicate_None()
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
                xs.LastAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LastAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LastAsync_PredicateThrows()
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
                xs.LastAsync(x => { if (x < 4) return x % 2 == 1; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + LastOrDefaultAsync +

        [TestMethod]
        public void LastOrDefaultAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefaultAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefaultAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefaultAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void LastOrDefaultAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LastOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LastOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_Predicate()
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
                xs.LastOrDefaultAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_Predicate_None()
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
                xs.LastOrDefaultAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LastOrDefaultAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LastOrDefaultAsync_PredicateThrows()
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
                xs.LastOrDefaultAsync(x => { if (x < 4) return x % 2 == 1; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + LongCount +

        [TestMethod]
        public void LongCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LongCount(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void LongCount_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 1L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Some()
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
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LongCount_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.LongCount()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void LongCount_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).LongCount();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        [TestMethod]
        public void LongCount_Predicate_Empty_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Empty_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Return_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnNext(250, 1L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Return_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Some_All()
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
                xs.LongCount(x => x < 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Some_None()
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
                xs.LongCount(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Some_Even()
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
                xs.LongCount(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Throw_True()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Throw_False()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => false)
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.LongCount(_ => true)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void LongCount_Predicate_PredicateThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.LongCount(x =>
                {
                    if (x == 3)
                        throw ex;

                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnError<long>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void LongCount_Predicate_InjectOverflow()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).LongCount(_ => true);

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif

        #endregion

        #region + Max +

        [TestMethod]
        public void Max_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(int)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), Comparer<DateTime>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(IComparer<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, string>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(default(IObservable<DateTime>), _ => "", Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), default(Func<DateTime, string>), Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Max(Observable.Empty<DateTime>(), _ => "", default(IComparer<string>)));
        }

        [TestMethod]
        public void Max_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<long>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 3L),
                OnNext(220, 4L),
                OnNext(230, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 4L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<float>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 3f),
                OnNext(220, 4f),
                OnNext(230, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 4f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 3.0),
                OnNext(220, 4.0),
                OnNext(230, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 4.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 3m),
                OnNext(220, 4m),
                OnNext(230, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, 4m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(int?)),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)4),
                OnNext(230, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)4),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnNext(230, (int?)4),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)4),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_GeneralNullableMaxTest_LhsIsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_GeneralNullableMaxTest_RhsIsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)null),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_GeneralNullableMaxTest_Less()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)3),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)3),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_GeneralNullableMaxTest_Greater()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)3),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)3),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<int?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(long?)),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)4L),
                OnNext(230, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)4L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)2L),
                OnNext(230, (long?)4L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)4L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<long?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(float?)),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)4f),
                OnNext(230, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)4f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)2f),
                OnNext(230, (float?)4f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)4f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(double?)),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)4.0),
                OnNext(230, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)4.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)2.0),
                OnNext(230, (double?)4.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)4.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(decimal?)),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)4m),
                OnNext(230, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)2m),
                OnNext(230, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Max_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, "c"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(2003, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Max()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<string>(Comparer<string>.Default))
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxOfT_Reference_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ThrowingComparer<string>(ex))
            );

            res.Messages.AssertEqual(
                OnError<string>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void MaxOfT_Value_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Max(new ThrowingComparer<DateTime>(ex))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => (int)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 3L),
                OnCompleted<long>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 3.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 3.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 3.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (int?)3),
                OnCompleted<int?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (long?)3.0),
                OnCompleted<long?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)3.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)3.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Max_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)3.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void MaxOfT_Selector_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "bar"),
                OnNext(220, "qux"),
                OnNext(230, "foo"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => new string(x.Reverse().ToArray())));

            res.Messages.AssertEqual(
                OnNext(240, "xuq"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void MaxOfT_Selector_Regular_Comparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "bar"),
                OnNext(220, "qux"),
                OnNext(230, "foo"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Max(x => new string(x.Reverse().ToArray()), new ReverseComparer<string>(Comparer<string>.Default)));

            res.Messages.AssertEqual(
                OnNext(240, "oof"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region + MaxBy +

        [TestMethod]
        public void MaxBy_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(default(IObservable<int>), x => x, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, default(Func<int, int>), Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MaxBy(DummyObservable<int>.Instance, x => x, null));
        }

        [TestMethod]
        public void MaxBy_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(4, "c")),
                OnNext(230, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(4, "c"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Multiple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(215, new KeyValuePair<int, string>(2, "d")),
                OnNext(220, new KeyValuePair<int, string>(3, "c")),
                OnNext(225, new KeyValuePair<int, string>(2, "y")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnNext(235, new KeyValuePair<int, string>(4, "r")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(4, "a"),
                    new KeyValuePair<int, string>(4, "r"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }
        [TestMethod]
        public void MaxBy_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxBy_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxBy_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(4, "c")),
                OnNext(230, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MaxBy_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxBy_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MaxBy_SelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy<KeyValuePair<int, string>, int>(x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MaxBy_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MaxBy(x => x.Key, new ThrowingComparer<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region + Min +

        [TestMethod]
        public void Min_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(int)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), Comparer<DateTime>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(IComparer<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, string>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => "", Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, string>), Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), _ => "", default(IComparer<string>)));
        }

        [TestMethod]
        public void Min_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 2),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 3L),
                OnNext(220, 2L),
                OnNext(230, 4L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 3f),
                OnNext(220, 2f),
                OnNext(230, 4f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 3.0),
                OnNext(220, 2.0),
                OnNext(230, 4.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 3m),
                OnNext(220, 2m),
                OnNext(230, 4m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(int?)),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnNext(230, (int?)4),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)4),
                OnNext(230, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_GeneralNullableMinTest_LhsIsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_GeneralNullableMinTest_RhsIsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)null),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_GeneralNullableMinTest_Less()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)3),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_GeneralNullableMinTest_Greater()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)3),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(long?)),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)2L),
                OnNext(230, (long?)4L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)4L),
                OnNext(230, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(float?)),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)2f),
                OnNext(230, (float?)4f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)4f),
                OnNext(230, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(double?)),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)2.0),
                OnNext(230, (double?)4.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)4.0),
                OnNext(230, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(decimal?)),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)2m),
                OnNext(230, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)4m),
                OnNext(230, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Min_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "c"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(2003, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinOfT_Value_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinOfT_Reference_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ThrowingComparer<string>(ex))
            );

            res.Messages.AssertEqual(
                OnError<string>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void MinOfT_Value_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ThrowingComparer<DateTime>(ex))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (int)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1L),
                OnCompleted<long>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (int?)1),
                OnCompleted<int?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (long?)1.0),
                OnCompleted<long?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)1.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)1.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Min_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)1.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void MinOfT_Selector_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "qux"),
                OnNext(220, "foo"),
                OnNext(230, "bar"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => new string(x.Reverse().ToArray())));

            res.Messages.AssertEqual(
                OnNext(240, "oof"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void MinOfT_Selector_Regular_Comparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "qux"),
                OnNext(220, "foo"),
                OnNext(230, "bar"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => new string(x.Reverse().ToArray()), new ReverseComparer<string>(Comparer<string>.Default)));

            res.Messages.AssertEqual(
                OnNext(240, "xuq"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region + MinBy +

        [TestMethod]
        public void MinBy_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MinBy(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MinBy(DummyObservable<int>.Instance, default(Func<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MinBy(default(IObservable<int>), x => x, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MinBy(DummyObservable<int>.Instance, default(Func<int, int>), Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MinBy(DummyObservable<int>.Instance, x => x, null));
        }

        [TestMethod]
        public void MinBy_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "c"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Multiple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(215, new KeyValuePair<int, string>(2, "d")),
                OnNext(220, new KeyValuePair<int, string>(3, "c")),
                OnNext(225, new KeyValuePair<int, string>(2, "y")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnNext(235, new KeyValuePair<int, string>(4, "r")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "d"),
                    new KeyValuePair<int, string>(2, "y"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinBy_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinBy_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(2, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(2, "a"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(20, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual(new[] {
                    new KeyValuePair<int, string>(20, "c"),
                })),
                OnCompleted<IList<KeyValuePair<int, string>>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void MinBy_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnError<KeyValuePair<int, string>>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinBy_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z"))
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void MinBy_SelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy<KeyValuePair<int, string>, int>(x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void MinBy_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, new KeyValuePair<int, string>(1, "z")),
                OnNext(210, new KeyValuePair<int, string>(3, "b")),
                OnNext(220, new KeyValuePair<int, string>(2, "c")),
                OnNext(230, new KeyValuePair<int, string>(4, "a")),
                OnCompleted<KeyValuePair<int, string>>(250)
            );

            var res = scheduler.Start(() =>
                xs.MinBy(x => x.Key, new ThrowingComparer<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<IList<KeyValuePair<int, string>>>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        class ReverseComparer<T> : IComparer<T>
        {
            private IComparer<T> _comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return -_comparer.Compare(x, y);
            }
        }

        class ThrowingComparer<T> : IComparer<T>
        {
            private Exception _ex;

            public ThrowingComparer(Exception ex)
            {
                _ex = ex;
            }

            public int Compare(T x, T y)
            {
                throw _ex;
            }
        }

        #endregion

        #region + SequenceEqual +

        [TestMethod]
        public void SequenceEqual_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(default(IObservable<int>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(default(IObservable<int>), DummyObservable<int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, default(IObservable<int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(IEqualityComparer<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(default(IObservable<int>), new[] { 42 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, default(IEnumerable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(default(IObservable<int>), new[] { 42 }, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, default(IEnumerable<int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual<int>(DummyObservable<int>.Instance, new[] { 42 }, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void SequenceEqual_Observable_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(720, true),
                OnCompleted<bool>(720)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_Equal_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(720, true),
                OnCompleted<bool>(720)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_Left()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 0),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_Left_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 0),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_Right()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 8)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_Right_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 8)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnNext(490, 8),
                OnNext(520, 9),
                OnNext(580, 10),
                OnNext(600, 11)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 9),
                OnNext(400, 9),
                OnNext(410, 10),
                OnNext(490, 11),
                OnNext(550, 12),
                OnNext(560, 13)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(490, false),
                OnCompleted<bool>(490)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_2_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnNext(490, 8),
                OnNext(520, 9),
                OnNext(580, 10),
                OnNext(600, 11)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 9),
                OnNext(400, 9),
                OnNext(410, 10),
                OnNext(490, 11),
                OnNext(550, 12),
                OnNext(560, 13)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(490, false),
                OnCompleted<bool>(490)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(420, false),
                OnCompleted<bool>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_3_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(420, false),
                OnCompleted<bool>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_ComparerThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys, new ThrowComparer(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_ComparerThrows_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs, new ThrowComparer(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        class ThrowComparer : IEqualityComparer<int>
        {
            private Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                throw _ex;
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_NotEqual_4_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_Left_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnError<bool>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SequenceEqual_Observable_Right_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnError<bool>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7 })
            );

            res.Messages.AssertEqual(
                OnNext(510, true),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_NotEqual_Elements()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 9, 6, 7 })
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_Comparer_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3 - 2, 4, 5, 6 + 42, 7 - 6 }, new OddEvenComparer())
            );

            res.Messages.AssertEqual(
                OnNext(510, true),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_Comparer_NotEqual()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3 - 2, 4, 5 + 9, 6 + 42, 7 - 6 }, new OddEvenComparer())
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        class OddEvenComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return (obj % 2).GetHashCode();
            }
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_Comparer_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7 }, new ThrowingComparer(5, ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        class ThrowingComparer : IEqualityComparer<int>
        {
            private int _x;
            private Exception _ex;

            public ThrowingComparer(int x, Exception ex)
            {
                _x = x;
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                if (x == _x)
                    throw _ex;

                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_NotEqual_TooLong()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7, 8 })
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_NotEqual_TooShort()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6 })
            );

            res.Messages.AssertEqual(
                OnNext(450, false),
                OnCompleted<bool>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_OnError()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnError<int>(310, ex)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4 })
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_IteratorThrows1()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(Throw(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_IteratorThrows2()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(Throw(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        private IEnumerable<int> Throw(Exception ex)
        {
            yield return 3;
            throw ex;
        }

        [TestMethod]
        public void SequenceEqual_Enumerable_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new RogueEnumerable<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region + SingleAsync +

        [TestMethod]
        public void SingleAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void SingleAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(220, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void SingleAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate()
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
                xs.SingleAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(240, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_Empty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_One()
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
                xs.SingleAsync(x => x == 4)
            );

            res.Messages.AssertEqual(
                OnNext(250, 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleAsync_PredicateThrows()
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
                xs.SingleAsync(x => { if (x < 4) return false; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + SingleOrDefaultAsync +

        [TestMethod]
        public void SingleOrDefaultAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefaultAsync(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefaultAsync(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefaultAsync(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Many()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(220, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate()
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
                xs.SingleOrDefaultAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnError<int>(240, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_Empty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync(x => x % 2 == 1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_One()
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
                xs.SingleOrDefaultAsync(x => x == 4)
            );

            res.Messages.AssertEqual(
                OnNext(250, 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_None()
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
                xs.SingleOrDefaultAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SingleOrDefaultAsync(x => x > 10)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SingleOrDefaultAsync_PredicateThrows()
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
                xs.SingleOrDefaultAsync(x => { if (x < 4) return false; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + Sum +

        [TestMethod]
        public void Sum_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(int)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));
        }

        [TestMethod]
        public void Sum_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int32_Some()
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
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2 + 3 + 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int32_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, int.MaxValue),
                OnNext(220, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Sum_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnNext(220, 3L),
                OnNext(230, 4L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L + 3L + 4L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Int64_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, long.MaxValue),
                OnNext(220, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Sum_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnNext(220, 3f),
                OnNext(230, 4f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f + 3f + 4f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnNext(220, 3.0),
                OnNext(230, 4.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0 + 3.0 + 4.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnNext(220, 3m),
                OnNext(230, 4m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m + 3m + 4m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)0),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)null),
                OnNext(230, (int?)4),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)(2 + 4)),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)int.MaxValue),
                OnNext(220, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int?>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)0L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnNext(220, (long?)null),
                OnNext(230, (long?)4L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)(2L + 4L)),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)long.MaxValue),
                OnNext(220, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long?>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)0f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnNext(220, (float?)null),
                OnNext(230, (float?)4f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)(2f + 4f)),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)0.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnNext(220, (double?)null),
                OnNext(230, (double?)4.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)(2.0 + 4.0)),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)0m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnNext(220, (decimal?)null),
                OnNext(230, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)(2m + 4m)),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Sum_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (int)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6L),
                OnCompleted<long>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (int?)4),
                OnCompleted<int?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (long?)4.0),
                OnCompleted<long?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)4.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)4.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void Sum_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<string>(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)4.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region + ToArray +

        [TestMethod]
        public void ToArray_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToArray<int>(null));
        }

        [TestMethod]
        public void ToArray_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToArray()
            );

            res.Messages.AssertEqual(
                OnNext<int[]>(660, a => a.SequenceEqual(new[] { 2, 3, 4, 5 })),
                OnCompleted<int[]>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToArray_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToArray()
            );

            res.Messages.AssertEqual(
                OnError<int[]>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToArray_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToArray()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + ToDictionary +

        [TestMethod]
        public void ToDictionary_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(DummyObservable<int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToDictionary<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
        }

        [TestMethod]
        public void ToDictionary_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnNext<IDictionary<int, int>>(660, d =>
                {
                    return d.Count == 4
                        && d[4] == 8
                        && d[6] == 12
                        && d[8] == 16
                        && d[10] == 20;
                }),
                OnCompleted<IDictionary<int, int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToDictionary_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToDictionary_KeySelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => { if (x < 4) return x * 2; throw ex; }, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToDictionary_ElementSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => { if (x < 4) return x * 4; throw ex; }, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToDictionary_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ToDictionary_MultipleAdd()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToDictionary(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<IDictionary<int, int>>(440, e => true)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToDictionary_Default()
        {
            var d1 = Observable.Range(1, 10).ToDictionary(x => x.ToString()).First();
            Assert.AreEqual(7, d1["7"]);

            var d2 = Observable.Range(1, 10).ToDictionary(x => x.ToString(), x => x * 2).First();
            Assert.AreEqual(18, d2["9"]);

            var d3 = Observable.Range(1, 10).ToDictionary(x => x.ToString(), EqualityComparer<string>.Default).First();
            Assert.AreEqual(7, d3["7"]);
        }

        #endregion

        #region + ToList +

        [TestMethod]
        public void ToList_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToList<int>(null));
        }

        [TestMethod]
        public void ToList_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(660, l => l.SequenceEqual(new[] { 2, 3, 4, 5 })),
                OnCompleted<IList<int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToList_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToList_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToList()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + ToLookup +

        [TestMethod]
        public void ToLookup_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(DummyObservable<int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToLookup<int, int, int>(DummyObservable<int>.Instance, DummyFunc<int, int>.Instance, null));
        }


        [TestMethod]
        public void ToLookup_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(660)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnNext<ILookup<int, int>>(660, d =>
                {
                    return d.Count == 2
                        && d[0].SequenceEqual(new[] { 4, 8 })
                        && d[1].SequenceEqual(new[] { 6, 10 });
                }),
                OnCompleted<ILookup<int, int>>(660)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToLookup_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnError<int>(660, ex)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(660, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 660)
            );
        }

        [TestMethod]
        public void ToLookup_KeySelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => { if (x < 4) return x * 2; throw ex; }, x => x * 4, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToLookup_ElementSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x * 2, x => { if (x < 4) return x * 4; throw ex; }, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
                OnError<ILookup<int, int>>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void ToLookup_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnNext(440, 4),
                OnNext(550, 5)
            );

            var res = scheduler.Start(() =>
                xs.ToLookup(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ToLookup_Default()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();
            d1["0"].AssertEqual(2, 4, 6, 8, 10);

            var d2 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString(), x => x * 2).First();
            d2["1"].AssertEqual(2, 6, 10, 14, 18);

            var d3 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString(), EqualityComparer<string>.Default).First();
            d3["0"].AssertEqual(2, 4, 6, 8, 10);
        }

        [TestMethod]
        public void ToLookup_Contains()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();
            Assert.IsTrue(d1.Contains("1"));
            Assert.IsFalse(d1.Contains("2"));
        }

        [TestMethod]
        public void ToLookup_Groups()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            foreach (var g in d1)
            {
                if (g.Key == "0")
                {
                    g.AssertEqual(2, 4, 6, 8, 10);
                }
                else if (g.Key == "1")
                {
                    g.AssertEqual(1, 3, 5, 7, 9);
                }
                else
                    Assert.Fail("Unknown group.");
            }
        }

        [TestMethod]
        public void ToLookup_Groups_2()
        {
            var d1 = Observable.Range(1, 10).ToLookup(x => (x % 2).ToString()).First();

            foreach (IGrouping<string, int> g in ((System.Collections.IEnumerable)d1))
            {
                if (g.Key == "0")
                {
                    var l = new List<int>();
                    foreach (int v in ((System.Collections.IEnumerable)g)) l.Add(v);
                    l.AssertEqual(2, 4, 6, 8, 10);
                }
                else if (g.Key == "1")
                {
                    var l = new List<int>();
                    foreach (int v in ((System.Collections.IEnumerable)g)) l.Add(v);
                    l.AssertEqual(1, 3, 5, 7, 9);
                }
                else
                    Assert.Fail("Unknown group.");
            }
        }

        #endregion
    }
}
