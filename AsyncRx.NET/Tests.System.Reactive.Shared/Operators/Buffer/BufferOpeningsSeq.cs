// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(bufferOpenings, bufferClosingSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="BufferExtensions.Buffer{T, TBufferOpening, TBufferClosing}(Seq{T}, Seq{TBufferOpening}, Func{TBufferOpening, Seq{TBufferClosing}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.BufferOpenings{T, TBufferOpening, TBufferClosing}(BufferOpeningsSeq{T, TBufferOpening, TBufferClosing})"/>.
/// </remarks>
public sealed class BufferOpeningsSeq<T, TBufferOpening, TBufferClosing>(
    Seq<T> source,
    Seq<TBufferOpening> bufferOpenings,
    Func<TBufferOpening, Seq<TBufferClosing>> bufferClosingSelector,
    string text)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Observable sequence whose elements denote the creation of new buffers.</summary>
    public Seq<TBufferOpening> BufferOpenings => bufferOpenings;

    /// <summary>A function invoked to define the closing of each produced buffer.</summary>
    public Func<TBufferOpening, Seq<TBufferClosing>> BufferClosingSelector => bufferClosingSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferOpenings(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({bufferOpenings}, {text})";
}
