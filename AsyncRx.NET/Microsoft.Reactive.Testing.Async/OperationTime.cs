// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Records one operation's extent in virtual time: when it began and when it completed.
/// The containing types give the operation its meaning — a notification delivery
/// (<see cref="AsyncRecorded{T}"/>), or the subscribe/dispose processes of an
/// <see cref="AsyncSubscription"/>.
/// </summary>
/// <remarks>
/// One sentinel scheme covers the incomplete states: <c>Start == Infinite</c> means the
/// operation never began (e.g. a dispose that was never called); a real <see cref="Start"/>
/// with <c>End == Infinite</c> means it began but never completed (e.g. a delivery cut off
/// in flight). An operation with <c>Start == End</c> is <em>logically instantaneous</em>;
/// one whose completion depended on something logically in the future has <c>End &gt; Start</c>
/// — a <em>prolonged</em> completion.
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct OperationTime : IEquatable<OperationTime>
{
    /// <summary>
    /// Infinite virtual time: as <see cref="Start"/>, the operation never began; as
    /// <see cref="End"/> (with a real start), it began but never completed.
    /// </summary>
    public const long Infinite = long.MaxValue;

    /// <summary>An operation that never began.</summary>
    public static readonly OperationTime Never = new(Infinite, Infinite);

    public OperationTime(long start, long end)
    {
        Start = start;
        End = end;
    }

    /// <summary>An operation that began at <paramref name="start"/> and has not (yet) completed.</summary>
    public static OperationTime StartingAt(long start) => new(start, Infinite);

    /// <summary>This operation with its completion recorded at <paramref name="end"/>.</summary>
    public OperationTime CompletedAt(long end) => new(Start, end);

    /// <summary>Virtual time at which the operation began, or <see cref="Infinite"/>.</summary>
    public long Start { get; }

    /// <summary>Virtual time at which the operation completed, or <see cref="Infinite"/>.</summary>
    public long End { get; }

    public bool HasStarted => Start != Infinite;

    public bool IsComplete => HasStarted && End != Infinite;

    /// <summary>The operation began and completed at the same virtual time (logically instantaneous).</summary>
    public bool IsInstantaneous => HasStarted && End == Start;

    /// <summary>The operation's completion depended on something logically in the future (prolonged completion).</summary>
    public bool IsProlonged => IsComplete && End > Start;

    public bool Equals(OperationTime other) => Start == other.Start && End == other.End;

    public override bool Equals(object? obj) => obj is OperationTime other && Equals(other);

    public override int GetHashCode() => unchecked((Start.GetHashCode() * 397) ^ End.GetHashCode());

    public static bool operator ==(OperationTime left, OperationTime right) => left.Equals(right);

    public static bool operator !=(OperationTime left, OperationTime right) => !left.Equals(right);

    public override string ToString() => this switch
    {
        { HasStarted: false } => "never",
        { IsInstantaneous: true } => $"{Start}",
        { IsComplete: false } => $"{Start}→incomplete",
        _ => $"{Start}→{End}",
    };
}
