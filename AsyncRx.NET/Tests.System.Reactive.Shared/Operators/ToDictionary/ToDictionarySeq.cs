// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToDictionary(keySelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.ToDictionary{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ToDictionary{TSource, TKey}(ToDictionarySeq{TSource, TKey})"/>.
/// </remarks>
public sealed class ToDictionarySeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    string keyText)
    : Seq<IDictionary<TKey, TSource>>
    where TKey : notnull
{
    /// <summary>An observable sequence to create a dictionary for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A function to extract a key from each element.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;
    /// <inheritdoc/>
    protected override Realized<Seq<IDictionary<TKey, TSource>>> AcceptCore(
        ISeqVisitor visitor) =>
        visitor.ToDictionary(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.ToDictionary({keyText})";
}
