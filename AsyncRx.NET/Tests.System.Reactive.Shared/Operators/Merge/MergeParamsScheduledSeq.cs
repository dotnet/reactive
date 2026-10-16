// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Merge(scheduler, sources)</c> over an array of sequences.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(SchedulerRef, Seq{T}[])"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeParamsScheduled{T}(MergeParamsScheduledSeq{T})"/>.
/// </remarks>
public sealed class MergeParamsScheduledSeq<T>(SchedulerRef scheduler, Seq<T>[] sources) : Seq<T>
{
    /// <summary>
    /// Scheduler used to introduce concurrency for making subscriptions to the given sequences.
    /// </summary>
    public SchedulerRef Scheduler => scheduler;

    /// <summary>Observable sequences.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeParamsScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Merge({scheduler}, {string.Join(", ", sources.AsEnumerable())})";
}
