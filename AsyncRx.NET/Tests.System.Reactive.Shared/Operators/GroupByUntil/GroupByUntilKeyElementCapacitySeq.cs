// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity)</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
/// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
/// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
/// <remarks>
/// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with these
/// parameters; materialized by each target through
/// <see cref="ISeqVisitor.GroupByUntilKeyElementCapacity{T, TKey, TElement, TDuration}(GroupByUntilKeyElementCapacitySeq{T, TKey, TElement, TDuration})"/>, with the duration selector adapted to take
/// the target's own group and return its own sequence.
/// </remarks>
public sealed class GroupByUntilKeyElementCapacitySeq<T, TKey, TElement, TDuration>(
    Seq<T> source,
    Func<T, TKey> keySelector,
    Func<T, TElement> elementSelector,
    Func<Group<TKey, TElement>, Seq<TDuration>> durationSelector,
    int capacity,
    string text)
    : Seq<Group<TKey, TElement>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>A function to map each source element to an element in an observable group.</summary>
    public Func<T, TElement> ElementSelector => elementSelector;

    /// <summary>A function to signal the expiration of a group.</summary>
    public Func<Group<TKey, TElement>, Seq<TDuration>> DurationSelector => durationSelector;

    /// <summary>The number of groups to keep active while enumerating the result.</summary>
    public int Capacity => capacity;

    /// <inheritdoc/>
    protected override Realized<Seq<Group<TKey, TElement>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.GroupByUntilKeyElementCapacity(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupByUntil({text})";
}
