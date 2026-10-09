// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>The argument checks on AsyncRx.NET.</summary>
/// <remarks>
/// Declares the operators whose Rx.NET counterparts differ only by the word
/// <c>AsyncObservable</c> or <c>Async</c> in the name: <c>AsAsyncObservable</c>,
/// <c>ToAsyncObservable</c>, <c>SubscribeSafeAsync</c> (Rx.NET's <c>SubscribeSafe</c>), the
/// handler-based <c>SubscribeAsync</c> family on <c>AsyncObservableExtensions</c> (Rx.NET's
/// handler-based <c>Subscribe</c> family on <c>ObservableExtensions</c>), and <c>UsingAsync</c>
/// (Rx.NET's asynchronous-factory overload of <c>Using</c>, which shares that name only because
/// its cancellation-token parameters keep the overloads apart; here the name differs because
/// nothing else would).
/// </remarks>
[TestClass]
public sealed class AsyncRxArgumentCheckingTests : ArgumentCheckingTests
{
    protected override IApiSurface Surface => AsyncRxApiSurface.Instance;

    [TestMethod]
    public void AsAsyncObservable_ArgumentChecking() => Check("AsAsyncObservable");

    [TestMethod]
    public void ToAsyncObservable_ArgumentChecking() => Check("ToAsyncObservable");

    [TestMethod]
    public void SubscribeSafeAsync_ArgumentChecking() => Check("SubscribeSafeAsync");

    [TestMethod]
    public void SubscribeAsync_ArgumentChecking() => Check("SubscribeAsync");

    [TestMethod]
    public void UsingAsync_ArgumentChecking() => Check("UsingAsync");
}
