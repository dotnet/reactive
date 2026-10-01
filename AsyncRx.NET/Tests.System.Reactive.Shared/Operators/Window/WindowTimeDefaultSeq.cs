// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.WindowTimeDefault{T}(WindowTimeDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class WindowTimeDefaultSeq<T>(Seq<T> source, TimeSpan timeSpan) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <inheritdoc/>
    public override Realized<Seq<Seq<T>>> Accept(ISeqVisitor visitor) => visitor.WindowTimeDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks)";
}
