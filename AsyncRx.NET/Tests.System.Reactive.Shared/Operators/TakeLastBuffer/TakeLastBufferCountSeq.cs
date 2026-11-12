// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeLastBuffer(count)</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeLastBufferExtensions.TakeLastBuffer{T}(Seq{T}, int)"/>; materialized by
/// each target through
/// <see cref="ISeqVisitor.TakeLastBufferCount{T}(TakeLastBufferCountSeq{T})"/>.
/// </remarks>
public sealed class TakeLastBufferCountSeq<T>(Seq<T> source, int count) : Seq<IList<T>>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Number of elements to take from the end of the source sequence.</summary>
    public int Count => count;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeLastBufferCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.TakeLastBuffer({count})";
}
