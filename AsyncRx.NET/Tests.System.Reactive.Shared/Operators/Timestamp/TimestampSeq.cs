// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Timestamp([scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="TimestampExtensions.Timestamp{T}(Seq{T})"/> or
/// <see cref="TimestampExtensions.Timestamp{T}(Seq{T}, SchedulerRef)"/>; materialized by each target
/// through <see cref="ISeqVisitor.Timestamp{T}(TimestampSeq{T})"/>. The elements are Rx.NET's
/// <see cref="Timestamped{T}"/>, which AsyncRx.NET uses too.
/// </remarks>
public sealed class TimestampSeq<T>(Seq<T> source, SchedulerRef? scheduler)
    : Seq<Timestamped<T>>
{
    /// <summary>Source sequence to retrieve timestamps for.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Scheduler used to compute timestamps, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Timestamped<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Timestamp(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"{source}.Timestamp()" : $"{source}.Timestamp({scheduler})";
}
