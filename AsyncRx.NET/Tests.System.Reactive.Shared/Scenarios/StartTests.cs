// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Start</c> scenarios, from Rx.NET's <c>StartTest.cs</c>.</summary>
/// <remarks>
/// 7 of the file's 8 tests (<c>Start_ArgumentChecking</c> is the code-generated stratum). The
/// two virtual-time tests transcribe as they are; the real-time ones await
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the originals block on
/// <c>ToEnumerable</c>, and <c>Start_ActionErrorAfterUnsubscribeDoesNotThrow</c> keeps its
/// barrier handshake over the kit's handler subscription.
/// </remarks>
public abstract class StartTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Start_Action()
    {
        var done = false;
        Assert.IsTrue(
            (await ToListAsync(Seq.Start(() => { done = true; }))).SequenceEqual([new Unit()]));
        Assert.IsTrue(done, "done");
    }

    [TestMethod]
    public void Start_Action2()
    {
        var scheduler = Scheduler;

        var done = false;

        var res = scheduler.Start(() =>
            Seq.Start(() => { done = true; }, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(200, new Unit()),
            OnCompleted<Unit>(200)
        );

        Assert.IsTrue(done, "done");
    }

    [TestMethod]
    public async Task Start_ActionError()
    {
        var ex = new Exception();

        var res = await ToListAsync(Seq.Start(() => { throw ex; }).Materialize());

        Assert.IsTrue(res.SequenceEqual([
            Notification.CreateOnError<Unit>(ex)
        ]));
    }

    [TestMethod]
    public async Task Start_ActionErrorAfterUnsubscribeDoesNotThrow()
    {
        var ex = new Exception();
        using Barrier gate = new(2);

        var sub = await Seq.Start(
            () =>
            {
                // 1: action running
                gate.SignalAndWait(TestContext.CancellationToken);
                // 2: unsubscribe Dispose returned
                gate.SignalAndWait(TestContext.CancellationToken);
                throw ex;
            })
            .SubscribeAsync(Scheduler, _ => { });

        // 1: action running
        gate.SignalAndWait(TestContext.CancellationToken);

        await sub.DisposeAsync();

        // 2: unsubscribe Dispose returned
        gate.SignalAndWait(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task Start_Func()
    {
        var res = await ToListAsync(Seq.Start(() => 1));

        Assert.IsTrue(res.SequenceEqual([
            1
        ]));
    }

    [TestMethod]
    public void Start_Func2()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Start(() => 1, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public async Task Start_FuncError()
    {
        var ex = new Exception();

        var res = await ToListAsync(Seq.Start<int>(() => { throw ex; }).Materialize());

        Assert.IsTrue(res.SequenceEqual([
            Notification.CreateOnError<int>(ex)
        ]));
    }

    /// <summary>MSTest's context, for its cancellation token.</summary>
    public TestContext TestContext { get; set; } = null!;
}
