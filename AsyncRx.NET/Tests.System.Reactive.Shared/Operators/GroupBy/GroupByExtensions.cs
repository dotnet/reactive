// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>GroupBy</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.GroupBy(x =&gt; x.Trim(), comparer)</c>
/// exactly as the Rx.NET test it was migrated from does. Each method builds one node of the
/// query description (one node type per overload, in this folder), and each target turns that
/// node into its own <c>GroupBy</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.GroupBy.cs</c>. Every form returns a <c>Seq&lt;Group&lt;TKey, T&gt;&gt;</c>, a
/// sequence of groups each carrying its key.
/// </remarks>
public static class GroupByExtensions
{
    /// <summary>Describes <c>source.GroupBy(keySelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeySeq{T, TKey}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByKey{T, TKey}(GroupByKeySeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupBy<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeySeq<T, TKey>(source, keySelector, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyComparerSeq{T, TKey}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.GroupByKeyComparer{T, TKey}(GroupByKeyComparerSeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupBy<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyComparerSeq<T, TKey>(source, keySelector, comparer, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, elementSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">
    /// A function to map each source element to an element in a group.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyElementSeq{T, TKey, TElement}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.GroupByKeyElement{T, TKey, TElement}(GroupByKeyElementSeq{T, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupBy<T, TKey, TElement>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyElementSeq<T, TKey, TElement>(source, keySelector, elementSelector, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, elementSelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">
    /// A function to map each source element to an element in a group.
    /// </param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyElementComparerSeq{T, TKey, TElement}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.GroupByKeyElementComparer{T, TKey, TElement}(GroupByKeyElementComparerSeq{T, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupBy<T, TKey, TElement>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyElementComparerSeq<T, TKey, TElement>(source, keySelector, elementSelector, comparer, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, capacity)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="capacity">
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyCapacitySeq{T, TKey}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.GroupByKeyCapacity{T, TKey}(GroupByKeyCapacitySeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupBy<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        int capacity,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyCapacitySeq<T, TKey>(source, keySelector, capacity, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, capacity, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="capacity">
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyCapacityComparerSeq{T, TKey}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.GroupByKeyCapacityComparer{T, TKey}(GroupByKeyCapacityComparerSeq{T, TKey})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupBy<T, TKey>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        int capacity,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyCapacityComparerSeq<T, TKey>(source, keySelector, capacity, comparer, text);

    /// <summary>Describes <c>source.GroupBy(keySelector, elementSelector, capacity)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">
    /// A function to map each source element to an element in a group.
    /// </param>
    /// <param name="capacity">
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyElementCapacitySeq{T, TKey, TElement}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.GroupByKeyElementCapacity{T, TKey, TElement}(GroupByKeyElementCapacitySeq{T, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupBy<T, TKey, TElement>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        int capacity,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyElementCapacitySeq<T, TKey, TElement>(source, keySelector, elementSelector, capacity, text);

    /// <summary>
    /// Describes <c>source.GroupBy(keySelector, elementSelector, capacity, comparer)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">
    /// A function to map each source element to an element in a group.
    /// </param>
    /// <param name="capacity">
    /// The number of groups the operator expects to create, used to size its lookup.
    /// </param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByKeyElementCapacityComparerSeq{T, TKey, TElement}"/>, which each
    /// target materializes through
    /// <see cref="ISeqVisitor.GroupByKeyElementCapacityComparer{T, TKey, TElement}(GroupByKeyElementCapacityComparerSeq{T, TKey, TElement})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupBy<T, TKey, TElement>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        int capacity,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByKeyElementCapacityComparerSeq<T, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer, text);
}
