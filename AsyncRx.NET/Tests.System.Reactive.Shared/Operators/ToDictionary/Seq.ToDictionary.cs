// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ToDictionary</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.ToDictionary(x =&gt; x % 2)</c>, a shared scenario writes the
/// same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the same call resolves
/// to the same overload on both sides. One node type per overload, in this folder, each
/// materialized by a target through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.ToDictionary.cs</c>: the key selector alone, with a comparer, with an element
/// selector, and with both. The result is a sequence of one dictionary, as Rx.NET's
/// <c>ToDictionary</c> yields it.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.ToDictionary(keySelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the dictionary key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a dictionary for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToDictionarySeq{TSource, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToDictionary{TSource, TKey}(ToDictionarySeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "")
        where TKey : notnull =>
        new ToDictionarySeq<TSource, TKey>(
            source, keySelector, keyText);

    /// <summary>Describes <c>source.ToDictionary(keySelector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the dictionary key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a dictionary for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="comparer">An equality comparer to compare keys.</param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToDictionaryComparerSeq{TSource, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToDictionaryComparer{TSource, TKey}(ToDictionaryComparerSeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "")
        where TKey : notnull =>
        new ToDictionaryComparerSeq<TSource, TKey>(
            source, keySelector, comparer, keyText);

    /// <summary>Describes <c>source.ToDictionary(keySelector, elementSelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the dictionary key computed for each element in the source sequence.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the dictionary value computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a dictionary for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="elementSelector">
    /// A transform function to produce a result element value from each element.
    /// </param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <param name="elementText">
    /// Supplied by the compiler (the source text of the element selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToDictionaryElementSeq{TSource, TKey, TElement}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToDictionaryElement{TSource, TKey, TElement}(ToDictionaryElementSeq{TSource, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "",
        [CallerArgumentExpression(nameof(elementSelector))] string elementText = "")
        where TKey : notnull =>
        new ToDictionaryElementSeq<TSource, TKey, TElement>(
            source, keySelector, elementSelector, keyText, elementText);

    /// <summary>
    /// Describes <c>source.ToDictionary(keySelector, elementSelector, comparer)</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the dictionary key computed for each element in the source sequence.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the dictionary value computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a dictionary for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="elementSelector">
    /// A transform function to produce a result element value from each element.
    /// </param>
    /// <param name="comparer">An equality comparer to compare keys.</param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <param name="elementText">
    /// Supplied by the compiler (the source text of the element selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToDictionaryElementComparerSeq{TSource, TKey, TElement}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToDictionaryElementComparer{TSource, TKey, TElement}(ToDictionaryElementComparerSeq{TSource, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "",
        [CallerArgumentExpression(nameof(elementSelector))] string elementText = "")
        where TKey : notnull =>
        new ToDictionaryElementComparerSeq<TSource, TKey, TElement>(
            source, keySelector, elementSelector, comparer, keyText, elementText);
}
