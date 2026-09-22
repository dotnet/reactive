// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Records one operation's extent in virtual time.
/// </summary>
/// <remarks>
/// <para>
/// Asynchronous operations don't necessarily complete at the same virtual time as they start. This
/// type enables the start and end times of an operation to be recorded, so that tests can assert
/// that they expect this kind of prolonged completion.
/// </para>
/// <para>
/// The <see cref="AsyncRecorded{T}"/> type uses this to describe the timing of the operation it
/// describes. <see cref="AsyncSubscription"/> uses this twice, to describe the timing of both
/// subscription and unsubscription.
/// </para>
/// <para>
/// The <see cref="Infinite"/> field is a sentinel value that can be used to indicate incomplete
/// operations. If <c>Start == Infinite</c>, this tells us that an operation never began. (For
/// example, an <see cref="AsyncSubscription"/> can indicate that the subscription was never
/// disposed by setting its <see cref="AsyncSubscription.Dispose"/> property's <see cref="Start"/>
/// to <see cref="Infinite"/>.) If <see cref="Start"/> is some real virtual time, but
/// <c>End == Infinite</c>, this represents an operation that began but never completed (e.g. an
/// observer that did not complete its handling of a notification before the end of a test).
/// </para>
/// <para>
/// An operation with <c>Start == End</c> is <em>logically instantaneous</em>; one whose completion
/// depended on something logically in the future has <c>End &gt; Start</c>, and we describe this
/// as a <em>prolonged</em> operation.
/// </para>
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct OperationTime : IEquatable<OperationTime>
{
    /// <summary>
    /// Infinite virtual time; a time that never occurred.
    /// </summary>
    /// <remarks>
    /// If <see cref="Start"/> has this value, it means the operation never began. If
    /// <see cref="Start"/> does not have this value, but <see cref="End"/> does, it means that the
    /// operation began but never completed.
    /// </remarks>
    public const long Infinite = long.MaxValue;

    /// <summary>An operation that never began.</summary>
    public static readonly OperationTime Never = new(Infinite, Infinite);

    /// <summary>
    /// Creates a new <see cref="OperationTime"/> with the given start and end times.
    /// </summary>
    /// <param name="start">The virtual time at which the operation began.</param>
    /// <param name="end">The virtual time at which the operation completed.</param>
    public OperationTime(long start, long end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Creates an uncompleted operation.
    /// </summary>
    /// <param name="start">The virtual time at which the operation began.</param>
    /// <returns>An operation that began at <paramref name="start"/> and has not (yet) completed.</returns>
    public static OperationTime StartingAt(long start) => new(start, Infinite);

    /// <summary>
    /// Returns a new <see cref="OperationTime"/> with the same start time and the given end time.
    /// </summary>
    /// <param name="end">The virtual time at which the operation completed.</param>
    /// <returns>A new <see cref="OperationTime"/> with the same start time and the given end time.</returns>
    public OperationTime CompletedAt(long end) => new(Start, end);

    /// <summary>
    /// Gets the virtual time at which the operation began, or <see cref="Infinite"/> if it never
    /// started.
    /// </summary>
    public long Start { get; }

    /// <summary>
    /// Gets the virtual time at which the operation completed, or <see cref="Infinite"/> if it
    /// never completed.
    /// </summary>
    public long End { get; }

    /// <summary>
    /// Gets a value indicating whether the operation has started.
    /// </summary>
    public bool HasStarted => Start != Infinite;

    /// <summary>
    /// Gets a value indicating whether the operation has completed.
    /// </summary>
    public bool IsComplete => HasStarted && End != Infinite;

    /// <summary>
    /// Gets a value indicating whether the operation began and completed at the same virtual time.
    /// </summary>
    /// <remarks>
    /// Operations of this kind are <em>logically instantaneous</em>. Obviously no work can be
    /// performed in zero time, but tests advance virtual time at specific controlled moments, so
    /// it is possible for an operation to complete with no virtual time passing. This corresponds
    /// to an operation that doesn't need to wait for some future event to occur before it can
    /// complete.
    /// </remarks>
    public bool IsInstantaneous => HasStarted && End == Start;

    /// <summary>
    /// Gets a value indicating whether the operation completed at a later virtual time than it started.
    /// </summary>
    public bool IsProlonged => IsComplete && End > Start;

    /// <inheritdoc/>
    public bool Equals(OperationTime other) => Start == other.Start && End == other.End;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OperationTime other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => unchecked((Start.GetHashCode() * 397) ^ End.GetHashCode());

    /// <summary>
    /// Compares two <see cref="OperationTime"/> instances for equality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(OperationTime left, OperationTime right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="OperationTime"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(OperationTime left, OperationTime right) => !left.Equals(right);

    /// <inheritdoc/>
    public override string ToString() => this switch
    {
        { HasStarted: false } => "never",
        { IsInstantaneous: true } => $"{Start}",
        { IsComplete: false } => $"{Start}→incomplete",
        _ => $"{Start}→{End}",
    };
}
