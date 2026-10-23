// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToLookup(keySelector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToLookup{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToLookupComparer{TSource, TKey}(ToLookupComparerSeq{TSource, TKey})"/>.
/// </remarks>
public sealed class ToLookupComparerSeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    IEqualityComparer<TKey> comparer,
    string keyText)
    : Seq<ILookup<TKey, TSource>>
{
    /// <summary>An observable sequence to create a lookup for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <summary>An equality comparer to compare keys.</summary>
    public IEqualityComparer<TKey> Comparer => comparer;
    /// <inheritdoc/>
    protected override Realized<Seq<ILookup<TKey, TSource>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToLookupComparer(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToLookup({keyText}, {comparer})";
}
