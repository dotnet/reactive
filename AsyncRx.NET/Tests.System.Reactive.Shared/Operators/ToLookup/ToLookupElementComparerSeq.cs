// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToLookup(keySelector, elementSelector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToLookup{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToLookupElementComparer{TSource, TKey, TElement}(ToLookupElementComparerSeq{TSource, TKey, TElement})"/>.
/// </remarks>
public sealed class ToLookupElementComparerSeq<TSource, TKey, TElement>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector,
    IEqualityComparer<TKey> comparer,
    string keyText,
    string elementText)
    : Seq<ILookup<TKey, TElement>>
{
    /// <summary>An observable sequence to create a lookup for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <summary>A transform function to produce a result element value from each element.</summary>
    public Func<TSource, TElement> ElementSelector => elementSelector;

    /// <summary>An equality comparer to compare keys.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;
    /// <inheritdoc/>
    protected override Realized<Seq<ILookup<TKey, TElement>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToLookupElementComparer(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToLookup({keyText}, {elementText}, {comparer})";
}
