// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToDictionary(keySelector, elementSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToDictionary{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToDictionaryElement{TSource, TKey, TElement}(ToDictionaryElementSeq{TSource, TKey, TElement})"/>.
/// </remarks>
public sealed class ToDictionaryElementSeq<TSource, TKey, TElement>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector,
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
    /// <inheritdoc/>
    protected override Realized<Seq<IDictionary<TKey, TElement>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToDictionaryElement(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToDictionary({keyText}, {elementText})";
}
