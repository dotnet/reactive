// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>SelectMany</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// The forms AsyncRx.NET has: an <c>other</c> sequence, a selector, and a collection selector
/// with a result selector, the last two with and without the element index. The enumerable,
/// three-selector and task-returning forms of Rx.NET join as their scenarios are migrated. Each
/// method builds one node of the query description (one node type per overload, in this folder),
/// and each target turns that node into its own <c>SelectMany</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.SelectMany.cs</c>.
/// </remarks>
public static class SelectManyExtensions
{
    /// <summary>Describes <c>source.SelectMany(other)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the other sequence and the elements in the result sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="other">
    /// An observable sequence to project each element from the source sequence onto.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManySeq{TIn, TOut}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SelectMany{TIn, TOut}(SelectManySeq{TIn, TOut})"/>.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(this Seq<TIn> source, Seq<TOut> other) =>
        new SelectManySeq<TIn, TOut>(source, other);

    /// <summary>Describes <c>source.SelectMany(onNext, onError, onCompleted)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the projected inner sequences and the elements in the merged
    /// result sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence of notifications to project.</param>
    /// <param name="onNext">A transform function to apply to each element.</param>
    /// <param name="onError">
    /// A transform function to apply when an error occurs in the source sequence.
    /// </param>
    /// <param name="onCompleted">
    /// A transform function to apply when the end of the source sequence is reached.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the element selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManySelectorsSeq{TIn, TOut}"/>, which each target materializes
    /// through its <c>SelectManySelectors</c> visitor member.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, Seq<TOut>> onNext,
        Func<Exception, Seq<TOut>> onError,
        Func<Seq<TOut>> onCompleted,
        [CallerArgumentExpression(nameof(onNext))] string text = "") =>
        new SelectManySelectorsSeq<TIn, TOut>(source, onNext, onError, onCompleted, text);

    /// <summary>
    /// Describes <c>source.SelectMany(onNext, onError, onCompleted)</c> for an element selector
    /// that takes the index.
    /// </summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the projected inner sequences and the elements in the merged
    /// result sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence of notifications to project.</param>
    /// <param name="onNext">
    /// A transform function to apply to each element; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="onError">
    /// A transform function to apply when an error occurs in the source sequence.
    /// </param>
    /// <param name="onCompleted">
    /// A transform function to apply when the end of the source sequence is reached.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the element selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManySelectorsIndexedSeq{TIn, TOut}"/>, which each target
    /// materializes through its <c>SelectManySelectorsIndexed</c> visitor member.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, int, Seq<TOut>> onNext,
        Func<Exception, Seq<TOut>> onError,
        Func<Seq<TOut>> onCompleted,
        [CallerArgumentExpression(nameof(onNext))] string text = "") =>
        new SelectManySelectorsIndexedSeq<TIn, TOut>(source, onNext, onError, onCompleted, text);

    /// <summary>Describes <c>source.SelectMany(collectionSelector, resultSelector)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TCollection">
    /// The type of the elements in the intermediate sequences produced by the collection
    /// selector.
    /// </typeparam>
    /// <typeparam name="TOut">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="collectionSelector">
    /// A transform function to apply to each element, returning the collection to flatten.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to each element of the intermediate sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the collection selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManyCollectionSeq{TIn, TCollection, TOut}"/>, which each target
    /// materializes through its <c>SelectManyCollection</c> visitor member.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TCollection, TOut>(
        this Seq<TIn> source,
        Func<TIn, Seq<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector,
        [CallerArgumentExpression(nameof(collectionSelector))] string text = "") =>
        new SelectManyCollectionSeq<TIn, TCollection, TOut>(
            source,
            collectionSelector,
            resultSelector,
            text);

    /// <summary>Describes <c>source.SelectMany(selector)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the projected inner sequences and the elements in the merged
    /// result sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManySelectorSeq{TIn, TOut}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.SelectManySelector{TIn, TOut}(SelectManySelectorSeq{TIn, TOut})"/>.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, Seq<TOut>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectManySelectorSeq<TIn, TOut>(source, selector, text);

    /// <summary>
    /// Describes <c>source.SelectMany(selector)</c> for a selector that takes the index.
    /// </summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the projected inner sequences and the elements in the merged
    /// result sequence.
    /// </typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="selector">
    /// A transform function to apply to each element; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManyIndexedSeq{TIn, TOut}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.SelectManyIndexed{TIn, TOut}(SelectManyIndexedSeq{TIn, TOut})"/>.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, int, Seq<TOut>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectManyIndexedSeq<TIn, TOut>(source, selector, text);

    /// <summary>
    /// Describes <c>source.SelectMany(collectionSelector, resultSelector)</c> for selectors that
    /// take the indexes.
    /// </summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TCollection">
    /// The type of the elements in the intermediate sequences produced by the collection
    /// selector.
    /// </typeparam>
    /// <typeparam name="TOut">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="collectionSelector">
    /// A transform function to apply to each element; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="resultSelector">
    /// A transform function to apply to each element of the intermediate sequence; the second
    /// parameter of the function represents the index of the source element and the fourth
    /// parameter represents the index of the intermediate element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the collection selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectManyCollectionIndexedSeq{TIn, TCollection, TOut}"/>, which each
    /// target materializes through its <c>SelectManyCollectionIndexed</c> visitor member.
    /// </remarks>
    public static Seq<TOut> SelectMany<TIn, TCollection, TOut>(
        this Seq<TIn> source,
        Func<TIn, int, Seq<TCollection>> collectionSelector,
        Func<TIn, int, TCollection, int, TOut> resultSelector,
        [CallerArgumentExpression(nameof(collectionSelector))] string text = "") =>
        new SelectManyCollectionIndexedSeq<TIn, TCollection, TOut>(
            source,
            collectionSelector,
            resultSelector,
            text);
}
