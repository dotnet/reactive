// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.DelaySubscription(dueTime[, scheduler])</c> with a relative due time.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.DelaySubscription{T}(Seq{T}, TimeSpan, SchedulerRef)"/> and
/// <see cref="Seq.DelaySubscription{T}(Seq{T}, TimeSpan)"/>; materialized by each target
/// through <see cref="ISeqVisitor.DelaySubscriptionTime{T}(DelaySubscriptionTimeSeq{T})"/>.
/// </remarks>
public sealed class DelaySubscriptionTimeSeq<T>(Seq<T> source, TimeSpan dueTime, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to delay subscription for.</summary>
    public Seq<T> Source => source;

    /// <summary>Relative time shift of the subscribe operation.</summary>
    public TimeSpan DueTime => dueTime;

    /// <summary>
    /// Scheduler to run the subscription delay timer on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DelaySubscriptionTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.DelaySubscription({dueTime.Ticks} ticks)"
            : $"{source}.DelaySubscription({dueTime.Ticks} ticks, {scheduler})";
}
