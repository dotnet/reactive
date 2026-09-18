// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Records a subscription to a testable async observable. Because <c>SubscribeAsync</c>
/// returns <c>ValueTask&lt;IAsyncDisposable&gt;</c> and <c>DisposeAsync</c> is also
/// asynchronous, subscription and unsubscription are each an operation with an extent in
/// virtual time — so unlike the sync <see cref="Subscription"/>'s two timestamps, this
/// records two <see cref="OperationTime"/>s (four timestamps in all).
/// </summary>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct AsyncSubscription : IEquatable<AsyncSubscription>
{
    public AsyncSubscription(OperationTime subscribe, OperationTime dispose)
    {
        Subscribe = subscribe;
        Dispose = dispose;
    }

    public AsyncSubscription(long subscribeCalled, long subscribeCompleted, long disposeCalled, long disposeCompleted)
        : this(new OperationTime(subscribeCalled, subscribeCompleted), new OperationTime(disposeCalled, disposeCompleted))
    {
    }

    /// <summary>When the <c>SubscribeAsync</c> call was made and when its task completed.</summary>
    public OperationTime Subscribe { get; }

    /// <summary>When the <c>DisposeAsync</c> call was made and when its task completed, or
    /// <see cref="OperationTime.Never"/> for a subscription that was never disposed.</summary>
    public OperationTime Dispose { get; }

    /// <summary>
    /// True when this subscription can be expressed in the sync compact form: subscribe
    /// logically instantaneous, and dispose logically instantaneous or never.
    /// </summary>
    public bool IsCompact => Subscribe.IsInstantaneous && (Dispose == OperationTime.Never || Dispose.IsInstantaneous);

    public bool Equals(AsyncSubscription other) => Subscribe == other.Subscribe && Dispose == other.Dispose;

    public override bool Equals(object? obj) => obj is AsyncSubscription other && Equals(other);

    public override int GetHashCode() => unchecked((Subscribe.GetHashCode() * 397) ^ Dispose.GetHashCode());

    public static bool operator ==(AsyncSubscription left, AsyncSubscription right) => left.Equals(right);

    public static bool operator !=(AsyncSubscription left, AsyncSubscription right) => !left.Equals(right);

    public override string ToString()
    {
        if (IsCompact)
        {
            // Print compact records the way the shared (sync) vocabulary writes them, so
            // failure output stays comparable with sync expectations.
            return Dispose == OperationTime.Never
                ? $"Subscribe({Subscribe.Start})"
                : $"Subscribe({Subscribe.Start}, {Dispose.Start})";
        }

        return $"Subscribe(called: {Format(Subscribe.Start)}, completed: {Format(Subscribe.End)}; " +
               $"Dispose called: {Format(Dispose.Start)}, completed: {Format(Dispose.End)})";
    }

    private static string Format(long time) => time == OperationTime.Infinite ? "Infinite" : time.ToString();
}
