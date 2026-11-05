// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Wait</c> tests, from its <c>WaitTest.cs</c>.</summary>
/// <remarks>
/// <c>Wait</c> blocks for the last element: the blocking form of <c>LastAsync</c>, whose scenarios
/// are shared (Step 46). AsyncRx.NET has no blocking operators, so these run here against Rx.NET
/// only (the maintainer's decision, Step 46). 4 of the file's 5 tests (the argument-checking test
/// is the code-generated stratum), as the originals but for MSTest's assertions.
/// </remarks>
[TestClass]
public sealed class RxWaitTests
{
    [TestMethod]
    public void Wait_Return()
    {
        var x = 42;
        var xs = Observable.Return(x, ThreadPoolScheduler.Instance);
        var res = xs.Wait();
        Assert.AreEqual(x, res);
    }

    [TestMethod]
    public void Wait_Empty()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => Observable.Empty<int>().Wait());
    }

    [TestMethod]
    public void Wait_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.Wait()));
    }

    [TestMethod]
    public void Wait_Range()
    {
        var n = 42;
        var xs = Observable.Range(1, n, ThreadPoolScheduler.Instance);
        var res = xs.Wait();
        Assert.AreEqual(n, res);
    }
}
