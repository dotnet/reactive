// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(sources)</c> over a sequence of sources, whose elements are
/// lists.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.CombineLatest{T}(IEnumerable{Seq{T}})"/>; materialized by each
/// target through <see cref="ISeqVisitor.CombineLatestList{T}(CombineLatestListSeq{T})"/>.
/// </remarks>
public sealed class CombineLatestListSeq<T>(IEnumerable<Seq<T>> sources) : Seq<IList<T>>
{
    /// <summary>Observable sources.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatestList(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.CombineLatest([{string.Join(", ", sources)}])";
}
