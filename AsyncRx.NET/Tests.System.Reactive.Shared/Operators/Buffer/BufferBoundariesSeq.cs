// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(bufferBoundaries)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="BufferExtensions.Buffer{T, TBufferBoundary}(Seq{T}, Seq{TBufferBoundary})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.BufferBoundaries{T, TBufferBoundary}(BufferBoundariesSeq{T, TBufferBoundary})"/>.
/// </remarks>
public sealed class BufferBoundariesSeq<T, TBufferBoundary>(
    Seq<T> source,
    Seq<TBufferBoundary> bufferBoundaries)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Sequence of buffer boundary markers.</summary>
    /// <remarks>
    /// The current buffer is closed and a new buffer is opened upon receiving a boundary marker.
    /// </remarks>
    public Seq<TBufferBoundary> BufferBoundaries => bufferBoundaries;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferBoundaries(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({bufferBoundaries})";
}
