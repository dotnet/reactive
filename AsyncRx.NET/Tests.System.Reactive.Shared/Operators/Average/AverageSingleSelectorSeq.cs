// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Average(selector)</c> with a selector returning <c>float</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.AverageSingleSelector{TSource}(AverageSingleSelectorSeq{TSource})"/>.
/// </remarks>
public sealed class AverageSingleSelectorSeq<TSource>(
    Seq<TSource> source,
    Func<TSource, float> selector,
    string text)
    : Seq<float>
{
    /// <summary>A sequence of values to calculate the average of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, float> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<float>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageSingleSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average({text})";
}
