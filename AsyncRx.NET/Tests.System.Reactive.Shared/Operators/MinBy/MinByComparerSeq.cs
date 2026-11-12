// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.MinBy(keySelector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.MinBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IComparer{TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinByComparer{TSource, TKey}(MinByComparerSeq{TSource, TKey})"/>.
/// </remarks>
public sealed class MinByComparerSeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    IComparer<TKey> comparer,
    string text)
    : Seq<IList<TSource>>
{
    /// <summary>An observable sequence to get the minimum elements for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>Key selector function.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <summary>Comparer used to compare key values.</summary>
    public IComparer<TKey> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<TSource>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinByComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.MinBy({text}, {comparer})";
}
