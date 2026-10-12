// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Repeat(value)</c> or <c>Seq.Repeat(value, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Repeat{T}(T)"/> and <see cref="Seq.Repeat{T}(T, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.RepeatValue{T}(RepeatValueSeq{T})"/>.
/// </remarks>
public sealed class RepeatValueSeq<T>(T value, SchedulerRef? scheduler = null) : Seq<T>
{
    /// <summary>Element to repeat.</summary>
    public T Value => value;

    /// <summary>Scheduler to run the producer loop on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.RepeatValue(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"Seq.Repeat({value})" : $"Seq.Repeat({value}, {scheduler})";
}
