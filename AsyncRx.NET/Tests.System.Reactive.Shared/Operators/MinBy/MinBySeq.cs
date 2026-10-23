// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.MinBy(keySelector)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.MinBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinBy{TSource, TKey}(MinBySeq{TSource, TKey})"/>.
/// </remarks>
public sealed class MinBySeq<TSource, TKey>(
    Seq<TSource> source,
    Func<TSource, TKey> keySelector,
    string text)
    : Seq<IList<TSource>>
{
    /// <summary>An observable sequence to get the minimum elements for.</summary>
    public Seq<TSource> Source => source;

    /// <summary>Key selector function.</summary>
    public Func<TSource, TKey> KeySelector => keySelector;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<TSource>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinBy(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.MinBy({text})";
}
