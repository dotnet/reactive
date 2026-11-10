// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>SkipWhile</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.SkipWhile(x =&gt; ...)</c> or
/// <c>xs.SkipWhile((x, i) =&gt; ...)</c> exactly as the Rx.NET test it was migrated from does.
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>SkipWhile</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.SkipWhile.cs</c>.
/// </remarks>
public static class SkipWhileExtensions
{
    /// <summary>Describes <c>source.SkipWhile(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to return elements from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipWhileSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipWhile{T}(SkipWhileSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipWhile<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new SkipWhileSeq<T>(source, predicate, text);

    /// <summary>Describes <c>source.SkipWhile((x, i) =&gt; ...)</c>, the indexed form.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to return elements from.</param>
    /// <param name="predicate">
    /// A function to test each element for a condition; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipWhileIndexedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipWhileIndexed{T}(SkipWhileIndexedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipWhile<T>(
        this Seq<T> source,
        Func<T, int, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new SkipWhileIndexedSeq<T>(source, predicate, text);
}
