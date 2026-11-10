// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>int</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, int?}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MaxNullableInt32Selector{TSource}(MaxNullableInt32SelectorSeq{TSource})"/>.
/// </remarks>
public sealed class MaxNullableInt32SelectorSeq<TSource>(
    Seq<TSource> source,
    Func<TSource, int?> selector,
    string text)
    : Seq<int?>
{
    /// <summary>A sequence of values to determine the maximum value of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, int?> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<int?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxNullableInt32Selector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max({text})";
}
