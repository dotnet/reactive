// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Merge(second, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(Seq{T}, Seq{T}, SchedulerRef)"/>; materialized by
/// each target through
/// <see cref="ISeqVisitor.MergeBinaryScheduled{T}(MergeBinaryScheduledSeq{T})"/>.
/// </remarks>
public sealed class MergeBinaryScheduledSeq<T>(Seq<T> first, Seq<T> second, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>First observable sequence.</summary>
    public Seq<T> First => first;

    /// <summary>Second observable sequence.</summary>
    public Seq<T> Second => second;

    /// <summary>
    /// Scheduler used to introduce concurrency for making subscriptions to the given sequences.
    /// </summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeBinaryScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Merge({second}, {scheduler})";
}
