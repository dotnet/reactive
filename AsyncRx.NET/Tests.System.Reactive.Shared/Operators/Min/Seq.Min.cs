// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Min</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Min()</c> or <c>xs.Min(x =&gt; x.Length)</c>, a shared
/// scenario writes the same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the
/// same call resolves to the same overload on both sides. One node type per overload, in this
/// folder, each materialized by a target through the matching <see cref="ISeqVisitor"/> member
/// in <c>ISeqVisitor.Min.cs</c>.
/// The forms are the typed ones, over each numeric type and its nullable form, and the generic
/// ones, with and without a comparer, each with and without a selector.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Min()</c> over a sequence of <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>int</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinInt32(MinInt32Seq)"/>.
    /// </remarks>
    public static Seq<int> Min(this Seq<int> source) =>
        new MinInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinInt32Selector{TSource}(MinInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, int> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of <c>long</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>long</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinInt64(MinInt64Seq)"/>.
    /// </remarks>
    public static Seq<long> Min(this Seq<long> source) =>
        new MinInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinInt64Selector{TSource}(MinInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, long> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of <c>float</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>float</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinSingle(MinSingleSeq)"/>.
    /// </remarks>
    public static Seq<float> Min(this Seq<float> source) =>
        new MinSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinSingleSelector{TSource}(MinSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, float> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of <c>double</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>double</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinDouble(MinDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double> Min(this Seq<double> source) =>
        new MinDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinDoubleSelector{TSource}(MinDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, double> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>decimal</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinDecimal(MinDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal> Min(this Seq<decimal> source) =>
        new MinDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinDecimalSelector{TSource}(MinDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>int</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableInt32(MinNullableInt32Seq)"/>.
    /// </remarks>
    public static Seq<int?> Min(this Seq<int?> source) =>
        new MinNullableInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableInt32Selector{TSource}(MinNullableInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int?> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, int?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinNullableInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>long</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>long</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableInt64(MinNullableInt64Seq)"/>.
    /// </remarks>
    public static Seq<long?> Min(this Seq<long?> source) =>
        new MinNullableInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableInt64Selector{TSource}(MinNullableInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long?> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, long?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinNullableInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>float</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>float</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableSingle(MinNullableSingleSeq)"/>.
    /// </remarks>
    public static Seq<float?> Min(this Seq<float?> source) =>
        new MinNullableSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableSingleSelector{TSource}(MinNullableSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float?> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, float?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinNullableSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>double</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>double</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableDouble(MinNullableDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double?> Min(this Seq<double?> source) =>
        new MinNullableDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableDoubleSelector{TSource}(MinNullableDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, double?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinNullableDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>decimal</c> values to determine the minimum value of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableDecimal(MinNullableDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal?> Min(this Seq<decimal?> source) =>
        new MinNullableDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinNullableDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinNullableDecimalSelector{TSource}(MinNullableDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal?> Min<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinNullableDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Min()</c> over elements of any type.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to determine the minimum element of.</param>
    /// <remarks>
    /// Builds a <see cref="MinSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Min{T}(MinSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Min<T>(this Seq<T> source) => new MinSeq<T>(source);

    /// <summary>Describes <c>source.Min(comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to determine the minimum element of.</param>
    /// <param name="comparer">Comparer used to compare elements.</param>
    /// <remarks>
    /// Builds a <see cref="MinComparerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MinComparer{T}(MinComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Min<T>(this Seq<T> source, IComparer<T> comparer) =>
        new MinComparerSeq<T>(source, comparer);

    /// <summary>
    /// Describes <c>source.Min(selector)</c> with a selector of any result type.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the objects derived from the elements in the source sequence to determine
    /// the minimum of.
    /// </typeparam>
    /// <param name="source">An observable sequence to determine the minimum element of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinSelectorSeq{TSource, TResult}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.MinSelector{TSource, TResult}(MinSelectorSeq{TSource, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Min<TSource, TResult>(
        this Seq<TSource> source,
        Func<TSource, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinSelectorSeq<TSource, TResult>(source, selector, text);

    /// <summary>Describes <c>source.Min(selector, comparer)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the objects derived from the elements in the source sequence to determine
    /// the minimum of.
    /// </typeparam>
    /// <param name="source">An observable sequence to determine the minimum element of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="comparer">Comparer used to compare elements.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MinSelectorComparerSeq{TSource, TResult}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.MinSelectorComparer{TSource, TResult}(MinSelectorComparerSeq{TSource, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Min<TSource, TResult>(
        this Seq<TSource> source,
        Func<TSource, TResult> selector,
        IComparer<TResult> comparer,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MinSelectorComparerSeq<TSource, TResult>(source, selector, comparer, text);
}
