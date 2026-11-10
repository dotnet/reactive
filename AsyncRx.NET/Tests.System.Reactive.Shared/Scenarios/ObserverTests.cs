// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>ObserverTest</c>, shared.</summary>
/// <remarks>
/// 4 of the file's 42 tests, transcribed as they are: the observer built by <c>CreateObserver</c>
/// from one handler or three, which is the library's own <c>Create</c>, and pushed into directly,
/// with each push awaited. What they claim is the library's defaults: with one handler an error is
/// thrown back at the caller (Rx.NET throws it from <c>OnError</c>, AsyncRx.NET returns it as a
/// faulted task, and awaiting the push sees either) and a completion is accepted silently; with
/// three, each notification reaches its handler and a terminal one is the last. The other 38 are
/// the Rx.NET runner's: eleven argument checks and the two-handler <c>Create</c> forms, and the
/// members AsyncRx.NET does not have (<c>ToObserver</c>, <c>ToNotifier</c>, <c>AsObserver</c>,
/// <c>Checked</c>, <c>NotifyOn</c>, <c>ToProgress</c>, and <c>Synchronize</c> over a monitor or an
/// <c>AsyncLock</c>); the AsyncRx.NET runner carries its own equivalents for argument checks, for
/// <c>Synchronize</c>, and for what <c>Checked</c> claims, which its plain observer does by
/// itself.
/// </remarks>
public abstract class ObserverTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Create_OnNext()
    {
        var next = false;
        var res = CreateObserver<int>(x => { Assert.AreEqual(42, x); next = true; });
        await res.OnNextAsync(42);
        Assert.IsTrue(next);
        await res.OnCompletedAsync();
    }

    [TestMethod]
    public async Task Create_OnNext_HasError()
    {
        var ex = new Exception();
        var e_ = default(Exception);

        var next = false;
        var res = CreateObserver<int>(x => { Assert.AreEqual(42, x); next = true; });
        await res.OnNextAsync(42);
        Assert.IsTrue(next);

        try
        {
            await res.OnErrorAsync(ex);
            Assert.Fail();
        }
        catch (Exception e)
        {
            e_ = e;
        }
        Assert.AreSame(ex, e_);
    }

    [TestMethod]
    public async Task Create_OnNextOnErrorOnCompleted1()
    {
        var ex = new Exception();
        var next = true;
        var error = false;
        var completed = false;
        var res = CreateObserver<int>(
            x => { Assert.AreEqual(42, x); next = true; },
            e => { Assert.AreSame(ex, e); error = true; },
            () => { completed = true; });
        await res.OnNextAsync(42);
        Assert.IsTrue(next);
        Assert.IsFalse(error);
        Assert.IsFalse(completed);
        await res.OnCompletedAsync();
        Assert.IsTrue(completed);
        Assert.IsFalse(error);
    }

    [TestMethod]
    public async Task Create_OnNextOnErrorOnCompleted2()
    {
        var ex = new Exception();
        var next = true;
        var error = false;
        var completed = false;
        var res = CreateObserver<int>(
            x => { Assert.AreEqual(42, x); next = true; },
            e => { Assert.AreSame(ex, e); error = true; },
            () => { completed = true; });
        await res.OnNextAsync(42);
        Assert.IsTrue(next);
        Assert.IsFalse(error);
        Assert.IsFalse(completed);
        await res.OnErrorAsync(ex);
        Assert.IsTrue(error);
        Assert.IsFalse(completed);
    }
}
