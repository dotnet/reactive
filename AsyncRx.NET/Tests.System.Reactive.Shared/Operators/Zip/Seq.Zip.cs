// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Zip</c> overloads that take a result selector, a list of sources, or an enumerable.
/// </summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observable.Zip(e0, e1, (x, y) => x + y)</c> or
/// <c>o1.Zip(o2, ...)</c>, a shared scenario writes the same with <see cref="Seq"/> in place of
/// <c>Observable</c>; the methods are extension methods, as Rx.NET's are, so both spellings work.
/// The forms that produce tuples live on <see cref="SeqEx"/>, as Rx.NET keeps them on
/// <c>ObservableEx</c>, and for the same reason: <c>Zip(e0, e1, e2)</c> on this class must mean
/// the <c>params</c> form over sources of one type, which a tuple form of the same arity on the
/// same class would hide. One node type per overload, in this folder. <c>Zip</c> was plumbing
/// before it came under test (the two-source selector form, which other scenarios use); that
/// form is now the first of the family here.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>Seq.Zip(sources)</c> over a sequence of sources, producing lists.
    /// </summary>
    /// <param name="sources">Observable sources.</param>
    /// <remarks>
    /// Builds a <see cref="ZipListSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ZipList{T}(ZipListSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Zip<T>(this IEnumerable<Seq<T>> sources) =>
        new ZipListSeq<T>(sources);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, source2, ...)</c> over any number of sources of one type,
    /// producing lists.
    /// </summary>
    /// <param name="sources">Observable sources.</param>
    /// <remarks>
    /// Builds a <see cref="ZipArraySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ZipArray{T}(ZipArraySeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Zip<T>(params Seq<T>[] sources) =>
        new ZipArraySeq<T>(sources);

    /// <summary>Describes <c>Seq.Zip(sources, resultSelector)</c>.</summary>
    /// <param name="sources">Observable sources.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="ZipListSelectorSeq{T, TResult}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.ZipListSelector{T, TResult}(ZipListSelectorSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T, TResult>(
        this IEnumerable<Seq<T>> sources,
        Func<IList<T>, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new ZipListSelectorSeq<T, TResult>(sources, resultSelector, text);

    /// <summary>
    /// Describes <c>first.Zip(second, resultSelector)</c> where the second source is an
    /// enumerable.
    /// </summary>
    /// <param name="first">First observable source.</param>
    /// <param name="second">Second enumerable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each consecutive pair of elements from the first and second source.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="ZipEnumerableSeq{T1, T2, TResult}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.ZipEnumerable{T1, T2, TResult}(ZipEnumerableSeq{T1, T2, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, TResult>(
        this Seq<T1> first,
        IEnumerable<T2> second,
        Func<T1, T2, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new ZipEnumerableSeq<T1, T2, TResult>(first, second, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source2, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each consecutive pair of elements from the first and second source.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip2Seq{T1, T2, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip2{T1, T2, TResult}(Zip2Seq{T1, T2, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Func<T1, T2, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip2Seq<T1, T2, TResult>(source1, source2, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source3, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip3Seq{T1, T2, T3, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip3{T1, T2, T3, TResult}(Zip3Seq{T1, T2, T3, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Func<T1, T2, T3, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip3Seq<T1, T2, T3, TResult>(source1, source2, source3, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source4, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip4Seq{T1, T2, T3, T4, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip4{T1, T2, T3, T4, TResult}(Zip4Seq{T1, T2, T3, T4, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Func<T1, T2, T3, T4, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip4Seq<T1, T2, T3, T4, TResult>(
            source1, source2, source3, source4, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source5, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip5Seq{T1, T2, T3, T4, T5, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip5{T1, T2, T3, T4, T5, TResult}(Zip5Seq{T1, T2, T3, T4, T5, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Func<T1, T2, T3, T4, T5, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip5Seq<T1, T2, T3, T4, T5, TResult>(
            source1, source2, source3, source4, source5, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source6, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip6Seq{T1, T2, T3, T4, T5, T6, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip6{T1, T2, T3, T4, T5, T6, TResult}(Zip6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip6Seq<T1, T2, T3, T4, T5, T6, TResult>(
            source1, source2, source3, source4, source5, source6, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source7, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip7{T1, T2, T3, T4, T5, T6, T7, TResult}(Zip7Seq{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip7Seq<T1, T2, T3, T4, T5, T6, T7, TResult>(
            source1, source2, source3, source4, source5, source6, source7, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source8, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(Zip8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, resultSelector,
            text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source9, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip9{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}(Zip9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source10, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}(Zip10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source11, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(Zip11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source12, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="source12">Twelfth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}(Zip12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Seq<T12> source12,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source13, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="source12">Twelfth observable source.</param>
    /// <param name="source13">Thirteenth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}(Zip13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Seq<T12> source12,
        Seq<T13> source13,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source14, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="source12">Twelfth observable source.</param>
    /// <param name="source13">Thirteenth observable source.</param>
    /// <param name="source14">Fourteenth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}(Zip14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Seq<T12> source12,
        Seq<T13> source13,
        Seq<T14> source14,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source15, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="source12">Twelfth observable source.</param>
    /// <param name="source13">Thirteenth observable source.</param>
    /// <param name="source14">Fourteenth observable source.</param>
    /// <param name="source15">Fifteenth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}(Zip15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Seq<T12> source12,
        Seq<T13> source13,
        Seq<T14> source14,
        Seq<T15> source15,
        Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
            > resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, resultSelector, text);

    /// <summary>
    /// Describes <c>Seq.Zip(source1, ..., source16, resultSelector)</c>.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <param name="source9">Ninth observable source.</param>
    /// <param name="source10">Tenth observable source.</param>
    /// <param name="source11">Eleventh observable source.</param>
    /// <param name="source12">Twelfth observable source.</param>
    /// <param name="source13">Thirteenth observable source.</param>
    /// <param name="source14">Fourteenth observable source.</param>
    /// <param name="source15">Fifteenth observable source.</param>
    /// <param name="source16">Sixteenth observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </param>
    /// <param name="text">The selector's text, captured for the description.</param>
    /// <remarks>
    /// Builds a <see cref="Zip16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}(Zip16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Zip<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9,
        Seq<T10> source10,
        Seq<T11> source11,
        Seq<T12> source12,
        Seq<T13> source13,
        Seq<T14> source14,
        Seq<T15> source15,
        Seq<T16> source16,
        Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
            > resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new Zip16Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16, resultSelector,
            text);
}
