// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="CombineLatestListSeq{T}"/> as the target's own
    /// <c>CombineLatest(IEnumerable)</c>.
    /// </summary>
    Realized<Seq<IList<T>>> CombineLatestList<T>(CombineLatestListSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatestArraySeq{T}"/> as the target's own
    /// <c>CombineLatest(params)</c>.
    /// </summary>
    Realized<Seq<IList<T>>> CombineLatestArray<T>(CombineLatestArraySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatestListSelectorSeq{T, TResult}"/> as the target's own
    /// <c>CombineLatest(IEnumerable, resultSelector)</c>.
    /// </summary>
    Realized<Seq<TResult>> CombineLatestListSelector<T, TResult>(
        CombineLatestListSelectorSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest2Seq{T1, T2, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 2 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest2<T1, T2, TResult>(CombineLatest2Seq<T1, T2, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest2TupleSeq{T1, T2}"/> as the target's own
    /// <c>CombineLatest</c> over 2 sources, producing tuples.
    /// </summary>
    Realized<Seq<(T1, T2)>> CombineLatest2Tuple<T1, T2>(CombineLatest2TupleSeq<T1, T2> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest3Seq{T1, T2, T3, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 3 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest3<
    T1, T2, T3, TResult
    >(CombineLatest3Seq<T1, T2, T3, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest3TupleSeq{T1, T2, T3}"/> as the target's own
    /// <c>CombineLatest</c> over 3 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3
        )>> CombineLatest3Tuple<
        T1, T2, T3
        >(
        CombineLatest3TupleSeq<T1, T2, T3> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest4Seq{T1, T2, T3, T4, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 4 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest4<
    T1, T2, T3, T4, TResult
    >(CombineLatest4Seq<T1, T2, T3, T4, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest4TupleSeq{T1, T2, T3, T4}"/> as the target's own
    /// <c>CombineLatest</c> over 4 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4
        )>> CombineLatest4Tuple<
        T1, T2, T3, T4
        >(
        CombineLatest4TupleSeq<T1, T2, T3, T4> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest5Seq{T1, T2, T3, T4, T5, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 5 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest5<
    T1, T2, T3, T4, T5, TResult
    >(CombineLatest5Seq<T1, T2, T3, T4, T5, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest5TupleSeq{T1, T2, T3, T4, T5}"/> as the target's own
    /// <c>CombineLatest</c> over 5 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5
        )>> CombineLatest5Tuple<
        T1, T2, T3, T4, T5
        >(
        CombineLatest5TupleSeq<T1, T2, T3, T4, T5> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest6Seq{T1, T2, T3, T4, T5, T6, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 6 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest6<
    T1, T2, T3, T4, T5, T6, TResult
    >(CombineLatest6Seq<T1, T2, T3, T4, T5, T6, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest6TupleSeq{T1, T2, T3, T4, T5, T6}"/> as the target's own
    /// <c>CombineLatest</c> over 6 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6
        )>> CombineLatest6Tuple<
        T1, T2, T3, T4, T5, T6
        >(
        CombineLatest6TupleSeq<T1, T2, T3, T4, T5, T6> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 7 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest7<
    T1, T2, T3, T4, T5, T6, T7, TResult
    >(CombineLatest7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest7TupleSeq{T1, T2, T3, T4, T5, T6, T7}"/> as the target's own
    /// <c>CombineLatest</c> over 7 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7
        )>> CombineLatest7Tuple<
        T1, T2, T3, T4, T5, T6, T7
        >(
        CombineLatest7TupleSeq<T1, T2, T3, T4, T5, T6, T7> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 8 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest8<
    T1, T2, T3, T4, T5, T6, T7, T8, TResult
    >(CombineLatest8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8}"/> as the target's own
    /// <c>CombineLatest</c> over 8 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )>> CombineLatest8Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        CombineLatest8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 9 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest9<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult
    >(CombineLatest9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> as the target's own
    /// <c>CombineLatest</c> over 9 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )>> CombineLatest9Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        CombineLatest9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 10 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest10<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult
    >(CombineLatest10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> as the target's own
    /// <c>CombineLatest</c> over 10 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )>> CombineLatest10Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        >(
        CombineLatest10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 11 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest11<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    >(CombineLatest11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> as the target's own
    /// <c>CombineLatest</c> over 11 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )>> CombineLatest11Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        >(
        CombineLatest11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 12 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest12<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult
    >(CombineLatest12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> as the target's own
    /// <c>CombineLatest</c> over 12 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )>> CombineLatest12Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        >(
        CombineLatest12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 13 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest13<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult
    >(CombineLatest13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> as the target's own
    /// <c>CombineLatest</c> over 13 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )>> CombineLatest13Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        >(
        CombineLatest13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 14 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest14<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(CombineLatest14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> as the target's own
    /// <c>CombineLatest</c> over 14 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )>> CombineLatest14Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        >(
        CombineLatest14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 15 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest15<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(
        CombineLatest15Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    > seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> as the target's own
    /// <c>CombineLatest</c> over 15 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )>> CombineLatest15Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        >(
        CombineLatest15TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
    > seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/> as the target's own
    /// <c>CombineLatest</c> over 16 sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> CombineLatest16<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
        CombineLatest16Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    > seq);

    /// <summary>
    /// Materializes a <see cref="CombineLatest16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> as the target's own
    /// <c>CombineLatest</c> over 16 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )>> CombineLatest16Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        >(
        CombineLatest16TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
    > seq);
}
