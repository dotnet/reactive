// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>TakeWhile</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.TakeWhile(x =&gt; ...)</c> or
/// <c>xs.TakeWhile((x, i) =&gt; ...)</c> exactly as the Rx.NET test it was migrated from does.
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>TakeWhile</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.TakeWhile.cs</c>.
/// </remarks>
public static class TakeWhileExtensions
{
    /// <summary>Describes <c>source.TakeWhile(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence to return elements from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeWhileSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeWhile{T}(TakeWhileSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeWhile<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new TakeWhileSeq<T>(source, predicate, text);

    /// <summary>Describes <c>source.TakeWhile((x, i) =&gt; ...)</c>, the indexed form.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence to return elements from.</param>
    /// <param name="predicate">
    /// A function to test each element for a condition; the second parameter of the function
    /// represents the index of the source element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeWhileIndexedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeWhileIndexed{T}(TakeWhileIndexedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeWhile<T>(
        this Seq<T> source,
        Func<T, int, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new TakeWhileIndexedSeq<T>(source, predicate, text);
}
