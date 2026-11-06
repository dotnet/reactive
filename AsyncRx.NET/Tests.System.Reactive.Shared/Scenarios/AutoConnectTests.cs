// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>AutoConnect</c> scenarios, from Rx.NET's <c>AutoConnectTest.cs</c>.</summary>
/// <remarks>
/// All 4 of the file's tests (it has no argument-checking test): connecting at the first
/// subscriber, at once, at the second, and the connection handed to a callback and disposed
/// through it. The originals assert on <c>called</c> right after applying the operator, which
/// over a description would prove nothing, so each scenario realizes its description first;
/// where the originals block on <c>ToList().First()</c> the shared text awaits
/// <see cref="SharedReactiveTest.ToListAsync{T}"/>, their handler subscriptions are the kit's,
/// their <c>Subject</c> is the kit's, and the connection the callback receives is the kit's
/// asynchronous disposable. The list assertions are the harness's element-wise
/// <c>AssertEqual</c>.
/// </remarks>
public abstract class AutoConnectTests : SharedReactiveTest
{
    [TestMethod]
    public async Task AutoConnect_Basic()
    {
        var called = 0;
        var source = Seq.Defer(() =>
        {
            called++;
            return Seq.Range(1, 5);
        })
        .Replay()
        .AutoConnect();

        Realize(source);
        Assert.AreEqual(0, called);

        var list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);

        list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);
    }

    [TestMethod]
    public async Task AutoConnect_Immediately()
    {
        var called = 0;
        var source = Seq.Defer(() =>
        {
            called++;
            return Seq.Range(1, 5);
        })
        .Replay()
        .AutoConnect(0);

        Realize(source);
        Assert.AreEqual(1, called);

        var list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);

        list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);
    }

    [TestMethod]
    public async Task AutoConnect_TwoConsumers()
    {
        var called = 0;
        var source = Seq.Defer(() =>
        {
            called++;
            return Seq.Range(1, 5);
        })
        .Replay()
        .AutoConnect(2);

        Realize(source);
        Assert.AreEqual(0, called);

        var list0 = new List<int>();

        await source.SubscribeAsync(Scheduler, v => list0.Add(v));

        Assert.AreEqual(0, called);
        Assert.AreEqual(0, list0.Count);

        var list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);
        list0.AssertEqual(1, 2, 3, 4, 5);

        list = await ToListAsync(source);

        Assert.AreEqual(1, called);
        list.AssertEqual(1, 2, 3, 4, 5);
    }

    [TestMethod]
    public async Task AutoConnect_Dispose()
    {
        var subject = CreateSubject<int>();

        var disposable = new IAsyncDisposable?[1];

        var source = subject
        .Replay()
        .AutoConnect(1, d => disposable[0] = d);

        Realize(source);
        Assert.IsNull(disposable[0]);

        var list = new List<int>();

        await source.SubscribeAsync(Scheduler, v => list.Add(v));

        Assert.IsNotNull(disposable[0]);

        await subject.OnNextAsync(1);
        await subject.OnNextAsync(2);
        await subject.OnNextAsync(3);

        await disposable[0]!.DisposeAsync();

        await subject.OnNextAsync(4);
        await subject.OnNextAsync(5);

        list.AssertEqual(1, 2, 3);
    }
}
