// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min(selector)</c> with a selector returning <c>float</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinSingleSelector{TSource}(MinSingleSelectorSeq{TSource})"/>.
/// </remarks>
public sealed class MinSingleSelectorSeq<TSource>(
    Seq<TSource> source,
    Func<TSource, float> selector,
    string text)
    : Seq<float>
{
    /// <summary>A sequence of values to determine the minimum value of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, float> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<float>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinSingleSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min({text})";
}
