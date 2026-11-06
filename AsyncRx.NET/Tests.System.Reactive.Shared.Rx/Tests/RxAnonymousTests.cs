// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>AnonymousObservable</c> and <c>AnonymousObserver</c> tests, from its
/// <c>AnonymousTest.cs</c>.
/// </summary>
/// <remarks>
/// All 3 of the file's tests, as the originals but for MSTest's assertions and <c>null!</c> and
/// <c>default!</c> where the nullable context would warn, against Rx.NET only: the argument checks
/// of the two anonymous types' constructors, and a null error refused. AsyncRx.NET's counterparts,
/// <c>AsyncObservable.Create</c> (the code-generated stratum) and the
/// <c>AsyncObserver&lt;T&gt;</c> class, have their equivalents in <c>AsyncRxAnonymousTests</c>.
/// </remarks>
[TestClass]
public sealed class RxAnonymousTests
{
    [TestMethod]
    public void AnonymousObservable_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObservable<int>(null!));
    }

    [TestMethod]
    public void AnonymousObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(default!));

        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(default!, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action)!));

        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(default!, ex => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action<Exception>)!));

        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(default!, ex => { }, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default!, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, ex => { }, default!));
    }

    [TestMethod]
    public void AnonymousObserver_Error_Null()
    {
        var observer = new AnonymousObserver<int>(_ => { }, e => { }, () => { });
        Assert.ThrowsExactly<ArgumentNullException>(() => observer.OnError(null!));
    }
}
