// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>All</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.All(x =&gt; ...)</c> exactly as the
/// Rx.NET test it was migrated from does. It builds one node of the query description, which
/// each target turns into its own <c>All</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.All.cs</c>.
/// </remarks>
public static class AllExtensions
{
    /// <summary>Describes <c>source.All(predicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// An observable sequence whose elements to apply the predicate to.
    /// </param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="AllSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.All{T}(AllSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> All<T>(
        this Seq<T> source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new AllSeq<T>(source, predicate, text);
}
