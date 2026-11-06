// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Distinct</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Distinct()</c>,
/// <c>xs.Distinct(comparer)</c>, <c>xs.Distinct(x =&gt; ...)</c> or
/// <c>xs.Distinct(x =&gt; ..., comparer)</c> exactly as the Rx.NET test it was migrated
/// from does. Each method builds one node of the query description (one node type per
/// overload, in this folder), which each target turns into its own <c>Distinct</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Distinct.cs</c>.
/// </remarks>
public static class DistinctExtensions
{
    /// <summary>Describes <c>source.Distinct()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct elements for.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Distinct{T}(DistinctSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Distinct<T>(this Seq<T> source) =>
        new DistinctSeq<T>(source);

    /// <summary>Describes <c>source.Distinct(comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct elements for.
    /// </param>
    /// <param name="comparer">Equality comparer for source elements.</param>
    /// <remarks>
    /// Builds a <see cref="DistinctComparerSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DistinctComparer{T}(DistinctComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Distinct<T>(
        this Seq<T> source,
        IEqualityComparer<T> comparer) =>
        new DistinctComparerSeq<T>(source, comparer);

    /// <summary>Describes <c>source.Distinct(keySelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the discriminator key computed for each element.
    /// </typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct elements for.
    /// </param>
    /// <param name="keySelector">A function to compute the comparison key for each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctKeySeq{T, TKey}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DistinctKey{T, TKey}(DistinctKeySeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<T> Distinct<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new DistinctKeySeq<T, TKey>(source, keySelector, text);

    /// <summary>Describes <c>source.Distinct(keySelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the discriminator key computed for each element.
    /// </typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct elements for.
    /// </param>
    /// <param name="keySelector">A function to compute the comparison key for each element.</param>
    /// <param name="comparer">Equality comparer for computed key values.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctKeyComparerSeq{T, TKey}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.DistinctKeyComparer{T, TKey}(DistinctKeyComparerSeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<T> Distinct<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new DistinctKeyComparerSeq<T, TKey>(source, keySelector, comparer, text);
}
