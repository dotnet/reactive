// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>StartWith</c> scenarios, from Rx.NET's <c>StartWithTest.cs</c>.</summary>
/// <remarks>
/// 4 of the file's 5 tests (<c>StartWith_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: the <c>params</c> and enumerable forms, each with and
/// without a scheduler; the scheduled ticks are <c>ScheduledAt</c> hops, as the <c>Repeat</c>
/// scenarios spell them.
/// </remarks>
public abstract class StartWithTests : SharedReactiveTest
{
    [TestMethod]
    public void StartWith()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.StartWith(1)
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void StartWith_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.StartWith(Scheduler, 1, 2, 3)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 2),
            OnNext(ScheduledAt(200, 3), 3),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void StartWith_Enumerable()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );

        List<int> data = [1, 2, 3];
        var res = Scheduler.Start(() =>
            xs.StartWith(data)
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(200, 2),
            OnNext(200, 3),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void StartWith_Enumerable_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );

        List<int> data = [1, 2, 3];
        var res = Scheduler.Start(() =>
            xs.StartWith(Scheduler, data)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 2),
            OnNext(ScheduledAt(200, 3), 3),
            OnNext(220, 4),
            OnCompleted<int>(250)
        );
    }
}
