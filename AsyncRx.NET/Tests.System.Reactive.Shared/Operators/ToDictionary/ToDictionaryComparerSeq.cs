// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToDictionary(keySelector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToDictionary{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToDictionaryComparer{TSource, TKey}(ToDictionaryComparerSeq{TSource, TKey})"/>.
/// </remarks>
public sealed class ToDictionaryComparerSeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    IEqualityComparer<TKey> comparer,
    string keyText)
    : Seq<IDictionary<TKey, TSource>>
    where TKey : notnull
{
    /// <summary>An observable sequence to create a dictionary for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <summary>An equality comparer to compare keys.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;
    /// <inheritdoc/>
    protected override Realized<Seq<IDictionary<TKey, TSource>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToDictionaryComparer(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToDictionary({keyText}, {comparer})";
}
