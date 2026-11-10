// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Then</c> operator over a single sequence.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Then(x =&gt; ...)</c>, a shared scenario writes the same
/// over a description, building a <see cref="SeqPlan1{T1, TResult}"/>; the <c>Then</c> over a
/// pattern is the pattern's own method.
/// </remarks>
public static class ThenExtensions
{
    /// <summary>Describes <c>source.Then(selector)</c>, a plan over one sequence.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, returned by the selector function.
    /// </typeparam>
    /// <param name="source">Observable sequence to apply the selector on.</param>
    /// <param name="selector">
    /// Selector that will be invoked for elements in the source sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="selector"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SeqPlan1{T1, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then1{T1, TResult}(SeqPlan1{T1, TResult})"/>.
    /// </remarks>
    public static SeqPlan<TResult> Then<TSource, TResult>(
        this Seq<TSource> source,
        Func<TSource, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan1<TSource, TResult>(source, selector, text);
}
