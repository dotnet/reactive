// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(bufferClosingSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="BufferExtensions.Buffer{T, TBufferClosing}(Seq{T}, Func{Seq{TBufferClosing}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.BufferClosings{T, TBufferClosing}(BufferClosingsSeq{T, TBufferClosing})"/>.
/// </remarks>
public sealed class BufferClosingsSeq<T, TBufferClosing>(
    Seq<T> source,
    Func<Seq<TBufferClosing>> bufferClosingSelector,
    string text)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>A function invoked to define the boundaries of the produced buffers.</summary>
    /// <remarks>A new buffer is started when the previous one is closed.</remarks>
    public Func<Seq<TBufferClosing>> BufferClosingSelector => bufferClosingSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferClosings(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({text})";
}
