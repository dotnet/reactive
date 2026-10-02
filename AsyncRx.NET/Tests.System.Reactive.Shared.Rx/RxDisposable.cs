// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Presents a sync subscription through the shared, async-shaped raw surface.</summary>
/// <remarks>
/// <para>
/// The raw surface that <see cref="IRxTarget"/> offers the shared scenarios is async-shaped,
/// because it has to be shareable with AsyncRx.NET, where a subscription is an
/// <see cref="IAsyncDisposable"/> and subscribing returns a
/// <c>ValueTask&lt;IAsyncDisposable&gt;</c>. The scenarios that subscribe by hand (in
/// <c>WindowTests</c> and <c>DelayTests</c>, for example) hold that result and dispose it
/// asynchronously at a later tick. On Rx.NET the real subscription is an <see cref="IDisposable"/>
/// with no asynchronous form, so <see cref="RxTarget"/> wraps it in one of these to meet the
/// shared signature. Disposing it disposes the real subscription synchronously and returns a
/// completed task, consistent with the rest of this target, where everything a scenario can
/// await completes synchronously.
/// </para>
/// <para>
/// AsyncRx.NET needs no counterpart, since its subscriptions already have the shared shape.
/// </para>
/// </remarks>
internal sealed class RxDisposable(IDisposable disposable) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        disposable.Dispose();
        return default;
    }
}
