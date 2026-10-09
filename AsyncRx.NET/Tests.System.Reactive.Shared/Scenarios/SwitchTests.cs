// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Switch</c> scenarios, from Rx.NET's <c>SwitchTest.cs</c>.</summary>
/// <remarks>
/// Five of its seven tests. Deliberately not here: <c>Switch_ArgumentChecking</c> (the
/// code-generated stratum) and <c>Switch_Task</c>, which exercises the
/// <c>IObservable&lt;Task&lt;T&gt;&gt;</c> overload on real tasks and the default scheduler, and
/// has no AsyncRx.NET counterpart. Where the original carries pre-v2 expectations under
/// <c>NO_PERF</c>, the v2 ones are used.
/// </remarks>
public abstract class SwitchTests : SharedReactiveTest
{
    [TestMethod]
    public void Switch_Data()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnCompleted<int>(50)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(20, 302),
            OnNext(30, 303),
            OnNext(40, 304),
            OnCompleted<int>(150)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Switch()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 201),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnNext(510, 301),
            OnNext(520, 302),
            OnNext(530, 303),
            OnNext(540, 304),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 400)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(500, 650)
        );
    }

    [TestMethod]
    public void Switch_InnerThrows()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnError<int>(50, ex)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(20, 302),
            OnNext(30, 303),
            OnNext(40, 304),
            OnCompleted<int>(150)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Switch()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 201),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 400)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Switch_OuterThrows()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnCompleted<int>(50)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnError<Seq<int>>(500, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Switch()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 201),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnError<int>(500, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 400)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );
    }

    [TestMethod]
    public void Switch_NoInner()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<Seq<int>>(500)
        );

        var res = Scheduler.Start(() =>
            xs.Switch()
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void Switch_InnerCompletes()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnCompleted<Seq<int>>(540)
        );

        var res = Scheduler.Start(() =>
            xs.Switch()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 103),
            OnNext(420, 104),
            OnNext(510, 105),
            OnNext(520, 106),
            OnCompleted<int>(540)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 540)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 530)
        );
    }
}
