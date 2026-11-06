// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Finally</c> scenarios, from Rx.NET's <c>FinallyTest.cs</c>.</summary>
/// <remarks>
/// 9 of the file's 10 tests (the <c>Finally_ArgumentChecking</c> test is the code-generated
/// stratum). Four transcribe as they are, the virtual-time tests over a never-ending source
/// (where the action runs on the unsubscribe at the end of the run), an empty, a single-element
/// and a failing one. The two <c>*_OnlyCalledOnce_*</c> tests subscribe through the harness
/// and dispose the subscription twice, as the originals do with a bare <c>Subscribe()</c> and
/// <c>Dispose()</c>; the three <c>*_DisposeOrder_*</c> tests chain three actions over an empty,
/// a single-element and a never-ending source and expect them innermost first, likewise.
/// </remarks>
public abstract class FinallyTests : SharedReactiveTest
{
    [TestMethod]
    public void Finally_Never()
    {
        var scheduler = Scheduler;

        var invoked = false;
        var res = scheduler.Start(() =>
            Seq.Never<int>().Finally(() => { invoked = true; })
        );

        res.Messages.AssertEqual(
        );

        Assert.IsTrue(invoked); // due to unsubscribe; see 1356
    }

    [TestMethod]
    public async Task Finally_OnlyCalledOnce_Never()
    {
        var invokeCount = 0;
        var someObservable = Seq.Never<int>().Finally(() => { invokeCount++; });
        var d = await someObservable.SubscribeAsync(Scheduler, _ => { });
        await d.DisposeAsync();
        await d.DisposeAsync();
        Assert.AreEqual(1, invokeCount);
    }

    [TestMethod]
    public async Task Finally_OnlyCalledOnce_Empty()
    {
        var invokeCount = 0;
        var someObservable = Seq.Empty<int>().Finally(() => { invokeCount++; });
        var d = await someObservable.SubscribeAsync(Scheduler, _ => { });
        await d.DisposeAsync();
        await d.DisposeAsync();
        Assert.AreEqual(1, invokeCount);
    }

    [TestMethod]
    public void Finally_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var invoked = false;
        var res = scheduler.Start(() =>
            xs.Finally(() => { invoked = true; })
        );

        Assert.IsTrue(invoked);

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Finally_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var invoked = false;
        var res = scheduler.Start(() =>
            xs.Finally(() => { invoked = true; })
        );

        Assert.IsTrue(invoked);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Finally_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();
        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var invoked = false;
        var res = scheduler.Start(() =>
            xs.Finally(() => { invoked = true; })
        );

        Assert.IsTrue(invoked);

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public async Task Finally_DisposeOrder_Empty()
    {
        var order = "";
        await Seq
            .Empty<Unit>()
            .Finally(() => order += "1")
            .Finally(() => order += "2")
            .Finally(() => order += "3")
            .SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual("123", order);
    }

    [TestMethod]
    public async Task Finally_DisposeOrder_Return()
    {
        var order = "";
        await Seq
            .Return(Unit.Default)
            .Finally(() => order += "1")
            .Finally(() => order += "2")
            .Finally(() => order += "3")
            .SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual("123", order);
    }

    [TestMethod]
    public async Task Finally_DisposeOrder_Never()
    {
        var order = "";
        var d = await Seq
            .Never<Unit>()
            .Finally(() => order += "1")
            .Finally(() => order += "2")
            .Finally(() => order += "3")
            .SubscribeAsync(Scheduler, _ => { });
        await d.DisposeAsync();
        Assert.AreEqual("123", order);
    }
}
