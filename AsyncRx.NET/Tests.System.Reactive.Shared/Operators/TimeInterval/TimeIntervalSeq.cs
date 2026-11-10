// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TimeInterval([scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="TimeIntervalExtensions.TimeInterval{T}(Seq{T})"/> or
/// <see cref="TimeIntervalExtensions.TimeInterval{T}(Seq{T}, SchedulerRef)"/>; materialized by each target
/// through <see cref="ISeqVisitor.TimeInterval{T}(TimeIntervalSeq{T})"/>. The elements are Rx.NET's
/// <see cref="TimeInterval{T}"/>, which AsyncRx.NET uses too.
/// </remarks>
public sealed class TimeIntervalSeq<T>(Seq<T> source, SchedulerRef? scheduler)
    : Seq<TimeInterval<T>>
{
    /// <summary>Source sequence to retrieve time intervals for.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Scheduler used to compute time intervals, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TimeInterval<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeInterval(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"{source}.TimeInterval()" : $"{source}.TimeInterval({scheduler})";
}
