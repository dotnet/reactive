// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupBy(keySelector, elementSelector, comparer)</c>.</summary>
/// <remarks>
/// The result is a sequence of <see cref="Group{TKey, T}"/> descriptions; every generic operator
/// applies to it, and a callback receives each group with its key.
/// </remarks>
public sealed class GroupBySeq<T, TKey, TElement>(
    Seq<T> source,
    Func<T, TKey> keySelector,
    Func<T, TElement> elementSelector,
    IEqualityComparer<TKey> comparer,
    string text) : Seq<Group<TKey, TElement>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>A function to map each source element to an element in a group.</summary>
    public Func<T, TElement> ElementSelector => elementSelector;

    /// <summary>An equality comparer to compare keys with.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;

    /// <inheritdoc/>
    public override Realized<Seq<Group<TKey, TElement>>> Accept(ISeqVisitor visitor) =>
        visitor.GroupBy(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupBy({text})";
}
