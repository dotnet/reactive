// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SkipUntil</c> scenarios, from Rx.NET's <c>SkipUntilTest.cs</c>.</summary>
/// <remarks>
/// 21 of the file's 23 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Nineteen transcribe as they are: the other-sequence form and the start-time form,
/// including the two that chain two start times. <c>SkipUntil_HasCompletedCausesDisposal</c>
/// writes the original's <c>Observable.Create</c> with an action-returning callback as
/// <c>Seq.Create</c> with the same callback in its <c>ValueTask</c> form. <c>SkipUntil_Default</c>
/// runs the scheduler-less start-time form on the real default scheduler and awaits the result
/// through <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on an event.
/// </remarks>
public abstract class SkipUntilTests : SharedReactiveTest
{
    [TestMethod]
    public void SkipUntil_SomeData_Next()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4), //!
            OnNext(240, 5), //!
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(225, 99),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_SomeData_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(225, ex)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(225, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_Error_SomeData()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(220, ex)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(230, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void SkipUntil_SomeData_Empty()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_Never_Next()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(225, 2), //!
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_Never_Error1()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(225, ex)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(225, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_SomeData_Error2()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SkipUntil_SomeData_Never()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(
                200,
                1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
        );
    }

    [TestMethod]
    public void SkipUntil_Never_Empty()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void SkipUntil_Never_Never()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void SkipUntil_HasCompletedCausesDisposal()
    {
        var scheduler = Scheduler;

        var disposed = false;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = Seq.Create<int>(obs => new ValueTask<Action?>(() => { disposed = true; }));

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
        );

        Assert.IsTrue(disposed, "disposed");
    }

    [TestMethod]
    public void SkipUntil_Immediate()
    {
        var scheduler = Scheduler;

        var xs = Seq.Return(1);
        var ys = Seq.Return("bar");

        var res = scheduler.Start(() =>
            xs.SkipUntil(ys)
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void SkipUntil_Empty_Empty()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnCompleted(250, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnCompleted(250, 1)
        );

        var res = scheduler.Start(() =>
            l.SkipUntil(r)
        );

        res.Messages.AssertEqual(
            );

        //Desired behaviour:
        //res.Messages.AssertEqual(
        //    OnCompleted(250, 1));
    }

    [TestMethod]
    public void SkipUntil_Zero()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(), scheduler)
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
    public void SkipUntil_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipUntil_Late()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void SkipUntil_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SkipUntil_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void SkipUntil_Twice1()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
                .SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void SkipUntil_Twice2()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            xs.SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
                .SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public async Task SkipUntil_Default()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).SkipUntil(DateTimeOffset.UtcNow.AddMinutes(1)));

        Assert.IsEmpty(res);
    }
}
