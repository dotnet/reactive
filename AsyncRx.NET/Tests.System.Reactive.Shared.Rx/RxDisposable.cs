// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

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
/// <see cref="For"/> returns the same wrapper for the same real disposable, so a scenario that
/// compares two results by identity (a connectable's <c>Connect()</c> returns the same
/// connection while it is connected) sees what it would see on Rx.NET.
/// </para>
/// <para>
/// AsyncRx.NET needs no counterpart, since its subscriptions already have the shared shape.
/// </para>
/// </remarks>
internal sealed class RxDisposable : IAsyncDisposable
{
    private static readonly ConditionalWeakTable<IDisposable, RxDisposable> Wrappers = new();

    private readonly IDisposable _disposable;

    private RxDisposable(IDisposable disposable)
    {
        _disposable = disposable;
    }

    /// <summary>The wrapper for <paramref name="disposable"/>, the same one each time.</summary>
    /// <param name="disposable">The real subscription or connection.</param>
    public static RxDisposable For(IDisposable disposable)
    {
        ArgumentNullException.ThrowIfNull(disposable);

        return Wrappers.GetValue(disposable, d => new RxDisposable(d));
    }

    public ValueTask DisposeAsync()
    {
        _disposable.Dispose();
        return default;
    }
}
