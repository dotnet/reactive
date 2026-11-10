// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Repeat(value, repeatCount)</c> or
/// <c>Seq.Repeat(value, repeatCount, scheduler)</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Repeat{T}(T, int)"/> and
/// <see cref="Seq.Repeat{T}(T, int, SchedulerRef)"/>; materialized by each target through
/// <see cref="ISeqVisitor.RepeatValueCount{T}(RepeatValueCountSeq{T})"/>.
/// </remarks>
public sealed class RepeatValueCountSeq<T>(T value, int repeatCount, SchedulerRef? scheduler = null)
    : Seq<T>
{
    /// <summary>Element to repeat.</summary>
    public T Value => value;

    /// <summary>Number of times to repeat the element.</summary>
    public int RepeatCount => repeatCount;

    /// <summary>Scheduler to run the producer loop on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.RepeatValueCount(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Repeat({value}, {repeatCount})"
            : $"Seq.Repeat({value}, {repeatCount}, {scheduler})";
}
