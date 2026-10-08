// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>CombineLatest</c> overloads that produce tuples.</summary>
/// <remarks>
/// Rx.NET keeps these on <c>ObservableEx</c> rather than <c>Observable</c> because
/// <c>CombineLatest(e0, e1, e2)</c> on the latter must mean the <c>params</c> form over sources
/// of one type, and a tuple form of the same arity on the same class would be chosen over it.
/// The shared description has the same two classes for the same reason: where an Rx.NET test
/// writes <c>ObservableEx.CombineLatest(e0, e1)</c>, a scenario writes
/// <c>SeqEx.CombineLatest(e0, e1)</c>. One node type per overload, in this folder.
/// </remarks>
public static class SeqEx
{
    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source2)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest2TupleSeq{T1, T2}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest2Tuple{T1, T2}(CombineLatest2TupleSeq{T1, T2})"/>.
    /// </remarks>
    public static Seq<(T1, T2)> CombineLatest<T1, T2>(
        this Seq<T1> source1,
        Seq<T2> source2) =>
        new CombineLatest2TupleSeq<T1, T2>(source1, source2);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source3)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest3TupleSeq{T1, T2, T3}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest3Tuple{T1, T2, T3}(CombineLatest3TupleSeq{T1, T2, T3})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3)> CombineLatest<T1, T2, T3>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3) =>
        new CombineLatest3TupleSeq<T1, T2, T3>(source1, source2, source3);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source4)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest4TupleSeq{T1, T2, T3, T4}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest4Tuple{T1, T2, T3, T4}(CombineLatest4TupleSeq{T1, T2, T3, T4})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4)> CombineLatest<T1, T2, T3, T4>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4) =>
        new CombineLatest4TupleSeq<T1, T2, T3, T4>(source1, source2, source3, source4);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source5)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest5TupleSeq{T1, T2, T3, T4, T5}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest5Tuple{T1, T2, T3, T4, T5}(CombineLatest5TupleSeq{T1, T2, T3, T4, T5})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5)> CombineLatest<T1, T2, T3, T4, T5>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5) =>
        new CombineLatest5TupleSeq<T1, T2, T3, T4, T5>(source1, source2, source3, source4, source5);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source6)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest6TupleSeq{T1, T2, T3, T4, T5, T6}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest6Tuple{T1, T2, T3, T4, T5, T6}(CombineLatest6TupleSeq{T1, T2, T3, T4, T5, T6})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6)> CombineLatest<T1, T2, T3, T4, T5, T6>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6) =>
        new CombineLatest6TupleSeq<T1, T2, T3, T4, T5, T6>(
            source1, source2, source3, source4, source5, source6);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source7)</c>, producing tuples.
    /// </summary>
    /// <param name="source1">First observable source.</param>
    /// <param name="source2">Second observable source.</param>
    /// <param name="source3">Third observable source.</param>
    /// <param name="source4">Fourth observable source.</param>
    /// <param name="source5">Fifth observable source.</param>
    /// <param name="source6">Sixth observable source.</param>
    /// <param name="source7">Seventh observable source.</param>
    /// <remarks>
    /// Builds a <see cref="CombineLatest7TupleSeq{T1, T2, T3, T4, T5, T6, T7}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest7Tuple{T1, T2, T3, T4, T5, T6, T7}(CombineLatest7TupleSeq{T1, T2, T3, T4, T5, T6, T7})"/>.
    /// </remarks>
    public static Seq<(T1, T2, T3, T4, T5, T6, T7)> CombineLatest<T1, T2, T3, T4, T5, T6, T7>(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7) =>
        new CombineLatest7TupleSeq<T1, T2, T3, T4, T5, T6, T7>(
            source1, source2, source3, source4, source5, source6, source7);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source8)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest8Tuple{T1, T2, T3, T4, T5, T6, T7, T8}(CombineLatest8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )> CombineLatest<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8) =>
        new CombineLatest8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8>(
            source1, source2, source3, source4, source5, source6, source7, source8);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source9)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest9Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9}(CombineLatest9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )> CombineLatest<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        this Seq<T1> source1,
        Seq<T2> source2,
        Seq<T3> source3,
        Seq<T4> source4,
        Seq<T5> source5,
        Seq<T6> source6,
        Seq<T7> source7,
        Seq<T8> source8,
        Seq<T9> source9) =>
        new CombineLatest9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source10)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest10Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(CombineLatest10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )> CombineLatest<
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
        new CombineLatest10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source11)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest11Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(CombineLatest11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )> CombineLatest<
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
        new CombineLatest11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source12)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest12Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(CombineLatest12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )> CombineLatest<
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
        new CombineLatest12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source13)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest13Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(CombineLatest13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )> CombineLatest<
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
        new CombineLatest13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source14)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest14Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(CombineLatest14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )> CombineLatest<
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
        new CombineLatest14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source15)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest15Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}(CombineLatest15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )> CombineLatest<
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
        new CombineLatest15TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
    >(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15);

    /// <summary>
    /// Describes <c>SeqEx.CombineLatest(source1, ..., source16)</c>, producing tuples.
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
    /// Builds a <see cref="CombineLatest16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.CombineLatest16Tuple{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}(CombineLatest16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16})"/>.
    /// </remarks>
    public static Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )> CombineLatest<
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
        new CombineLatest16TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
    >(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16);
}
