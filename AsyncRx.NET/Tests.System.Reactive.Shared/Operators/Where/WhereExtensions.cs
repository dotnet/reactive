// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Where</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// The plain and the indexed form. Each method builds one node of the query description (one
/// node type per overload, in this folder), and each target turns that node into its own
/// <c>Where</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Where.cs</c>. <c>Where</c> was plumbing before it came under test, and other
/// scenarios still compose with it.
/// </remarks>
public static class WhereExtensions
{
    /// <summary>Describes <c>source.Where(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence whose elements to filter.</param>
    /// <param name="predicate">A function to test each source element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WhereSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Where{T}(WhereSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Where<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new WhereSeq<T>(source, predicate, text);

    /// <summary>Describes <c>source.Where((x, i) =&gt; ...)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence whose elements to filter.</param>
    /// <param name="predicate">
    /// A function to test each source element for a condition; the second parameter of the
    /// function represents the index of the source element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WhereIndexedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.WhereIndexed{T}(WhereIndexedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Where<T>(
        this Seq<T> source,
        Func<T, int, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new WhereIndexedSeq<T>(source, predicate, text);
}
