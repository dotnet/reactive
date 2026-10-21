// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Prepend</c> scenarios, from Rx.NET's <c>PrependTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>Prepend_ArgumentChecking</c> is the code-generated stratum),
/// all transcribed as they are: one value, a value on a scheduler, and chains of three with and
/// without a scheduler, each also cut short by <c>Take</c>; the scheduled ticks are
/// <c>ScheduledAt</c> hops, as the <c>Repeat</c> scenarios spell them.
/// </remarks>
public abstract class PrependTests : SharedReactiveTest
{
    [TestMethod]
    public void Prepend()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3)
        );

        res.Messages.AssertEqual(
            OnNext(200, 3),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Prepend_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 3),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Prepend_Many()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3).Prepend(4).Prepend(5)
        );

        res.Messages.AssertEqual(
            OnNext(200, 5),
            OnNext(200, 4),
            OnNext(200, 3),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Prepend_Many_Take()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3).Prepend(4).Prepend(5).Take(2)
        );

        res.Messages.AssertEqual(
            OnNext(200, 5),
            OnNext(200, 4),
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void Prepend_Many_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3, Scheduler).Prepend(4, Scheduler).Prepend(5, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 5),
            OnNext(ScheduledAt(200, 2), 4),
            OnNext(ScheduledAt(200, 3), 3),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Prepend_Many_Take_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(3, Scheduler).Prepend(4, Scheduler).Prepend(5, Scheduler).Take(2)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 5),
            OnNext(ScheduledAt(200, 2), 4),
            OnCompleted<int>(ScheduledAt(200, 2))
        );
    }
}
