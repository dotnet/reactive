// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Scan</c> scenarios, from Rx.NET's <c>ScanTest.cs</c>.</summary>
/// <remarks>
/// 12 of the file's 13 tests (the <c>Scan_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: the seeded and seedless forms over never-ending,
/// empty, single, failing and several-element sources, with an accumulator that throws. Unlike
/// <c>Aggregate</c>, an empty source without a seed simply completes.
/// </remarks>
public abstract class ScanTests : SharedReactiveTest
{
    [TestMethod]
    public void Scan_Seed_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable<int>();

        var seed = 42;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Scan_Seed_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var seed = 42;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_Seed_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var seed = 42;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnNext(220, seed + 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_Seed_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();
        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var seed = 42;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_Seed_SomeData()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var seed = 1;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnNext(210, seed + 2),
            OnNext(220, seed + 2 + 3),
            OnNext(230, seed + 2 + 3 + 4),
            OnNext(240, seed + 2 + 3 + 4 + 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_Seed_AccumulatorThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var ex = new Exception();
        var seed = 1;
        var res = scheduler.Start(() =>
            xs.Scan(seed, (acc, x) => { if (x == 4) { throw ex; } return acc + x; })
        );

        res.Messages.AssertEqual(
            OnNext(210, seed + 2),
            OnNext(220, seed + 2 + 3),
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable<int>();

        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();
        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_SomeData()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => acc + x)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 2 + 3),
            OnNext(230, 2 + 3 + 4),
            OnNext(240, 2 + 3 + 4 + 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Scan_NoSeed_AccumulatorThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var ex = new Exception();
        var res = scheduler.Start(() =>
            xs.Scan((acc, x) => { if (x == 4) { throw ex; } return acc + x; })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 2 + 3),
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}
