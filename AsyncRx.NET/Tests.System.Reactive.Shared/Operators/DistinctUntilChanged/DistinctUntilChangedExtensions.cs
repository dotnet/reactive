// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>DistinctUntilChanged</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.DistinctUntilChanged()</c>,
/// <c>xs.DistinctUntilChanged(comparer)</c>, <c>xs.DistinctUntilChanged(x =&gt; ...)</c> or
/// <c>xs.DistinctUntilChanged(x =&gt; ..., comparer)</c> exactly as the Rx.NET test it was migrated
/// from does. Each method builds one node of the query description (one node type per
/// overload, in this folder), which each target turns into its own <c>DistinctUntilChanged</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.DistinctUntilChanged.cs</c>.
/// </remarks>
public static class DistinctUntilChangedExtensions
{
    /// <summary>Describes <c>source.DistinctUntilChanged()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct contiguous elements for.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctUntilChangedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DistinctUntilChanged{T}(DistinctUntilChangedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DistinctUntilChanged<T>(this Seq<T> source) =>
        new DistinctUntilChangedSeq<T>(source);

    /// <summary>Describes <c>source.DistinctUntilChanged(comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct contiguous elements for.
    /// </param>
    /// <param name="comparer">Equality comparer for source elements.</param>
    /// <remarks>
    /// Builds a <see cref="DistinctUntilChangedComparerSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DistinctUntilChangedComparer{T}(DistinctUntilChangedComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DistinctUntilChanged<T>(
        this Seq<T> source,
        IEqualityComparer<T> comparer) =>
        new DistinctUntilChangedComparerSeq<T>(source, comparer);

    /// <summary>Describes <c>source.DistinctUntilChanged(keySelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the discriminator key computed for each element.
    /// </typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct contiguous elements for.
    /// </param>
    /// <param name="keySelector">A function to compute the comparison key for each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctUntilChangedKeySeq{T, TKey}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DistinctUntilChangedKey{T, TKey}(DistinctUntilChangedKeySeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<T> DistinctUntilChanged<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new DistinctUntilChangedKeySeq<T, TKey>(source, keySelector, text);

    /// <summary>Describes <c>source.DistinctUntilChanged(keySelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the discriminator key computed for each element.
    /// </typeparam>
    /// <param name="source">
    /// An observable sequence to retain distinct contiguous elements for.
    /// </param>
    /// <param name="keySelector">A function to compute the comparison key for each element.</param>
    /// <param name="comparer">Equality comparer for computed key values.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DistinctUntilChangedKeyComparerSeq{T, TKey}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.DistinctUntilChangedKeyComparer{T, TKey}(DistinctUntilChangedKeyComparerSeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<T> DistinctUntilChanged<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new DistinctUntilChangedKeyComparerSeq<T, TKey>(source, keySelector, comparer, text);
}
