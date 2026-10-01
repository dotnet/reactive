// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Scenarios that exist to exercise nested sequences in the query model.</summary>
/// <remarks>
/// Not migrated from Rx.NET: these apply a generic operator to a sequence of windows, and nest
/// windows two deep, which the suite's own scenarios never do. They pin that the description model
/// and both targets handle any depth of nesting through the same code path.
/// </remarks>
public abstract class NestedSequenceTests : SharedReactiveTest
{
    [TestMethod]
    public void WindowWithCount_Skip_Flattened()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnCompleted<int>(260)
        );

        var res = Scheduler.Start(() =>
            xs.Window(2, 2).Skip(1).Select((w, i) => w.Select(x => i + " " + x)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(230, "0 3"),
            OnNext(240, "0 4"),
            OnNext(250, "1 5"),
            OnCompleted<string>(260)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 260)
        );
    }

    [TestMethod]
    public void WindowWithCount_OfWindows_FlattensTwice()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        Seq<Seq<Seq<int>>> windows = xs.Window(2, 2).Window(1, 1);

        var res = Scheduler.Start(() =>
            windows.Merge().Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }
}
