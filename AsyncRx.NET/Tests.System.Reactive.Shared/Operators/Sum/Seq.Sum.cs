// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Sum</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Sum()</c> or <c>xs.Sum(x =&gt; x.Length)</c>, a shared
/// scenario writes the same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the
/// same call resolves to the same overload on both sides. One node type per overload, in this
/// folder, each materialized by a target through the matching <see cref="ISeqVisitor"/> member
/// in <c>ISeqVisitor.Sum.cs</c>.
/// The forms are the typed ones, over each numeric type and its nullable form, each with and
/// without a selector.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Sum()</c> over a sequence of <c>int</c>.</summary>
    /// <param name="source">A sequence of <c>int</c> values to calculate the sum of.</param>
    /// <remarks>
    /// Builds a <see cref="SumInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumInt32(SumInt32Seq)"/>.
    /// </remarks>
    public static Seq<int> Sum(this Seq<int> source) =>
        new SumInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumInt32Selector{TSource}(SumInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, int> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of <c>long</c>.</summary>
    /// <param name="source">A sequence of <c>long</c> values to calculate the sum of.</param>
    /// <remarks>
    /// Builds a <see cref="SumInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumInt64(SumInt64Seq)"/>.
    /// </remarks>
    public static Seq<long> Sum(this Seq<long> source) =>
        new SumInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumInt64Selector{TSource}(SumInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, long> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of <c>float</c>.</summary>
    /// <param name="source">A sequence of <c>float</c> values to calculate the sum of.</param>
    /// <remarks>
    /// Builds a <see cref="SumSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumSingle(SumSingleSeq)"/>.
    /// </remarks>
    public static Seq<float> Sum(this Seq<float> source) =>
        new SumSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumSingleSelector{TSource}(SumSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, float> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of <c>double</c>.</summary>
    /// <param name="source">A sequence of <c>double</c> values to calculate the sum of.</param>
    /// <remarks>
    /// Builds a <see cref="SumDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumDouble(SumDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double> Sum(this Seq<double> source) =>
        new SumDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumDoubleSelector{TSource}(SumDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, double> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of <c>decimal</c>.</summary>
    /// <param name="source">A sequence of <c>decimal</c> values to calculate the sum of.</param>
    /// <remarks>
    /// Builds a <see cref="SumDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumDecimal(SumDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal> Sum(this Seq<decimal> source) =>
        new SumDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumDecimalSelector{TSource}(SumDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>int</c> values to calculate the sum of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableInt32(SumNullableInt32Seq)"/>.
    /// </remarks>
    public static Seq<int?> Sum(this Seq<int?> source) =>
        new SumNullableInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableInt32Selector{TSource}(SumNullableInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<int?> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, int?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumNullableInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>long</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>long</c> values to calculate the sum of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableInt64(SumNullableInt64Seq)"/>.
    /// </remarks>
    public static Seq<long?> Sum(this Seq<long?> source) =>
        new SumNullableInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableInt64Selector{TSource}(SumNullableInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<long?> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, long?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumNullableInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>float</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>float</c> values to calculate the sum of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableSingle(SumNullableSingleSeq)"/>.
    /// </remarks>
    public static Seq<float?> Sum(this Seq<float?> source) =>
        new SumNullableSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableSingleSelector{TSource}(SumNullableSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float?> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, float?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumNullableSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>double</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>double</c> values to calculate the sum of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableDouble(SumNullableDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double?> Sum(this Seq<double?> source) =>
        new SumNullableDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableDoubleSelector{TSource}(SumNullableDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, double?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumNullableDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>decimal</c> values to calculate the sum of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableDecimal(SumNullableDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal?> Sum(this Seq<decimal?> source) =>
        new SumNullableDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Sum(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the sum of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SumNullableDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SumNullableDecimalSelector{TSource}(SumNullableDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal?> Sum<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SumNullableDecimalSelectorSeq<TSource>(source, selector, text);
}
