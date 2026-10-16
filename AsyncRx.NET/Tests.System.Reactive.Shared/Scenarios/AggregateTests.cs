// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Aggregate</c> scenarios, from Rx.NET's <c>AggregateTest.cs</c>.</summary>
/// <remarks>
/// 19 of the file's 20 tests (the <c>Aggregate_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: the seeded, seeded-with-result and seedless forms over
/// empty, single, failing, never-ending and several-element sources, with an accumulator or
/// result selector that throws. An empty source without a seed fails with
/// <c>InvalidOperationException</c>, which the original asserts by predicate and both targets
/// deliver.
/// </remarks>
public abstract class AggregateTests : SharedReactiveTest
{
    [TestMethod]
    public void AggregateWithSeed_Empty()
    {
        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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

    [TestMethod]
    public void AggregateWithSeed_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

    [TestMethod]
    public void AggregateWithSeedAndResult_Empty()
    {
        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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

    [TestMethod]
    public void AggregateWithSeedAndResult_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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

    [TestMethod]
    public void AggregateWithSeedAndResult_ResultSelectorThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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

    [TestMethod]
    public void AggregateWithoutSeed_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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
        var scheduler = Scheduler;

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

        var scheduler = Scheduler;

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
