// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>GroupByUntil</c> overloads: <c>GroupBy</c> whose groups expire when a duration sequence
/// signals.
/// </summary>
/// <remarks>
/// One node type per overload, as for <c>GroupBy</c>; the duration selector takes the group as a
/// <see cref="Group{TKey, T}"/> description and returns a description.
/// </remarks>
public static class GroupByUntilExtensions
{
    /// <summary>Describes <c>source.GroupByUntil(keySelector, durationSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeySeq{T, TKey, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKey{T, TKey, TDuration}(GroupByUntilKeySeq{T, TKey, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupByUntil<T, TKey, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<Group<TKey, T>, Seq<TDuration>> durationSelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeySeq<T, TKey, TDuration>(source, keySelector, durationSelector, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, durationSelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyComparerSeq{T, TKey, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyComparer{T, TKey, TDuration}(GroupByUntilKeyComparerSeq{T, TKey, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupByUntil<T, TKey, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<Group<TKey, T>, Seq<TDuration>> durationSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyComparerSeq<T, TKey, TDuration>(source, keySelector, durationSelector, comparer, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, durationSelector, capacity)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="capacity">The number of groups to keep active while enumerating the result.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyCapacitySeq{T, TKey, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyCapacity{T, TKey, TDuration}(GroupByUntilKeyCapacitySeq{T, TKey, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupByUntil<T, TKey, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<Group<TKey, T>, Seq<TDuration>> durationSelector,
        int capacity,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyCapacitySeq<T, TKey, TDuration>(source, keySelector, durationSelector, capacity, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, durationSelector, capacity, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="capacity">The number of groups to keep active while enumerating the result.</param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyCapacityComparerSeq{T, TKey, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyCapacityComparer{T, TKey, TDuration}(GroupByUntilKeyCapacityComparerSeq{T, TKey, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, T>> GroupByUntil<T, TKey, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<Group<TKey, T>, Seq<TDuration>> durationSelector,
        int capacity,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyCapacityComparerSeq<T, TKey, TDuration>(source, keySelector, durationSelector, capacity, comparer, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, elementSelector, durationSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyElementSeq{T, TKey, TElement, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyElement{T, TKey, TElement, TDuration}(GroupByUntilKeyElementSeq{T, TKey, TElement, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupByUntil<T, TKey, TElement, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<Group<TKey, TElement>, Seq<TDuration>> durationSelector,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyElementSeq<T, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyElementComparerSeq{T, TKey, TElement, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyElementComparer{T, TKey, TElement, TDuration}(GroupByUntilKeyElementComparerSeq{T, TKey, TElement, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupByUntil<T, TKey, TElement, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<Group<TKey, TElement>, Seq<TDuration>> durationSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyElementComparerSeq<T, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="capacity">The number of groups to keep active while enumerating the result.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyElementCapacitySeq{T, TKey, TElement, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyElementCapacity{T, TKey, TElement, TDuration}(GroupByUntilKeyElementCapacitySeq{T, TKey, TElement, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupByUntil<T, TKey, TElement, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<Group<TKey, TElement>, Seq<TDuration>> durationSelector,
        int capacity,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyElementCapacitySeq<T, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, text);

    /// <summary>Describes <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
    /// <param name="durationSelector">A function to signal the expiration of a group.</param>
    /// <param name="capacity">The number of groups to keep active while enumerating the result.</param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="GroupByUntilKeyElementCapacityComparerSeq{T, TKey, TElement, TDuration}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.GroupByUntilKeyElementCapacityComparer{T, TKey, TElement, TDuration}(GroupByUntilKeyElementCapacityComparerSeq{T, TKey, TElement, TDuration})"/>.
    /// </remarks>
    public static Seq<Group<TKey, TElement>> GroupByUntil<T, TKey, TElement, TDuration>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<Group<TKey, TElement>, Seq<TDuration>> durationSelector,
        int capacity,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupByUntilKeyElementCapacityComparerSeq<T, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer, text);
}
