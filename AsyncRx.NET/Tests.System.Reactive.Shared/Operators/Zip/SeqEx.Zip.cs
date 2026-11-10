// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Zip</c> overloads that produce tuples.</summary>
/// <remarks>
/// On <see cref="SeqEx"/> rather than <see cref="Seq"/> for the reason given on the class: a
/// tuple form of arity three on <see cref="Seq"/> would be chosen over the <c>params</c> form.
/// </remarks>
public static partial class SeqEx
{
    /// <summary>
    /// Describes <c>SeqEx.Zip(first, second)</c> where the second source is an enumerable,
    /// producing pairs.
    /// </summary>
    /// <param name="first">First observable source.</param>
    /// <param name="second">Second enumerable source.</param>
    /// <remarks>
    /// Builds a <see cref="ZipEnumerableTupleSeq{T1, T2}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.ZipEnumerableTuple{T1, T2}(ZipEnumerableTupleSeq{T1, T2})"/>.
    /// </remarks>
    public static Seq<(T1, T2)> Zip<T1, T2>(this Seq<T1> first, IEnumerable<T2> second) =>
        new ZipEnumerableTupleSeq<T1, T2>(first, second);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source2)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip2TupleSeq{T1, T2}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip2Tuple{T1, T2}(Zip2TupleSeq{T1, T2})"/>.
    /// </remarks>
    public static Seq<(T1, T2)> Zip<T1, T2>(
        this Seq<T1> source1,
        Seq<T2> source2) =>
        new Zip2TupleSeq<T1, T2>(source1, source2);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source3)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip3TupleSeq{T1, T2, T3}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip3Tuple{T1, T2, T3}(Zip3TupleSeq{T1, T2, T3})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3)> Zip<T1, T2, T3>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3) =>
        new Zip3TupleSeq<T1, T2, T3>(source1, source2, source3);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source4)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip4TupleSeq{T1, T2, T3, T4}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip4Tuple{T1, T2, T3, T4}(Zip4TupleSeq{T1, T2, T3, T4})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4) =>
        new Zip4TupleSeq<T1, T2, T3, T4>(source1, source2, source3, source4);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source5)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip5TupleSeq{T1, T2, T3, T4, T5}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip5Tuple{T1, T2, T3, T4, T5}(Zip5TupleSeq{T1, T2, T3, T4, T5})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5) =>
        new Zip5TupleSeq<T1, T2, T3, T4, T5>(source1, source2, source3, source4, source5);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source6)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip6TupleSeq{T1, T2, T3, T4, T5, T6}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip6Tuple{T1, T2, T3, T4, T5, T6}(Zip6TupleSeq{T1, T2, T3, T4, T5, T6})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6) =>
        new Zip6TupleSeq<T1, T2, T3, T4, T5, T6>(
            source1, source2, source3, source4, source5, source6);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source7)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip7TupleSeq{T1, T2, T3, T4, T5, T6, T7}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip7Tuple{T1, T2, T3, T4, T5, T6, T7}(Zip7TupleSeq{T1, T2, T3, T4, T5, T6, T7})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7) =>
        new Zip7TupleSeq<T1, T2, T3, T4, T5, T6, T7>(
            source1, source2, source3, source4, source5, source6, source7);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source8)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <param name="source8">Eighth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="Zip8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip8Tuple{T1, T2, T3, T4, T5, T6, T7, T8}(Zip8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6, T7, T8)> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8) =>
        new Zip8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8>(
            source1, source2, source3, source4, source5, source6, source7, source8);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source9)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip9Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9}(Zip9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9) =>
        new Zip9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source10)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip10Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(Zip10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
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
        Seq<T10> source10) =>
        new Zip10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source11)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip11Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(Zip11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
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
        Seq<T11> source11) =>
        new Zip11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source12)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip12Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(Zip12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
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
        Seq<T12> source12) =>
        new Zip12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source13)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip13Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(Zip13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
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
        Seq<T13> source13) =>
        new Zip13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source14)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip14Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(Zip14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
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
        Seq<T14> source14) =>
        new Zip14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source15)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip15Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}(Zip15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
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
        Seq<T15> source15) =>
        new Zip15TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15);

    /// <summary>
    /// Describes <c>SeqEx.Zip(source1, ..., source16)</c>, producing tuples.
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
    /// <remarks>
    /// Builds a <see cref="Zip16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Zip16Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}(Zip16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )> Zip<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
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
        Seq<T16> source16) =>
        new Zip16TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16);
}
