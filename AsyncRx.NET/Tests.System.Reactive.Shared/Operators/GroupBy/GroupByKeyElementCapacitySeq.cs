// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupBy(keySelector, elementSelector, capacity)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="GroupByExtensions.GroupBy{T, TKey, TElement}(Seq{T}, Func{T, TKey}, Func{T, TElement}, int, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.GroupByKeyElementCapacity{T, TKey, TElement}(GroupByKeyElementCapacitySeq{T, TKey, TElement})"/>.
/// The result is a sequence of <see cref="Group{TKey, T}"/> descriptions, each with its key.
/// </remarks>
public sealed class GroupByKeyElementCapacitySeq<T, TKey, TElement>(
    Seq<T> source,
    Func<T,
    TKey> keySelector,
    Func<T,
    TElement> elementSelector,
    int capacity,
    string text) : Seq<Group<TKey, TElement>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>A function to map each source element to an element in a group.</summary>
    public Func<T, TElement> ElementSelector => elementSelector;

    /// <summary>
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </summary>
    public int Capacity => capacity;

    /// <inheritdoc/>
    protected override Realized<Seq<Group<TKey, TElement>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.GroupByKeyElementCapacity(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupBy({text})";
}
