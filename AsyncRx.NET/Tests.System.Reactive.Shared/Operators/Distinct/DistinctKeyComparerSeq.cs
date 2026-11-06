// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Distinct(keySelector, comparer)</c>.</summary>
/// <remarks>
/// Built by <see cref="DistinctExtensions.Distinct{T, TKey}(Seq{T}, Func{T, TKey}, IEqualityComparer{TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DistinctKeyComparer{T, TKey}(DistinctKeyComparerSeq{T, TKey})"/>.
/// </remarks>
public sealed class DistinctKeyComparerSeq<T, TKey>(
    Seq<T> source,
    Func<T, TKey> keySelector,
    IEqualityComparer<TKey> comparer,
    string text)
    : Seq<T>
{
    /// <summary>An observable sequence to retain distinct elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to compute the comparison key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>Equality comparer for computed key values.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DistinctKeyComparer(this);

    /// <inheritdoc/>
    public override string ToString() => 
        $"{source}.Distinct({text}, {comparer.GetType().Name})";
}
