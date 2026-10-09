// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Zip(sources)</c> over a sequence of sources, whose elements are lists.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Zip{T}(IEnumerable{Seq{T}})"/>; materialized by each target
/// through <see cref="ISeqVisitor.ZipList{T}(ZipListSeq{T})"/>.
/// </remarks>
public sealed class ZipListSeq<T>(IEnumerable<Seq<T>> sources) : Seq<IList<T>>
{
    /// <summary>Observable sources.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ZipList(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Zip([{string.Join(", ", sources)}])";
}
