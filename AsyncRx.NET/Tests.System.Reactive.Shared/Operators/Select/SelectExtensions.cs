// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Select</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// The plain and the indexed form. Each method builds one node of the query description (one
/// node type per overload, in this folder), and each target turns that node into its own
/// <c>Select</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Select.cs</c>. <c>Select</c> was plumbing before it came under test, and
/// other scenarios still compose with it; the indexed form over a <c>Seq&lt;Seq&lt;T&gt;&gt;</c>
/// is what the flattening idiom <c>xs.Window(...).Select((w, i) =&gt; w.Select(...)).Merge()</c>
/// needs.
/// </remarks>
public static class SelectExtensions
{
    /// <summary>Describes <c>source.Select(selector)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the result sequence, obtained by running the selector function
    /// for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectSeq{TIn, TOut}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Select{TIn, TOut}(SelectSeq{TIn, TOut})"/>.
    /// </remarks>
    public static Seq<TOut> Select<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, TOut> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectSeq<TIn, TOut>(source, selector, text);

    /// <summary>Describes <c>source.Select((x, i) =&gt; ...)</c>.</summary>
    /// <typeparam name="TIn">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOut">
    /// The type of the elements in the result sequence, obtained by running the selector function
    /// for each element in the source sequence.
    /// </typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on.</param>
    /// <param name="selector">
    /// A transform function to apply to each source element; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SelectIndexedSeq{TIn, TOut}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SelectIndexed{TIn, TOut}(SelectIndexedSeq{TIn, TOut})"/>.
    /// </remarks>
    public static Seq<TOut> Select<TIn, TOut>(
        this Seq<TIn> source,
        Func<TIn, int, TOut> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectIndexedSeq<TIn, TOut>(source, selector, text);
}
