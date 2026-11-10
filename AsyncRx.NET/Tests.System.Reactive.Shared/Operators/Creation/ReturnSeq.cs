// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Return(value)</c> or <c>Seq.Return(value, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Return{T}(T)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Return{T}(ReturnSeq{T})"/>.
/// </remarks>
public sealed class ReturnSeq<T>(T value, SchedulerRef? scheduler = null) : Seq<T>
{
    /// <summary>Single element in the resulting observable sequence.</summary>
    public T Value => value;

    /// <summary>Scheduler to send the single element on.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Return(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"Seq.Return({value})" : $"Seq.Return({value}, {scheduler})";
}
