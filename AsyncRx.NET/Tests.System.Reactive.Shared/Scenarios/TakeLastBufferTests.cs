// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>TakeLastBuffer</c> scenarios, from Rx.NET's <c>TakeLastBufferTest.cs</c>.
/// </summary>
/// <remarks>
/// 22 of the file's 24 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). The count and duration forms transcribe as they are, with the original's predicate
/// expectations over the one list each result carries. The four <c>*_Default</c> tests run the
/// scheduler-less and default-scheduler forms on the real default scheduler and await the result
/// through <see cref="SharedReactiveTest.ToListAsync{T}"/>, taking its single list, where the
/// originals block on <c>SingleAsync</c> and an event.
/// </remarks>
public abstract class TakeLastBufferTests : SharedReactiveTest
{
    [TestMethod]
    public void TakeLastBuffer_Zero_Completed()
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
            xs.TakeLastBuffer(0)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(650, lst => lst.Count == 0),
            OnCompleted<IList<int>>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Zero_Error()
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
            xs.TakeLastBuffer(0)
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Zero_Disposed()
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
            xs.TakeLastBuffer(0)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_One_Completed()
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
            xs.TakeLastBuffer(1)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(650, lst => lst.SequenceEqual([9])),
            OnCompleted<IList<int>>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_One_Error()
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
            xs.TakeLastBuffer(1)
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_One_Disposed()
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
            xs.TakeLastBuffer(1)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Three_Completed()
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
            xs.TakeLastBuffer(3)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(650, lst => lst.SequenceEqual([7, 8, 9])),
            OnCompleted<IList<int>>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Three_Error()
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
            xs.TakeLastBuffer(3)
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Three_Disposed()
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
            xs.TakeLastBuffer(3)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Zero1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, lst => lst.Count == 0),
            OnCompleted<IList<int>>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Zero2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, lst => lst.Count == 0),
            OnCompleted<IList<int>>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Some1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(240, lst => lst.SequenceEqual([2, 3])),
            OnCompleted<IList<int>>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Some2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, lst => lst.Count == 0),
            OnCompleted<IList<int>>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Some3()
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
            xs.TakeLastBuffer(TimeSpan.FromTicks(45), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, lst => lst.SequenceEqual([6, 7, 8, 9])),
            OnCompleted<IList<int>>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Some4()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(250, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnCompleted<int>(350)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(350, lst => lst.Count == 0),
            OnCompleted<IList<int>>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_All()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, lst => lst.SequenceEqual([1, 2])),
            OnCompleted<IList<int>>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeLastBuffer_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task TakeLastBuffer_Default1()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).TakeLastBuffer(TimeSpan.FromSeconds(60)));

        Assert.IsTrue(res.Single().SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public async Task TakeLastBuffer_Default2()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .TakeLastBuffer(TimeSpan.FromSeconds(60), DisableOptimizations(DefaultScheduler)));

        Assert.IsTrue(res.Single().SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public async Task TakeLastBuffer_Default3()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).TakeLastBuffer(TimeSpan.Zero));

        Assert.IsEmpty(res.Single());
    }

    [TestMethod]
    public async Task TakeLastBuffer_Default4()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .TakeLastBuffer(TimeSpan.Zero, DisableOptimizations(DefaultScheduler)));

        Assert.IsEmpty(res.Single());
    }
}
