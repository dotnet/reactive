// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>SingleAsync</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes
/// <c>xs.SingleAsync()</c> or <c>xs.SingleAsync(x =&gt; ...)</c>
/// exactly as the Rx.NET test it was migrated from does. Each method builds one node of the
/// query description (one node type per overload, in this folder), which each target turns
/// into its own <c>SingleAsync</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.SingleAsync.cs</c>.
/// </remarks>
public static class SingleAsyncExtensions
{
    /// <summary>Describes <c>source.SingleAsync()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="SingleAsyncSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SingleAsync{T}(SingleAsyncSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SingleAsync<T>(this Seq<T> source) =>
        new SingleAsyncSeq<T>(source);

    /// <summary>Describes <c>source.SingleAsync(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source observable sequence.</param>
    /// <param name="predicate">
    /// A predicate function to evaluate for elements in the source sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SingleAsyncPredicateSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.SingleAsyncPredicate{T}(SingleAsyncPredicateSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SingleAsync<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new SingleAsyncPredicateSeq<T>(source, predicate, text);
}
