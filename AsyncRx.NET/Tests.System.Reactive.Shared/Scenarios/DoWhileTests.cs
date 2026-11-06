// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>DoWhile</c> scenarios, from Rx.NET's <c>DoWhileTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>DoWhile_ArgumentChecking</c> is the code-generated stratum), all
/// transcribed as they are: the source runs once before the condition is consulted, then
/// repeats while it holds, errors, never ends, or the condition throws.
/// </remarks>
public abstract class DoWhileTests : SharedReactiveTest
{
    [TestMethod]
    public void DoWhile_AlwaysFalse()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => false));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void DoWhile_AlwaysTrue()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => true));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnNext(500, 1),
            OnNext(550, 2),
            OnNext(600, 3),
            OnNext(650, 4),
            OnNext(750, 1),
            OnNext(800, 2),
            OnNext(850, 3),
            OnNext(900, 4)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700),
            Subscribe(700, 950),
            Subscribe(950, 1000)
        );
    }

    [TestMethod]
    public void DoWhile_AlwaysTrue_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnError<int>(50, ex)
        );

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => true));

        results.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DoWhile_AlwaysTrue_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1)
        );

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => true));

        results.Messages.AssertEqual(
            OnNext(250, 1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void DoWhile_SometimesTrue()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var n = 0;

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => ++n < 3));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnNext(500, 1),
            OnNext(550, 2),
            OnNext(600, 3),
            OnNext(650, 4),
            OnNext(750, 1),
            OnNext(800, 2),
            OnNext(850, 3),
            OnNext(900, 4),
            OnCompleted<int>(950)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700),
            Subscribe(700, 950)
        );
    }

    [TestMethod]
    public void DoWhile_SometimesThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var n = 0;

        var ex = new Exception();

        var results = scheduler.Start(() => Seq.DoWhile(xs, () => ++n < 3 || Throw<bool>(ex)));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnNext(500, 1),
            OnNext(550, 2),
            OnNext(600, 3),
            OnNext(650, 4),
            OnNext(750, 1),
            OnNext(800, 2),
            OnNext(850, 3),
            OnNext(900, 4),
            OnError<int>(950, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700),
            Subscribe(700, 950)
        );
    }

    private static T Throw<T>(Exception ex)
    {
        throw ex;
    }
}
