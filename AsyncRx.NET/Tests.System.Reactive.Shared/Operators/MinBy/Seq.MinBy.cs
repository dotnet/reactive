// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>MinBy</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.MinBy(x =&gt; x.Key)</c>, a shared scenario writes the
/// same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the same call
/// resolves to the same overload on both sides. One node type per overload, in this folder,
/// each materialized by a target through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.MinBy.cs</c>. The result is a sequence of one list, the elements whose key
/// is the minimum, as Rx.NET's <c>MinBy</c> yields it.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.MinBy(keySelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to get the minimum elements for.</param>
    /// <param name="keySelector">Key selector function.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinBySeq{TSource, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinBy{TSource, TKey}(MinBySeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<IList<TSource>> MinBy<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new MinBySeq<TSource, TKey>(source, keySelector, text);

    /// <summary>Describes <c>source.MinBy(keySelector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key computed for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence to get the minimum elements for.</param>
    /// <param name="keySelector">Key selector function.</param>
    /// <param name="comparer">Comparer used to compare key values.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query
    /// in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinByComparerSeq{TSource, TKey}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.MinByComparer{TSource, TKey}(MinByComparerSeq{TSource, TKey})"/>.
    /// </remarks>
    public static Seq<IList<TSource>> MinBy<TSource, TKey>(
        this Seq<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new MinByComparerSeq<TSource, TKey>(source, keySelector, comparer, text);
}
