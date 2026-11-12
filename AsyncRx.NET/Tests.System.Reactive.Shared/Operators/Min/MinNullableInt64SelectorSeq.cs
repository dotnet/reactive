// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>long</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, long?}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinNullableInt64Selector{TSource}(MinNullableInt64SelectorSeq{TSource})"/>.
/// </remarks>
public sealed class MinNullableInt64SelectorSeq<TSource>(
    Seq<TSource> source,
    Func<TSource, long?> selector,
    string text)
    : Seq<long?>
{
    /// <summary>A sequence of values to determine the minimum value of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, long?> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<long?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinNullableInt64Selector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min({text})";
}
