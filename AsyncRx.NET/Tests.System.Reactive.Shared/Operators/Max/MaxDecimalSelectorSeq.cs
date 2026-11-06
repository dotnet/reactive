// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Max(selector)</c> with a selector returning <c>decimal</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, decimal}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MaxDecimalSelector{TSource}(MaxDecimalSelectorSeq{TSource})"/>.
/// </remarks>
public sealed class MaxDecimalSelectorSeq<TSource>(
    Seq<TSource> source,
    Func<TSource, decimal> selector,
    string text)
    : Seq<decimal>
{
    /// <summary>A sequence of values to determine the maximum value of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, decimal> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<decimal>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxDecimalSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max({text})";
}
