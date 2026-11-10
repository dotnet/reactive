// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min(selector)</c> with a selector of any result type.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MinSelector{TSource, TResult}(MinSelectorSeq{TSource, TResult})"/>.
/// </remarks>
public sealed class MinSelectorSeq<TSource, TResult>(
    Seq<TSource> source,
    Func<TSource, TResult> selector,
    string text)
    : Seq<TResult>
{
    /// <summary>An observable sequence to determine the minimum element of.</summary>
    public Seq<TSource> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TSource, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min({text})";
}
