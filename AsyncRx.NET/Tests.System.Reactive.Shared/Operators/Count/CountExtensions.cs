// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Count</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Count()</c> or
/// <c>xs.Count(x =&gt; ...)</c> exactly as the Rx.NET test it was migrated from does. Each
/// method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>Count</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Count.cs</c>.
/// </remarks>
public static class CountExtensions
{
    /// <summary>Describes <c>source.Count()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence that contains elements to be counted.</param>
    /// <remarks>
    /// Builds a <see cref="CountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Count{T}(CountSeq{T})"/>.
    /// </remarks>
    public static Seq<int> Count<T>(this Seq<T> source) => new CountSeq<T>(source);

    /// <summary>Describes <c>source.Count(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence that contains elements to be counted.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CountPredicateSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CountPredicate{T}(CountPredicateSeq{T})"/>.
    /// </remarks>
    public static Seq<int> Count<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new CountPredicateSeq<T>(source, predicate, text);
}
