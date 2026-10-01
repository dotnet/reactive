// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>sources.Merge(maxConcurrent, scheduler)</c> over an enumerable of sequences.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, int, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MergeEnumerableMaxConcurrentScheduled{T}(MergeEnumerableMaxConcurrentScheduledSeq{T})"/>.
/// </remarks>
public sealed class MergeEnumerableMaxConcurrentScheduledSeq<T>(IEnumerable<Seq<T>> sources, int maxConcurrent, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Enumerable sequence of observable sequences.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <summary>Maximum number of observable sequences being subscribed to concurrently.</summary>
    public int MaxConcurrent => maxConcurrent;

    /// <summary>Scheduler to run the enumeration of the sequence of sources on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.MergeEnumerableMaxConcurrentScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{{{string.Join(", ", sources)}}}.Merge({maxConcurrent}, {scheduler})";
}
