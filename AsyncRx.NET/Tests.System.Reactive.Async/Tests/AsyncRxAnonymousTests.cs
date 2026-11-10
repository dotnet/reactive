// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// AsyncRx.NET's equivalents of Rx.NET's <c>AnonymousObserver</c> tests, over its
/// <c>AsyncObserver&lt;T&gt;</c> class.
/// </summary>
/// <remarks>
/// The constructor refuses a null handler in each of its three positions, and
/// <c>OnErrorAsync</c> refuses a null error; the latter is awaited so that a refusal thrown
/// synchronously and one returned as a faulted task both pass. <c>AnonymousObservable</c>'s
/// counterpart, <c>AsyncObservable.Create</c>, is checked by the code-generated stratum.
/// </remarks>
[TestClass]
public sealed class AsyncRxAnonymousTests
{
    [TestMethod]
    public void AnonymousObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new AsyncObserver<int>(null!, _ => default, () => default));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new AsyncObserver<int>(_ => default, null!, () => default));
        Assert.ThrowsExactly<ArgumentNullException>(
            () => new AsyncObserver<int>(_ => default, _ => default, null!));
    }

    [TestMethod]
    public async Task AnonymousObserver_Error_Null()
    {
        var observer = new AsyncObserver<int>(_ => default, _ => default, () => default);
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            async () => await observer.OnErrorAsync(null!));
    }
}
