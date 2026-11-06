// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="AggregateSeq{T}"/> as the target's own
    /// <c>Aggregate(accumulator)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="AggregateExtensions.Aggregate{T}(Seq{T}, Func{T, T, T}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Aggregate<T>(AggregateSeq<T> seq);

    /// <summary>
    /// Materializes an <see cref="AggregateSeedSeq{T, TAccumulate}"/> as the target's own
    /// <c>Aggregate(seed, accumulator)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="AggregateExtensions.Aggregate{T, TAccumulate}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, string)"/>.
    /// </remarks>
    Realized<Seq<TAccumulate>> AggregateSeed<T, TAccumulate>(AggregateSeedSeq<T, TAccumulate> seq);

    /// <summary>
    /// Materializes an <see cref="AggregateSeedResultSeq{T, TAccumulate, TResult}"/> as the
    /// target's own <c>Aggregate(seed, accumulator, resultSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="AggregateExtensions.Aggregate{T, TAccumulate, TResult}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, Func{TAccumulate, TResult}, string, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> AggregateSeedResult<T, TAccumulate, TResult>(
        AggregateSeedResultSeq<T, TAccumulate, TResult> seq);
}
