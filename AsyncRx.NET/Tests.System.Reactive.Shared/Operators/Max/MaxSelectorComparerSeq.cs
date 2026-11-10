// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max(selector, comparer)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Max{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, IComparer{TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MaxSelectorComparer{TSource, TResult}(MaxSelectorComparerSeq{TSource, TResult})"/>.
/// </remarks>
public sealed class MaxSelectorComparerSeq<TSource, TResult>(
    Seq<TSource> source,
    Func<TSource, TResult> selector,
    IComparer<TResult> comparer,
    string text)
    : Seq<TResult>
{
    /// <summary>An observable sequence to determine the maximum element of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, TResult> Selector => selector;

    /// <summary>Comparer used to compare elements.</summary>
    public IComparer<TResult> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxSelectorComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max({text}, {comparer})";
}
