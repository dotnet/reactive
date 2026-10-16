// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.DistinctUntilChanged(comparer)</c>.</summary>
/// <remarks>
/// Built by <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T}(Seq{T}, IEqualityComparer{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DistinctUntilChangedComparer{T}(DistinctUntilChangedComparerSeq{T})"/>.
/// </remarks>
public sealed class DistinctUntilChangedComparerSeq<T>(Seq<T> source, IEqualityComparer<T> comparer)
    : Seq<T>
{
    /// <summary>An observable sequence to retain distinct contiguous elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>Equality comparer for source elements.</summary>
    public IEqualityComparer<T> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DistinctUntilChangedComparer(this);

    /// <inheritdoc/>
    public override string ToString() => 
        $"{source}.DistinctUntilChanged({comparer.GetType().Name})";
}
