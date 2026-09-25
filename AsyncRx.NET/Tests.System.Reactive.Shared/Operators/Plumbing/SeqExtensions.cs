// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The plumbing operators the shared scenarios compose around the operator under test —
/// <c>Select</c>, <c>Where</c>, <c>SelectMany</c>, <c>Concat</c>, <c>Merge</c>, and the indexed
/// <c>Select</c> over a <see cref="Nested{T}"/> that the flattening idiom
/// <c>xs.Window(...).Select((w, i) =&gt; w.Select(...)).Merge()</c> needs — as extension methods,
/// so that a scenario writes them exactly as the Rx.NET test it was migrated from does.
/// </summary>
/// <remarks>
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), and each target turns that node into its own operator call through the matching
/// member of <see cref="ISeqVisitor"/>.
/// </remarks>
public static class SeqExtensions
{
    /// <summary>Describes <c>source.Select(selector)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="SelectSeq{TIn, TOut}"/>, which each target materializes through <see cref="ISeqVisitor.Select{TIn, TOut}(SelectSeq{TIn, TOut})"/>.</remarks>
    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectSeq<TIn, TOut>(source, selector, text);

    /// <summary>Describes <c>source.Select((x, i) =&gt; ...)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="SelectIndexedSeq{TIn, TOut}"/>, which each target materializes through <see cref="ISeqVisitor.SelectIndexed{TIn, TOut}(SelectIndexedSeq{TIn, TOut})"/>.</remarks>
    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, int, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectIndexedSeq<TIn, TOut>(source, selector, text);

    /// <summary>Describes <c>source.Where(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence whose elements to filter.</param>
    /// <param name="predicate">A function to test each source element for a condition.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="WhereSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Where{T}(WhereSeq{T})"/>.</remarks>
    public static Seq<T> Where<T>(this Seq<T> source, Func<T, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new WhereSeq<T>(source, predicate, text);

    /// <summary>Describes <c>source.SelectMany(other)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">The type of the elements in the other sequence and the elements in the result sequence.</typeparam>
    /// <param name="source">An observable sequence of elements to project.</param>
    /// <param name="other">An observable sequence to project each element from the source sequence onto.</param>
    /// <remarks>Builds a <see cref="SelectManySeq{TIn, TOut}"/>, which each target materializes through <see cref="ISeqVisitor.SelectMany{TIn, TOut}(SelectManySeq{TIn, TOut})"/>.</remarks>
    public static Seq<TOut> SelectMany<TIn, TOut>(this Seq<TIn> source, Seq<TOut> other) => new SelectManySeq<TIn, TOut>(source, other);

    /// <summary>Describes <c>first.Concat(second)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">First observable sequence.</param>
    /// <param name="second">Second observable sequence.</param>
    /// <remarks>Builds a <see cref="ConcatSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Concat{T}(ConcatSeq{T})"/>.</remarks>
    public static Seq<T> Concat<T>(this Seq<T> first, Seq<T> second) => new ConcatSeq<T>(first, second);

    /// <summary>Describes <c>sources.Merge()</c> over a nested sequence.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequence of inner observable sequences.</param>
    /// <remarks>Builds a <see cref="MergeSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Merge{T}(MergeSeq{T})"/>.</remarks>
    public static Seq<T> Merge<T>(this Nested<T> sources) => new MergeSeq<T>(sources);

    /// <summary>Describes <c>nested.Select((window, i) =&gt; ...)</c> over a nested sequence.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="SelectNestedSeq{TIn, TOut}"/>, which each target materializes through <see cref="ISeqVisitor.SelectNested{TIn, TOut}(SelectNestedSeq{TIn, TOut})"/>.</remarks>
    public static Nested<TOut> Select<TIn, TOut>(this Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectNestedSeq<TIn, TOut>(source, selector, text);
}
