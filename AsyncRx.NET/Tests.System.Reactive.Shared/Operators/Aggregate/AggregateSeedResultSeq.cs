// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Aggregate(seed, accumulator, resultSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="AggregateExtensions.Aggregate{T, TAccumulate, TResult}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, Func{TAccumulate, TResult}, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.AggregateSeedResult{T, TAccumulate, TResult}(AggregateSeedResultSeq{T, TAccumulate, TResult})"/>.
/// </remarks>
public sealed class AggregateSeedResultSeq<T, TAccumulate, TResult>(
    Seq<T> source,
    TAccumulate seed,
    Func<TAccumulate, T, TAccumulate> accumulator,
    Func<TAccumulate, TResult> resultSelector,
    string accumulatorText,
    string resultSelectorText)
    : Seq<TResult>
{
    /// <summary>An observable sequence to aggregate over.</summary>
    public Seq<T> Source => source;

    /// <summary>The initial accumulator value.</summary>
    public TAccumulate Seed => seed;

    /// <summary>An accumulator function to be invoked on each element.</summary>
    public Func<TAccumulate, T, TAccumulate> Accumulator => accumulator;

    /// <summary>
    /// A function to transform the final accumulator value into the result value.
    /// </summary>
    public Func<TAccumulate, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AggregateSeedResult(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.Aggregate({seed}, {accumulatorText}, {resultSelectorText})";
}
