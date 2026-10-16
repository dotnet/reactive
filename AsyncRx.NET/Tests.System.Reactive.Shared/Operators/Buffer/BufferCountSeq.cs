// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(count, skip)</c>.</summary>
/// <remarks>
/// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, int, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.BufferCount{T}(BufferCountSeq{T})"/>.
/// </remarks>
public sealed class BufferCountSeq<T>(Seq<T> source, int count, int skip) : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each buffer.</summary>
    public int Count => count;

    /// <summary>Number of elements to skip between creation of consecutive buffers.</summary>
    public int Skip => skip;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({count}, {skip})";
}
