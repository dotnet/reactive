// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(source1, source2, ...)</c> over any number of sources of one
/// type, whose elements are lists.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.CombineLatest{T}(Seq{T}[])"/>, the <c>params</c> form; materialized
/// by each target through <see cref="ISeqVisitor.CombineLatestArray{T}(CombineLatestArraySeq{T})"/>.
/// </remarks>
public sealed class CombineLatestArraySeq<T>(Seq<T>[] sources) : Seq<IList<T>>
{
    /// <summary>Observable sources.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatestArray(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.CombineLatest({string.Join(", ", sources.Select(s => s.ToString()))})";
}
