// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeLast(count[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, int, SchedulerRef)"/> or, with no
/// scheduler, by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, int)"/>; materialized by each
/// target through <see cref="ISeqVisitor.TakeLastCount{T}(TakeLastCountSeq{T})"/>.
/// </remarks>
public sealed class TakeLastCountSeq<T>(Seq<T> source, int count, SchedulerRef? scheduler) : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Number of elements to take from the end of the source sequence.</summary>
    public int Count => count;

    /// <summary>
    /// Scheduler used to drain the queue upon completion of the source sequence, or null for the
    /// target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeLastCount(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.TakeLast({count})"
            : $"{source}.TakeLast({count}, {scheduler})";
}
