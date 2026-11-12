// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToDictionary(keySelector, elementSelector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToDictionary{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToDictionaryElementComparer{TSource, TKey, TElement}(ToDictionaryElementComparerSeq{TSource, TKey, TElement})"/>.
/// </remarks>
public sealed class ToDictionaryElementComparerSeq<TSource, TKey, TElement>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector,
    IEqualityComparer<TKey> comparer,
    string keyText,
    string elementText)
    : Seq<IDictionary<TKey, TElement>>
    where TKey : notnull
{
    /// <summary>An observable sequence to create a dictionary for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <summary>A transform function to produce a result element value from each element.</summary>
    public Func<TSource, TElement> ElementSelector => elementSelector;

    /// <summary>An equality comparer to compare keys.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;
    /// <inheritdoc/>
    protected override Realized<Seq<IDictionary<TKey, TElement>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToDictionaryElementComparer(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToDictionary({keyText}, {elementText}, {comparer})";
}
