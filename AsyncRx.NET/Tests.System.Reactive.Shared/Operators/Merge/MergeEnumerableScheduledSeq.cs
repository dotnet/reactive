// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Merge(scheduler)</c> over an enumerable of sequences.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, SchedulerRef)"/>; materialized
/// by each target through
/// <see cref="ISeqVisitor.MergeEnumerableScheduled{T}(MergeEnumerableScheduledSeq{T})"/>.
/// </remarks>
public sealed class MergeEnumerableScheduledSeq<T>(IEnumerable<Seq<T>> sources, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Enumerable sequence of observable sequences.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <summary>Scheduler to run the enumeration of the sequence of sources on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.MergeEnumerableScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{{{string.Join(", ", sources)}}}.Merge({scheduler})";
}
