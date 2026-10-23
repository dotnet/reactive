// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>The argument checks on Rx.NET.</summary>
/// <remarks>
/// Declares the operators whose AsyncRx.NET counterparts differ only by the word
/// <c>Observable</c> or <c>Async</c> in the name: <c>AsObservable</c>, <c>ToObservable</c> and
/// <c>SubscribeSafe</c>, and the handler-based <c>Subscribe</c> family on
/// <c>ObservableExtensions</c> (AsyncRx.NET's <c>SubscribeAsync</c>). The enumerable
/// <c>Subscribe</c> on <c>Observable</c> is a different thing, and the shared class declares it.
/// </remarks>
[TestClass]
public sealed class RxArgumentCheckingTests : ArgumentCheckingTests
{
    protected override IApiSurface Surface => RxApiSurface.Instance;

    [TestMethod]
    public void AsObservable_ArgumentChecking() => Check("AsObservable");

    [TestMethod]
    public void ToObservable_ArgumentChecking() => Check("ToObservable");

    [TestMethod]
    public void SubscribeSafe_ArgumentChecking() => Check("SubscribeSafe");

    [TestMethod]
    public void Subscribe_Extensions_ArgumentChecking() => Check("Subscribe", SurfaceClass.Extensions);
}
