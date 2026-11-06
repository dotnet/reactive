// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>If</c> scenarios, from Rx.NET's <c>IfTest.cs</c>.</summary>
/// <remarks>
/// 9 of the file's 11 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum), all transcribed as they are: the three-argument form taking each branch, a
/// condition that throws, disposal, and the two-argument form whose else branch is an empty
/// sequence, on the default scheduler or on the one given (<c>ScheduledAt(200)</c> for the
/// scheduled empty's tick).
/// </remarks>
public abstract class IfTests : SharedReactiveTest
{
    [TestMethod]
    public void If_True()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(250, 2),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(310, 3),
            OnNext(350, 4),
            OnCompleted<int>(400)
        );

        var results = scheduler.Start(() => Seq.If(() => true, xs, ys));

        results.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(250, 2),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void If_False()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(250, 2),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(310, 3),
            OnNext(350, 4),
            OnCompleted<int>(400)
        );

        var results = scheduler.Start(() => Seq.If(() => false, xs, ys));

        results.Messages.AssertEqual(
            OnNext(310, 3),
            OnNext(350, 4),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void If_Throw()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(250, 2),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(310, 3),
            OnNext(350, 4),
            OnCompleted<int>(400)
        );

        var ex = new Exception();

        var results = scheduler.Start(() => Seq.If(() => Throw<bool>(ex), xs, ys));

        results.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void If_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(250, 2)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(310, 3),
            OnNext(350, 4),
            OnCompleted<int>(400)
        );

        var results = scheduler.Start(() => Seq.If(() => true, xs, ys));

        results.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(250, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void If_Default_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnCompleted<int>(440)
        );

        var b = false;

        scheduler.ScheduleAbsolute(150, () => b = true);

        var results = scheduler.Start(() => Seq.If(() => b, xs));

        results.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(330, 3),
            OnCompleted<int>(440)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 440)
        );
    }

    [TestMethod]
    public void If_Default_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnError<int>(440, ex)
        );

        var b = false;

        scheduler.ScheduleAbsolute(150, () => b = true);

        var results = scheduler.Start(() => Seq.If(() => b, xs));

        results.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(330, 3),
            OnError<int>(440, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 440)
        );
    }

    [TestMethod]
    public void If_Default_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3)
        );

        var b = false;

        scheduler.ScheduleAbsolute(150, () => b = true);

        var results = scheduler.Start(() => Seq.If(() => b, xs));

        results.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(330, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void If_Default_Other()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnError<int>(440, new Exception())
        );

        var b = true;

        scheduler.ScheduleAbsolute(150, () => b = false);

        var results = scheduler.Start(() => Seq.If(() => b, xs));

        results.Messages.AssertEqual(
            OnCompleted<int>(200)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void If_Default_Scheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnError<int>(440, new Exception())
        );

        var results = scheduler.Start(() => Seq.If(() => false, xs, scheduler));

        results.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    private static T Throw<T>(Exception ex)
    {
        throw ex;
    }
}
