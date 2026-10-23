// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Append(value, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Append{T}(Seq{T}, T, SchedulerRef)"/>; materialized by each
/// target through <see cref="ISeqVisitor.AppendScheduler{T}(AppendSchedulerSeq{T})"/>.
/// </remarks>
public sealed class AppendSchedulerSeq<T>(Seq<T> source, T value, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence to append the value to.</summary>
    public Seq<T> Source => source;

    /// <summary>Value to append to the specified sequence.</summary>
    public T Value => value;

    /// <summary>Scheduler to emit the append values on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AppendScheduler(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Append({value}, {scheduler})";
}
