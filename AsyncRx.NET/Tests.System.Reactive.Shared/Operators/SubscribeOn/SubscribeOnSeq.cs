// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SubscribeOn(scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="SubscribeOnExtensions.SubscribeOn{T}(Seq{T}, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SubscribeOn{T}(SubscribeOnSeq{T})"/>.
/// </remarks>
public sealed class SubscribeOnSeq<T>(Seq<T> source, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Scheduler to perform subscription and unsubscription actions on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SubscribeOn(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SubscribeOn({scheduler})";
}
