// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>While</c> scenarios, from Rx.NET's <c>WhileTest.cs</c>.</summary>
/// <remarks>
/// 8 of the file's 9 tests (<c>While_ArgumentChecking</c> is the code-generated stratum). Six
/// transcribe as they are: a condition always false, always true (repeating until disposal, or
/// until the source errors, or over a source that never ends), sometimes true, and sometimes
/// throwing. <c>LoopTest1</c> and <c>LoopTest2</c> are inside <c>#if HAS_STACKTRACE</c>
/// upstream, a symbol no build defines, so they have not run there; they claim that a loop (a
/// <c>While</c> over a <c>For</c>, and a <c>While</c> over a recursive <c>Concat</c>) yields its
/// elements with the stack at a constant depth, and run here with the loop observed through
/// <c>Do</c> and awaited through <see cref="SharedReactiveTest.ToListAsync{T}"/> where the
/// originals block on <c>ForEach</c>. They showed AsyncRx.NET's loops growing the stack with
/// each iteration, fixed at Step 62.
/// </remarks>
public abstract class WhileTests : SharedReactiveTest
{
    [TestMethod]
    public void While_AlwaysFalse()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var results = scheduler.Start(() => Seq.While(() => false, xs));

        results.Messages.AssertEqual(
            OnCompleted<int>(200)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void While_AlwaysTrue()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(150, 3),
            OnNext(200, 4),
            OnCompleted<int>(250)
        );

        var results = scheduler.Start(() => Seq.While(() => true, xs));

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
    public void While_AlwaysTrue_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnError<int>(50, ex)
        );

        var results = scheduler.Start(() => Seq.While(() => true, xs));

        results.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void While_AlwaysTrue_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(50, 1)
        );

        var results = scheduler.Start(() => Seq.While(() => true, xs));

        results.Messages.AssertEqual(
            OnNext(250, 1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void While_SometimesTrue()
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

        var results = scheduler.Start(() => Seq.While(() => ++n < 3, xs));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnNext(500, 1),
            OnNext(550, 2),
            OnNext(600, 3),
            OnNext(650, 4),
            OnCompleted<int>(700)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700)
        );
    }

    [TestMethod]
    public void While_SometimesThrows()
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

        var results = scheduler.Start(() => Seq.While(() => ++n < 3 || Throw<bool>(ex), xs));

        results.Messages.AssertEqual(
            OnNext(250, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnNext(500, 1),
            OnNext(550, 2),
            OnNext(600, 3),
            OnNext(650, 4),
            OnError<int>(700, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700)
        );
    }

    [TestMethod]
    public async Task LoopTest1()
    {
        var loop = Seq.Defer(() =>
        {
            var n = 0;
            return Seq.While(
                () => n++ < 5,
                Seq.Defer(() =>
                {
                    return Seq.For(
                        Enumerable.Range(0, n),
                        x => Seq.Return(x)
                    );
                })
            );
        });

        var res = new List<int>();
        var std = new List<int>();
        await ToListAsync(loop.Do(x =>
        {
            res.Add(x);
            std.Add(new global::System.Diagnostics.StackTrace().FrameCount);
        }));

        Assert.IsTrue(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2, 0, 1, 2, 3, 0, 1, 2, 3, 4 }));
        Assert.IsTrue(std.Distinct().Count() == 1);
    }

    [TestMethod]
    public async Task LoopTest2()
    {
        var n = 0;

        Seq<int>? loop = null;
        loop = Seq.While(
            () => n++ < 10,
            Seq.Concat(
                Seq.Return(42),
                Seq.Defer(() => loop!)
            )
        );

        var res = new List<int>();
        var std = new List<int>();
        await ToListAsync(loop.Do(x =>
        {
            res.Add(x);
            std.Add(new global::System.Diagnostics.StackTrace().FrameCount);
        }));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Repeat(42, 10)));
        Assert.IsTrue(std.Distinct().Count() == 1);
    }

    private static T Throw<T>(Exception ex)
    {
        throw ex;
    }
}
