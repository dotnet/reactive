// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SkipLast</c> scenarios, from Rx.NET's <c>SkipLastTest.cs</c>.</summary>
/// <remarks>
/// 20 of the file's 22 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). The count and duration forms transcribe as they are. The four <c>*_Default</c> tests
/// run the scheduler-less and default-scheduler forms on the real default scheduler and await
/// the result through <see cref="SharedReactiveTest.ToListAsync{T}"/> where the originals block
/// on an event.
/// </remarks>
public abstract class SkipLastTests : SharedReactiveTest
{
    [TestMethod]
    public void SkipLast_Zero_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(0)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_Zero_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(0)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_Zero_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(0)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void SkipLast_One_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(1)
        );

        res.Messages.AssertEqual(
            OnNext(250, 2),
            OnNext(270, 3),
            OnNext(310, 4),
            OnNext(360, 5),
            OnNext(380, 6),
            OnNext(410, 7),
            OnNext(590, 8),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_One_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(1)
        );

        res.Messages.AssertEqual(
            OnNext(250, 2),
            OnNext(270, 3),
            OnNext(310, 4),
            OnNext(360, 5),
            OnNext(380, 6),
            OnNext(410, 7),
            OnNext(590, 8),
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_One_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(1)
        );

        res.Messages.AssertEqual(
            OnNext(250, 2),
            OnNext(270, 3),
            OnNext(310, 4),
            OnNext(360, 5),
            OnNext(380, 6),
            OnNext(410, 7),
            OnNext(590, 8)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void SkipLast_Three_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(3)
        );

        res.Messages.AssertEqual(
            OnNext(310, 2),
            OnNext(360, 3),
            OnNext(380, 4),
            OnNext(410, 5),
            OnNext(590, 6),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_Three_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(3)
        );

        res.Messages.AssertEqual(
            OnNext(310, 2),
            OnNext(360, 3),
            OnNext(380, 4),
            OnNext(410, 5),
            OnNext(590, 6),
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void SkipLast_Three_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(3)
        );

        res.Messages.AssertEqual(
            OnNext(310, 2),
            OnNext(360, 3),
            OnNext(380, 4),
            OnNext(410, 5),
            OnNext(590, 6)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void SkipLast_Zero1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipLast_Zero2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipLast_Some1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.FromTicks(15), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(230, 1),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipLast_Some2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnNext(270, 7),
            OnNext(280, 8),
            OnNext(290, 9),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.FromTicks(45), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(270, 2),
            OnNext(280, 3),
            OnNext(290, 4),
            OnNext(300, 5),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SkipLast_All()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipLast_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SkipLast_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task SkipLast_Default1()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).SkipLast(TimeSpan.FromSeconds(60)));

        Assert.IsEmpty(res);
    }

    [TestMethod]
    public async Task SkipLast_Default2()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .SkipLast(TimeSpan.FromSeconds(60), DisableOptimizations(DefaultScheduler)));

        Assert.IsEmpty(res);
    }

    [TestMethod]
    public async Task SkipLast_Default3()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).SkipLast(TimeSpan.Zero));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public async Task SkipLast_Default4()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .SkipLast(TimeSpan.Zero, DisableOptimizations(DefaultScheduler)));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
    }
}
