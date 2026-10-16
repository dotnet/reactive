// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan, timeShift)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, TimeSpan)"/>; materialized
/// by each target through
/// <see cref="ISeqVisitor.WindowTimeShiftDefault{T}(WindowTimeShiftDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class WindowTimeShiftDefaultSeq<T>(Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Interval between creation of consecutive windows.</summary>
    public TimeSpan TimeShift => timeShift;

    /// <inheritdoc/>
    protected override Realized<Seq<Seq<T>>> AcceptCore(ISeqVisitor visitor) => visitor.WindowTimeShiftDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {timeShift.Ticks} ticks)";
}
