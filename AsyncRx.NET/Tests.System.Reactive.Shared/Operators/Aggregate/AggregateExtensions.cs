// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Aggregate</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Aggregate((acc, x) =&gt; ...)</c>,
/// <c>xs.Aggregate(seed, ...)</c> or <c>xs.Aggregate(seed, ..., result)</c> exactly as the
/// Rx.NET test it was migrated from does; the type parameters are in Rx.NET's order, so a test
/// that names them (<c>Aggregate&lt;int, int, int&gt;</c>) transcribes too. Each method builds
/// one node of the query description (one node type per overload, in this folder), which each
/// target turns into its own <c>Aggregate</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Aggregate.cs</c>.
/// </remarks>
public static class AggregateExtensions
{
    /// <summary>Describes <c>source.Aggregate(accumulator)</c>, without a seed.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to aggregate over.</param>
    /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="accumulator"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="AggregateSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Aggregate{T}(AggregateSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Aggregate<T>(
        this Seq<T> source,
        Func<T, T, T> accumulator,
        [CallerArgumentExpression(nameof(accumulator))] string text = "") =>
        new AggregateSeq<T>(source, accumulator, text);

    /// <summary>Describes <c>source.Aggregate(seed, accumulator)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
    /// <param name="source">An observable sequence to aggregate over.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="accumulator"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="AggregateSeedSeq{T, TAccumulate}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.AggregateSeed{T, TAccumulate}(AggregateSeedSeq{T, TAccumulate})"/>.
    /// </remarks>
    public static Seq<TAccumulate> Aggregate<T, TAccumulate>(
        this Seq<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator,
        [CallerArgumentExpression(nameof(accumulator))] string text = "") =>
        new AggregateSeedSeq<T, TAccumulate>(source, seed, accumulator, text);

    /// <summary>Describes <c>source.Aggregate(seed, accumulator, resultSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
    /// <typeparam name="TResult">The type of the resulting value.</typeparam>
    /// <param name="source">An observable sequence to aggregate over.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
    /// <param name="resultSelector">
    /// A function to transform the final accumulator value into the result value.
    /// </param>
    /// <param name="accumulatorText">
    /// Supplied by the compiler (the source text of <paramref name="accumulator"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="resultSelectorText">
    /// Supplied by the compiler (the source text of <paramref name="resultSelector"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="AggregateSeedResultSeq{T, TAccumulate, TResult}"/>, which each
    /// target materializes through
    /// <see cref="ISeqVisitor.AggregateSeedResult{T, TAccumulate, TResult}(AggregateSeedResultSeq{T, TAccumulate, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Aggregate<T, TAccumulate, TResult>(
        this Seq<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator,
        Func<TAccumulate, TResult> resultSelector,
        [CallerArgumentExpression(nameof(accumulator))] string accumulatorText = "",
        [CallerArgumentExpression(nameof(resultSelector))] string resultSelectorText = "") =>
        new AggregateSeedResultSeq<T, TAccumulate, TResult>(
            source, seed, accumulator, resultSelector, accumulatorText, resultSelectorText);
}
