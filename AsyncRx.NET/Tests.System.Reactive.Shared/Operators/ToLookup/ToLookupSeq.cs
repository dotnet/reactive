// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToLookup(keySelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToLookup{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToLookup{TSource, TKey}(ToLookupSeq{TSource, TKey})"/>.
/// </remarks>
public sealed class ToLookupSeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    string keyText)
    : Seq<ILookup<TKey, TSource>>
{
    /// <summary>An observable sequence to create a lookup for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;
    /// <inheritdoc/>
    protected override Realized<Seq<ILookup<TKey, TSource>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToLookup(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToLookup({keyText})";
}
