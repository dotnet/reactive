// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min(selector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Min{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, IComparer{TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinSelectorComparer{TSource, TResult}(MinSelectorComparerSeq{TSource, TResult})"/>.
/// </remarks>
public sealed class MinSelectorComparerSeq<TSource, TResult>(
    Seq<TSource> source,
    Func<TSource, TResult> selector,
    IComparer<TResult> comparer,
    string text)
    : Seq<TResult>
{
    /// <summary>An observable sequence to determine the minimum element of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, TResult> Selector => selector;

    /// <summary>Comparer used to compare elements.</summary>
    public IComparer<TResult> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinSelectorComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min({text}, {comparer})";
}
