// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>LongCount</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.LongCount()</c> or
/// <c>xs.LongCount(x =&gt; ...)</c> exactly as the Rx.NET test it was migrated from does. Each
/// method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>LongCount</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.LongCount.cs</c>.
/// </remarks>
public static class LongCountExtensions
{
    /// <summary>Describes <c>source.LongCount()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence that contains elements to be counted.</param>
    /// <remarks>
    /// Builds a <see cref="LongCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.LongCount{T}(LongCountSeq{T})"/>.
    /// </remarks>
    public static Seq<long> LongCount<T>(this Seq<T> source) => new LongCountSeq<T>(source);

    /// <summary>Describes <c>source.LongCount(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence that contains elements to be counted.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="LongCountPredicateSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.LongCountPredicate{T}(LongCountPredicateSeq{T})"/>.
    /// </remarks>
    public static Seq<long> LongCount<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new LongCountPredicateSeq<T>(source, predicate, text);
}
