// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupBy(keySelector, capacity, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, int, IEqualityComparer{TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.GroupByKeyCapacityComparer{T, TKey}(GroupByKeyCapacityComparerSeq{T, TKey})"/>.
/// The result is a sequence of <see cref="Group{TKey, T}"/> descriptions, each with its key.
/// </remarks>
public sealed class GroupByKeyCapacityComparerSeq<T, TKey>(
    Seq<T> source,
    Func<T,
    TKey> keySelector,
    int capacity,
    IEqualityComparer<TKey> comparer,
    string text) : Seq<Group<TKey, T>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </summary>
    public int Capacity => capacity;

    /// <summary>An equality comparer to compare keys with.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;

    /// <inheritdoc/>
    public override Realized<Seq<Group<TKey, T>>> Accept(ISeqVisitor visitor) =>
        visitor.GroupByKeyCapacityComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupBy({text})";
}
