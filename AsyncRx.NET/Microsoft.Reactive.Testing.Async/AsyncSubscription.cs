// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Records information about subscriptions to and unsubscriptions from asynchronous observable
/// sequences.
/// </summary>
/// <remarks>
/// <para>
/// This is the async counterpart of <see cref="Subscription"/>. Asynchronous operations don't
/// necessarily complete at the same virtual time as they start, which is why
/// <see cref="Subscribe"/> and <see cref="Dispose"/> have type <see cref="OperationTime"/>,
/// enabling this type to defines start and end times for each operation.
/// </para>
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct AsyncSubscription : IEquatable<AsyncSubscription>
{
    /// <summary>
    /// Creates an <see cref="AsyncSubscription"/> with the given subscription and unsubscription
    /// timing.
    /// </summary>
    /// <param name="subscribe">The subscription start and end times.</param>
    /// <param name="dispose">The unsubscription start and end times.</param>
    public AsyncSubscription(OperationTime subscribe, OperationTime dispose)
    {
        Subscribe = subscribe;
        Dispose = dispose;
    }

    /// <summary>
    /// Creates an <see cref="AsyncSubscription"/> with the given subscription and unsubscription
    /// timing.
    /// </summary>
    /// <param name="subscribeCalled">
    /// The virtual time at which <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/> was called.
    /// </param>
    /// <param name="subscribeCompleted">The virtual time at which the subscription completed.</param>
    /// <param name="disposeCalled">
    /// The virtual time at which <see cref="IAsyncDisposable.DisposeAsync"/> was called on the object
    /// returned by <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/>.
    /// </param>
    /// <param name="disposeCompleted">The virtual time at which the unsubscription completed.</param>
    public AsyncSubscription(long subscribeCalled, long subscribeCompleted, long disposeCalled, long disposeCompleted)
        : this(new OperationTime(subscribeCalled, subscribeCompleted), new OperationTime(disposeCalled, disposeCompleted))
    {
    }

    /// <summary>
    /// Gets the timing of the <c>SubscribeAsync</c> call.
    /// </summary>
    public OperationTime Subscribe { get; }

    /// <summary>
    /// Gets the timing of the <c>DisposeAsync</c> call that ended the subscription.
    /// </summary>
    public OperationTime Dispose { get; }

    /// <summary>
    /// Gets a value indicating whether this subscription could be represented in the compact form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An <see cref="AsyncSubscription"/> is considered compact if it can be fully represented as
    /// a <see cref="Subscription"/>.
    /// </para>
    /// <para>
    /// This is true when this subscription is logically instantaneous (starts and completes in the
    /// same virtual tick), and dispose either is logically instantaneous or never occurred.
    /// </para>
    /// </remarks>
    public bool IsCompact => Subscribe.IsInstantaneous && (Dispose == OperationTime.Never || Dispose.IsInstantaneous);

    /// <inheritdoc/>
    public bool Equals(AsyncSubscription other) => Subscribe == other.Subscribe && Dispose == other.Dispose;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AsyncSubscription other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => unchecked((Subscribe.GetHashCode() * 397) ^ Dispose.GetHashCode());

    /// <summary>
    /// Compares two <see cref="AsyncSubscription"/> instances for equality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(AsyncSubscription left, AsyncSubscription right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="AsyncSubscription"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(AsyncSubscription left, AsyncSubscription right) => !left.Equals(right);

    /// <inheritdoc/>
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
