// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.DelaySubscription(dueTime[, scheduler])</c> with an absolute due time.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.DelaySubscription{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/> and
/// <see cref="Seq.DelaySubscription{T}(Seq{T}, DateTimeOffset)"/>; materialized by each target
/// through <see cref="ISeqVisitor.DelaySubscriptionAbsolute{T}(DelaySubscriptionAbsoluteSeq{T})"/>.
/// </remarks>
public sealed class DelaySubscriptionAbsoluteSeq<T>(Seq<T> source, DateTimeOffset dueTime, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to delay subscription for.</summary>
    public Seq<T> Source => source;

    /// <summary>Absolute time to perform the subscription at.</summary>
    public DateTimeOffset DueTime => dueTime;

    /// <summary>
    /// Scheduler to run the subscription delay timer on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DelaySubscriptionAbsolute(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.DelaySubscription({dueTime.Ticks} ticks)"
            : $"{source}.DelaySubscription({dueTime.Ticks} ticks, {scheduler})";
}
