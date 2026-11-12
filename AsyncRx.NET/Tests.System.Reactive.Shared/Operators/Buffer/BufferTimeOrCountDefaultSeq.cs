// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(timeSpan, count)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, int)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.BufferTimeOrCountDefault{T}(BufferTimeOrCountDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class BufferTimeOrCountDefaultSeq<T>(
    Seq<T> source,
    TimeSpan timeSpan,
    int count)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Maximum time length of a buffer.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Maximum element count of a buffer.</summary>
    public int Count => count;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferTimeOrCountDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({timeSpan.Ticks} ticks, {count})";
}
