// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Scan</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Scan((acc, x) =&gt; ...)</c> or
/// <c>xs.Scan(seed, ...)</c> exactly as the Rx.NET test it was migrated from does. Each method
/// builds one node of the query description (one node type per overload, in this folder),
/// which each target turns into its own <c>Scan</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Scan.cs</c>.
/// </remarks>
public static class ScanExtensions
{
    /// <summary>Describes <c>source.Scan(accumulator)</c>, without a seed.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to accumulate over.</param>
    /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="accumulator"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ScanSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Scan{T}(ScanSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Scan<T>(
        this Seq<T> source,
        Func<T, T, T> accumulator,
        [CallerArgumentExpression(nameof(accumulator))] string text = "") =>
        new ScanSeq<T>(source, accumulator, text);

    /// <summary>Describes <c>source.Scan(seed, accumulator)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulated value.</typeparam>
    /// <param name="source">An observable sequence to accumulate over.</param>
    /// <param name="seed">The initial accumulator value.</param>
    /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="accumulator"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ScanSeedSeq{T, TAccumulate}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.ScanSeed{T, TAccumulate}(ScanSeedSeq{T, TAccumulate})"/>.
    /// </remarks>
    public static Seq<TAccumulate> Scan<T, TAccumulate>(
        this Seq<T> source,
        TAccumulate seed,
        Func<TAccumulate, T, TAccumulate> accumulator,
        [CallerArgumentExpression(nameof(accumulator))] string text = "") =>
        new ScanSeedSeq<T, TAccumulate>(source, seed, accumulator, text);
}
