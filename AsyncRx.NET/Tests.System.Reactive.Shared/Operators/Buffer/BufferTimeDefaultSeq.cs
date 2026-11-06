// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(timeSpan)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.BufferTimeDefault{T}(BufferTimeDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class BufferTimeDefaultSeq<T>(Seq<T> source, TimeSpan timeSpan) : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each buffer.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferTimeDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({timeSpan.Ticks} ticks)";
}
