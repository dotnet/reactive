// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ToArray</c> scenarios, from Rx.NET's <c>ToArrayTest.cs</c>.</summary>
/// <remarks>
/// 3 of the file's 4 tests (<c>ToArray_ArgumentChecking</c> is the code-generated stratum),
/// all transcribed as they are: completion, failure and disposal before completion.
/// </remarks>
public abstract class ToArrayTests : SharedReactiveTest
{
    [TestMethod]
    public void ToArray_Completed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(660)
        );

        var res = Scheduler.Start(() =>
            xs.ToArray()
        );

        res.Messages.AssertEqual(
            OnNext<int[]>(660, a => a.SequenceEqual([2, 3, 4, 5])),
            OnCompleted<int[]>(660)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 660)
        );
    }

    [TestMethod]
    public void ToArray_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnError<int>(660, ex)
        );

        var res = Scheduler.Start(() =>
            xs.ToArray()
        );

        res.Messages.AssertEqual(
            OnError<int[]>(660, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 660)
        );
    }

    [TestMethod]
    public void ToArray_Disposed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5)
        );

        var res = Scheduler.Start(() =>
            xs.ToArray()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }
}
