// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SkipLast(count)</c>.</summary>
/// <remarks>
/// Built by <see cref="SkipLastExtensions.SkipLast{T}(Seq{T}, int)"/>; materialized by each
/// target through <see cref="ISeqVisitor.SkipLastCount{T}(SkipLastCountSeq{T})"/>.
/// </remarks>
public sealed class SkipLastCountSeq<T>(Seq<T> source, int count) : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Number of elements to bypass at the end of the source sequence.</summary>
    public int Count => count;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SkipLastCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SkipLast({count})";
}
