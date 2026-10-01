// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Merge</c> scenarios.</summary>
/// <remarks>
/// So far only the scenario that exercises a hot source of observables, from Rx.NET's
/// <c>MergeTest.cs</c>; the rest of that file is still to be migrated.
/// </remarks>
public abstract class MergeTests : SharedReactiveTest
{
    [TestMethod]
    public void Merge_ObservableOfObservable_Data()
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
            OnNext(120, 305),
            OnCompleted<int>(150)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Merge()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 103),
            OnNext(410, 201),
            OnNext(420, 104),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnNext(510, 105),
            OnNext(510, 301),
            OnNext(520, 106),
            OnNext(520, 302),
            OnNext(530, 303),
            OnNext(540, 304),
            OnNext(620, 305),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 530)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(500, 650)
        );
    }
}
