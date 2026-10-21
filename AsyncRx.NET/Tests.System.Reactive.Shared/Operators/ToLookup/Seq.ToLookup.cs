// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ToLookup</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.ToLookup(x =&gt; x % 2)</c>, a shared scenario writes the
/// same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the same call resolves
/// to the same overload on both sides. One node type per overload, in this folder, each
/// materialized by a target through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.ToLookup.cs</c>: the key selector alone, with a comparer, with an element
/// selector, and with both. The result is a sequence of one lookup, as Rx.NET's
/// <c>ToLookup</c> yields it.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.ToLookup(keySelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the lookup key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a lookup for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToLookupSeq{TSource, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToLookup{TSource, TKey}(ToLookupSeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "") =>
        new ToLookupSeq<TSource, TKey>(
            source, keySelector, keyText);

    /// <summary>Describes <c>source.ToLookup(keySelector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the lookup key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a lookup for.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="comparer">An equality comparer to compare keys.</param>
    /// <param name="keyText">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ToLookupComparerSeq{TSource, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToLookupComparer{TSource, TKey}(ToLookupComparerSeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "") =>
        new ToLookupComparerSeq<TSource, TKey>(
            source, keySelector, comparer, keyText);

    /// <summary>Describes <c>source.ToLookup(keySelector, elementSelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the lookup key computed for each element in the source sequence.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the lookup value computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a lookup for.</param>
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
    /// Builds a <see cref="ToLookupElementSeq{TSource, TKey, TElement}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToLookupElement{TSource, TKey, TElement}(ToLookupElementSeq{TSource, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "",
        [CallerArgumentExpression(nameof(elementSelector))] string elementText = "") =>
        new ToLookupElementSeq<TSource, TKey, TElement>(
            source, keySelector, elementSelector, keyText, elementText);

    /// <summary>Describes <c>source.ToLookup(keySelector, elementSelector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the lookup key computed for each element in the source sequence.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the lookup value computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to create a lookup for.</param>
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
    /// Builds a <see cref="ToLookupElementComparerSeq{TSource, TKey, TElement}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToLookupElementComparer{TSource, TKey, TElement}(ToLookupElementComparerSeq{TSource, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string keyText = "",
        [CallerArgumentExpression(nameof(elementSelector))] string elementText = "") =>
        new ToLookupElementComparerSeq<TSource, TKey, TElement>(
            source, keySelector, elementSelector, comparer, keyText, elementText);
}
