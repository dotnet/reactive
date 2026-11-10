// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Let</c> tests, from its <c>LetTest.cs</c>.</summary>
/// <remarks>
/// <c>Let</c> applies a selector to the source once, at the call, for a fluent style of reusing a
/// sequence without sharing its subscriptions. Rx.NET marks it <c>[Experimental]</c>, in
/// <c>System.Reactive.Linq.ObservableEx</c>; AsyncRx.NET has no experimental surface and nothing
/// of this shape, so these run here against Rx.NET only (the maintainer's decision, Step 87: the
/// suite keeps the upstream coverage). <c>ObservableEx</c> is outside the code-generated stratum,
/// so the argument-checking test comes too, with the upstream <c>Dummy*</c> doubles replaced
/// inline. Both of the file's tests, as the originals but for MSTest's assertions.
/// </remarks>
[TestClass]
public sealed class RxLetTests
{
    [TestMethod]
    public void Let_ArgumentChecking()
    {
        var someObservable = Observable.Empty<int>();

        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Let(default(IObservable<int>)!, x => x));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Let<int, int>(someObservable, null!));
    }

    [TestMethod]
    public void Let_CallsFunctionImmediately()
    {
        var called = false;
        Observable.Empty<int>().Let(x => { called = true; return x; });
        Assert.IsTrue(called);
    }
}
