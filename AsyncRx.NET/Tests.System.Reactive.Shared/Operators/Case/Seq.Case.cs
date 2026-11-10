// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.Case(selector, sources, defaultSource)</c>.</summary>
    /// <typeparam name="TValue">
    /// The type of the value returned by the selector function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="selector">Selector function used to determine which source to run.</param>
    /// <param name="sources">
    /// Dictionary of sources to select from based on the selector's result.
    /// </param>
    /// <param name="defaultSource">
    /// Default source to select if the selector's value is not in the dictionary.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="selector"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CaseSeq{TValue, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Case{TValue, TResult}(CaseSeq{TValue, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Case<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, Seq<TResult>> sources,
        Seq<TResult> defaultSource,
        [CallerArgumentExpression(nameof(selector))] string text = "")
        where TValue : notnull =>
        new CaseSeq<TValue, TResult>(selector, sources, defaultSource, null, text);

    /// <summary>Describes <c>Seq.Case(selector, sources)</c>.</summary>
    /// <typeparam name="TValue">
    /// The type of the value returned by the selector function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="selector">Selector function used to determine which source to run.</param>
    /// <param name="sources">
    /// Dictionary of sources to select from based on the selector's result.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="selector"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CaseSeq{TValue, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Case{TValue, TResult}(CaseSeq{TValue, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Case<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, Seq<TResult>> sources,
        [CallerArgumentExpression(nameof(selector))] string text = "")
        where TValue : notnull =>
        new CaseSeq<TValue, TResult>(selector, sources, null, null, text);

    /// <summary>Describes <c>Seq.Case(selector, sources, scheduler)</c>.</summary>
    /// <typeparam name="TValue">
    /// The type of the value returned by the selector function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="selector">Selector function used to determine which source to run.</param>
    /// <param name="sources">
    /// Dictionary of sources to select from based on the selector's result.
    /// </param>
    /// <param name="scheduler">
    /// Scheduler to generate an empty sequence on if the selector's value is not in the
    /// dictionary.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="selector"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CaseSeq{TValue, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Case{TValue, TResult}(CaseSeq{TValue, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Case<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, Seq<TResult>> sources,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(selector))] string text = "")
        where TValue : notnull =>
        new CaseSeq<TValue, TResult>(selector, sources, null, scheduler, text);
}
