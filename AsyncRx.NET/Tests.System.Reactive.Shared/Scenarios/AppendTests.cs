// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Append</c> scenarios, from Rx.NET's <c>AppendTest.cs</c>.</summary>
/// <remarks>
/// 7 of the file's 8 tests (<c>Append_ArgumentChecking</c> is the code-generated stratum),
/// all transcribed as they are: one value, a null value, a value on a scheduler, and chains of
/// three with and without a scheduler, each also cut short by <c>Take</c>. The null value
/// carries a null-forgiving <c>!</c>, since this project compiles with nullable reference types.
/// </remarks>
public abstract class AppendTests : SharedReactiveTest
{
    [TestMethod]
    public void Append()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(250, 3),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Append_Null()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, "1"),
            OnNext(220, "2"),
            OnCompleted<string>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(null!)
        );

        res.Messages.AssertEqual(
            OnNext(220, "2"),
            OnNext(250, (string)null!),
            OnCompleted<string>(250)
        );
    }

    [TestMethod]
    public void Append_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnCompleted<int>(ScheduledAt(250))
        );
    }

    [TestMethod]
    public void Append_Many()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3).Append(4).Append(5)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(250, 5),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Append_Many_Take()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3).Append(4).Append(5).Take(2)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(250, 3),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void Append_Many_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler).Append(4, Scheduler).Append(5, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(ScheduledAt(250, 1), 3),
            OnNext(ScheduledAt(250, 2), 4),
            OnNext(ScheduledAt(250, 3), 5),
            OnCompleted<int>(ScheduledAt(250, 3))
        );
    }

    [TestMethod]
    public void Append_Many_Take_Scheduler()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler).Append(4, Scheduler).Append(5, Scheduler).Take(2)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnCompleted<int>(ScheduledAt(250))
        );
    }
}
