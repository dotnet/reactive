// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own task-like support tests, from its <c>TaskLikeSupportTest.cs</c>.</summary>
/// <remarks>
/// All 4 of the file's tests, as the originals but for MSTest's assertions, against Rx.NET only.
/// They exercise the compiler's task-like support for <c>ITaskObservable&lt;T&gt;</c>: an
/// <c>async</c> method returning it, through Rx.NET's <c>TaskObservableMethodBuilder</c>, that
/// returns a value, throws, and awaits observables and tasks in turn. AsyncRx.NET carries an
/// <c>AsyncObservableMethodBuilder</c> but attaches it to no public type, so there is no <c>async
/// IAsyncObservable&lt;T&gt;</c> to write the equivalent against. The two <c>Basics</c> tests run
/// the same method with and without a <c>SynchronizationContext</c>, since <c>AsyncSubject</c>'s
/// awaiter resumes through the context when there is one and inline when there is not; the
/// original's comment records that one is set explicitly to null because a test elsewhere once
/// left a UI context behind, which would hang the completion. The <c>CS1998</c> suppressions are
/// the original's, for the two methods that await nothing.
/// </remarks>
[TestClass]
public sealed class RxTaskLikeSupportTests
{
    [TestMethod]
    public async Task Return()
    {
        Assert.AreEqual(42, await ManOrBoy_Return());
    }

    [TestMethod]
    public async Task Throw()
    {
        await Assert.ThrowsExactlyAsync<DivideByZeroException>(
            async () => await ManOrBoy_Throw(42, 0));
    }

    [TestMethod]
    public async Task BasicsNoSynchronizationContext()
    {
        var ctx = SynchronizationContext.Current;
        try
        {
            SynchronizationContext.SetSynchronizationContext(null);
            Assert.AreEqual(45, await ManOrBoy_Basics());
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(ctx);
        }
    }

    [TestMethod]
    public async Task BasicsWithSynchronizationContext()
    {
        var ctx = SynchronizationContext.Current;
        try
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            Assert.AreEqual(45, await ManOrBoy_Basics());
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(ctx);
        }
    }

    private async ITaskObservable<int> ManOrBoy_Return()
    {
        return 42;
    }

    private async ITaskObservable<int> ManOrBoy_Throw(int n, int d)
    {
        return n / d;
    }

    private async ITaskObservable<int> ManOrBoy_Basics()
    {
        var res = 0;

        for (var i = 0; i < 10; i++)
        {
            switch (i % 4)
            {
                case 0:
                    res += await Observable.Return(i);
                    break;
                case 1:
                    res += await Observable.Return(i).Delay(TimeSpan.FromMilliseconds(50));
                    break;
                case 2:
                    res += await Task.FromResult(i);
                    break;
                case 3:
                    res += await Task.Run(() => { Task.Delay(50).Wait(); return i; });
                    break;
            }
        }

        return res;
    }
}
