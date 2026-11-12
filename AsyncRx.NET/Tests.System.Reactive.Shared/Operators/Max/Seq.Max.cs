// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Max</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Max()</c> or <c>xs.Max(x =&gt; x.Length)</c>, a shared
/// scenario writes the same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the
/// same call resolves to the same overload on both sides. One node type per overload, in this
/// folder, each materialized by a target through the matching <see cref="ISeqVisitor"/> member
/// in <c>ISeqVisitor.Max.cs</c>.
/// The forms are the typed ones, over each numeric type and its nullable form, and the generic
/// ones, with and without a comparer, each with and without a selector.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Max()</c> over a sequence of <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>int</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxInt32(MaxInt32Seq)"/>.
    /// </remarks>
    public static Seq<int> Max(this Seq<int> source) =>
        new MaxInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxInt32Selector{TSource}(MaxInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, int> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of <c>long</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>long</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxInt64(MaxInt64Seq)"/>.
    /// </remarks>
    public static Seq<long> Max(this Seq<long> source) =>
        new MaxInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxInt64Selector{TSource}(MaxInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, long> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of <c>float</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>float</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxSingle(MaxSingleSeq)"/>.
    /// </remarks>
    public static Seq<float> Max(this Seq<float> source) =>
        new MaxSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxSingleSelector{TSource}(MaxSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, float> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of <c>double</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>double</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxDouble(MaxDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double> Max(this Seq<double> source) =>
        new MaxDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxDoubleSelector{TSource}(MaxDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, double> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>decimal</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxDecimal(MaxDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal> Max(this Seq<decimal> source) =>
        new MaxDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxDecimalSelector{TSource}(MaxDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>int</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableInt32(MaxNullableInt32Seq)"/>.
    /// </remarks>
    public static Seq<int?> Max(this Seq<int?> source) =>
        new MaxNullableInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableInt32Selector{TSource}(MaxNullableInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int?> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, int?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxNullableInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>long</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>long</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableInt64(MaxNullableInt64Seq)"/>.
    /// </remarks>
    public static Seq<long?> Max(this Seq<long?> source) =>
        new MaxNullableInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableInt64Selector{TSource}(MaxNullableInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long?> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, long?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxNullableInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>float</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>float</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableSingle(MaxNullableSingleSeq)"/>.
    /// </remarks>
    public static Seq<float?> Max(this Seq<float?> source) =>
        new MaxNullableSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableSingleSelector{TSource}(MaxNullableSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float?> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, float?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxNullableSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>double</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>double</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableDouble(MaxNullableDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double?> Max(this Seq<double?> source) =>
        new MaxNullableDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableDoubleSelector{TSource}(MaxNullableDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, double?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxNullableDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>decimal</c> values to determine the maximum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableDecimal(MaxNullableDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal?> Max(this Seq<decimal?> source) =>
        new MaxNullableDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxNullableDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxNullableDecimalSelector{TSource}(MaxNullableDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal?> Max<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxNullableDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Max()</c> over elements of any type.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to determine the maximum element of.</param>
    /// <remarks>
    /// Builds a <see cref="MaxSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Max{T}(MaxSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Max<T>(this Seq<T> source) => new MaxSeq<T>(source);

    /// <summary>Describes <c>source.Max(comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to determine the maximum element of.</param>
    /// <param name="comparer">Comparer used to compare elements.</param>
    /// <remarks>
    /// Builds a <see cref="MaxComparerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MaxComparer{T}(MaxComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Max<T>(this Seq<T> source, IComparer<T> comparer) =>
        new MaxComparerSeq<T>(source, comparer);

    /// <summary>
    /// Describes <c>source.Max(selector)</c> with a selector of any result type.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the objects derived from the elements in the source sequence to determine
    /// the maximum of.
    /// </typeparam>
    /// <param name="source">An observable sequence to determine the maximum element of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxSelectorSeq{TSource, TResult}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.MaxSelector{TSource, TResult}(MaxSelectorSeq{TSource, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Max<TSource, TResult>(
        this Seq<TSource> source,
        Func<TSource, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxSelectorSeq<TSource, TResult>(source, selector, text);

    /// <summary>Describes <c>source.Max(selector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the objects derived from the elements in the source sequence to determine
    /// the maximum of.
    /// </typeparam>
    /// <param name="source">An observable sequence to determine the maximum element of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="comparer">Comparer used to compare elements.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MaxSelectorComparerSeq{TSource, TResult}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.MaxSelectorComparer{TSource, TResult}(MaxSelectorComparerSeq{TSource, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Max<TSource, TResult>(
        this Seq<TSource> source,
        Func<TSource, TResult> selector,
        IComparer<TResult> comparer,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MaxSelectorComparerSeq<TSource, TResult>(source, selector, comparer, text);
}
