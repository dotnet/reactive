// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Average</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Average()</c> or <c>xs.Average(x =&gt; x.Length)</c>, a shared
/// scenario writes the same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's, so the
/// same call resolves to the same overload on both sides. One node type per overload, in this
/// folder, each materialized by a target through the matching <see cref="ISeqVisitor"/> member
/// in <c>ISeqVisitor.Average.cs</c>.
/// The forms are the typed ones, over each numeric type and its nullable form, each with and
/// without a selector.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Average()</c> over a sequence of <c>int</c>.</summary>
    /// <param name="source">A sequence of <c>int</c> values to calculate the average of.</param>
    /// <remarks>
    /// Builds a <see cref="AverageInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageInt32(AverageInt32Seq)"/>.
    /// </remarks>
    public static Seq<double> Average(this Seq<int> source) =>
        new AverageInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageInt32Selector{TSource}(AverageInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, int> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Average()</c> over a sequence of <c>long</c>.</summary>
    /// <param name="source">A sequence of <c>long</c> values to calculate the average of.</param>
    /// <remarks>
    /// Builds a <see cref="AverageInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageInt64(AverageInt64Seq)"/>.
    /// </remarks>
    public static Seq<double> Average(this Seq<long> source) =>
        new AverageInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageInt64Selector{TSource}(AverageInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, long> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Average()</c> over a sequence of <c>float</c>.</summary>
    /// <param name="source">A sequence of <c>float</c> values to calculate the average of.</param>
    /// <remarks>
    /// Builds a <see cref="AverageSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageSingle(AverageSingleSeq)"/>.
    /// </remarks>
    public static Seq<float> Average(this Seq<float> source) =>
        new AverageSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageSingleSelector{TSource}(AverageSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, float> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Average()</c> over a sequence of <c>double</c>.</summary>
    /// <param name="source">A sequence of <c>double</c> values to calculate the average of.</param>
    /// <remarks>
    /// Builds a <see cref="AverageDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageDouble(AverageDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double> Average(this Seq<double> source) =>
        new AverageDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageDoubleSelector{TSource}(AverageDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, double> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Average()</c> over a sequence of <c>decimal</c>.</summary>
    /// <param name="source">
    /// A sequence of <c>decimal</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageDecimal(AverageDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal> Average(this Seq<decimal> source) =>
        new AverageDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageDecimalSelector{TSource}(AverageDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageDecimalSelectorSeq<TSource>(source, selector, text);

    /// <summary>Describes <c>source.Average()</c> over a sequence of nullable <c>int</c>.</summary>
    /// <param name="source">
    /// A sequence of nullable <c>int</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableInt32Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableInt32(AverageNullableInt32Seq)"/>.
    /// </remarks>
    public static Seq<double?> Average(this Seq<int?> source) =>
        new AverageNullableInt32Seq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableInt32SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableInt32Selector{TSource}(AverageNullableInt32SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, int?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageNullableInt32SelectorSeq<TSource>(source, selector, text);

    /// <summary>
    /// Describes <c>source.Average()</c> over a sequence of nullable <c>long</c>.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <c>long</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableInt64Seq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableInt64(AverageNullableInt64Seq)"/>.
    /// </remarks>
    public static Seq<double?> Average(this Seq<long?> source) =>
        new AverageNullableInt64Seq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableInt64SelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableInt64Selector{TSource}(AverageNullableInt64SelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, long?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageNullableInt64SelectorSeq<TSource>(source, selector, text);

    /// <summary>
    /// Describes <c>source.Average()</c> over a sequence of nullable <c>float</c>.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <c>float</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableSingleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableSingle(AverageNullableSingleSeq)"/>.
    /// </remarks>
    public static Seq<float?> Average(this Seq<float?> source) =>
        new AverageNullableSingleSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableSingleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableSingleSelector{TSource}(AverageNullableSingleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<float?> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, float?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageNullableSingleSelectorSeq<TSource>(source, selector, text);

    /// <summary>
    /// Describes <c>source.Average()</c> over a sequence of nullable <c>double</c>.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <c>double</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableDoubleSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableDouble(AverageNullableDoubleSeq)"/>.
    /// </remarks>
    public static Seq<double?> Average(this Seq<double?> source) =>
        new AverageNullableDoubleSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableDoubleSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableDoubleSelector{TSource}(AverageNullableDoubleSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<double?> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, double?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageNullableDoubleSelectorSeq<TSource>(source, selector, text);

    /// <summary>
    /// Describes <c>source.Average()</c> over a sequence of nullable <c>decimal</c>.
    /// </summary>
    /// <param name="source">
    /// A sequence of nullable <c>decimal</c> values to calculate the average of.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableDecimalSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableDecimal(AverageNullableDecimalSeq)"/>.
    /// </remarks>
    public static Seq<decimal?> Average(this Seq<decimal?> source) =>
        new AverageNullableDecimalSeq(source);

    /// <summary>
    /// Describes <c>source.Average(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of values to calculate the average of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="AverageNullableDecimalSelectorSeq{TSource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AverageNullableDecimalSelector{TSource}(AverageNullableDecimalSelectorSeq{TSource})"/>.
    /// </remarks>
    public static Seq<decimal?> Average<TSource>(
        this Seq<TSource> source,
        Func<TSource, decimal?> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new AverageNullableDecimalSelectorSeq<TSource>(source, selector, text);
}
