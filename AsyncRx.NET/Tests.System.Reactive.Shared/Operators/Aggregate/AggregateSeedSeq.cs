// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Aggregate(seed, accumulator)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="AggregateExtensions.Aggregate{T, TAccumulate}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.AggregateSeed{T, TAccumulate}(AggregateSeedSeq{T, TAccumulate})"/>.
/// </remarks>
public sealed class AggregateSeedSeq<T, TAccumulate>(
    Seq<T> source,
    TAccumulate seed,
    Func<TAccumulate, T, TAccumulate> accumulator,
    string text)
    : Seq<TAccumulate>
{
    /// <summary>An observable sequence to aggregate over.</summary>
    public Seq<T> Source => source;

    /// <summary>The initial accumulator value.</summary>
    public TAccumulate Seed => seed;

    /// <summary>An accumulator function to be invoked on each element.</summary>
    public Func<TAccumulate, T, TAccumulate> Accumulator => accumulator;

    /// <inheritdoc/>
    protected override Realized<Seq<TAccumulate>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AggregateSeed(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Aggregate({seed}, {text})";
}
