// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Prepend(value, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Prepend{T}(Seq{T}, T, SchedulerRef)"/>; materialized by each
/// target through <see cref="ISeqVisitor.PrependScheduler{T}(PrependSchedulerSeq{T})"/>.
/// </remarks>
public sealed class PrependSchedulerSeq<T>(Seq<T> source, T value, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence to prepend the value to.</summary>
    public Seq<T> Source => source;

    /// <summary>Value to prepend to the specified sequence.</summary>
    public T Value => value;

    /// <summary>Scheduler to emit the prepend values on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.PrependScheduler(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Prepend({value}, {scheduler})";
}
