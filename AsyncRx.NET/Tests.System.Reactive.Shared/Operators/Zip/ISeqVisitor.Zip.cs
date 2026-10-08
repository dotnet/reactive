// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ZipListSeq{T}"/> as the target's own <c>Zip(IEnumerable)</c>.
    /// </summary>
    Realized<Seq<IList<T>>> ZipList<T>(ZipListSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ZipArraySeq{T}"/> as the target's own <c>Zip(params)</c>.
    /// </summary>
    Realized<Seq<IList<T>>> ZipArray<T>(ZipArraySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ZipListSelectorSeq{T, TResult}"/> as the target's own
    /// <c>Zip(IEnumerable, resultSelector)</c>.
    /// </summary>
    Realized<Seq<TResult>> ZipListSelector<T, TResult>(ZipListSelectorSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ZipEnumerableSeq{T1, T2, TResult}"/> as the target's own
    /// <c>Zip</c> over an observable and an enumerable with a result selector.
    /// </summary>
    Realized<Seq<TResult>> ZipEnumerable<T1, T2, TResult>(ZipEnumerableSeq<T1, T2, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ZipEnumerableTupleSeq{T1, T2}"/> as the target's own
    /// <c>Zip</c> over an observable and an enumerable, producing pairs.
    /// </summary>
    Realized<Seq<(T1, T2)>> ZipEnumerableTuple<T1, T2>(ZipEnumerableTupleSeq<T1, T2> seq);

    /// <summary>
    /// Materializes a <see cref="Zip2Seq{T1, T2, TResult}"/> as the target's own <c>Zip</c> over 2
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip2<T1, T2, TResult>(Zip2Seq<T1, T2, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip2TupleSeq{T1, T2}"/> as the target's own <c>Zip</c> over
    /// 2 sources, producing tuples.
    /// </summary>
    Realized<Seq<(T1, T2)>> Zip2Tuple<T1, T2>(Zip2TupleSeq<T1, T2> seq);

    /// <summary>
    /// Materializes a <see cref="Zip3Seq{T1, T2, T3, TResult}"/> as the target's own <c>Zip</c> over 3
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip3<T1, T2, T3, TResult>(Zip3Seq<T1, T2, T3, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip3TupleSeq{T1, T2, T3}"/> as the target's own <c>Zip</c> over
    /// 3 sources, producing tuples.
    /// </summary>
    Realized<Seq<(T1, T2, T3)>> Zip3Tuple<T1, T2, T3>(Zip3TupleSeq<T1, T2, T3> seq);

    /// <summary>
    /// Materializes a <see cref="Zip4Seq{T1, T2, T3, T4, TResult}"/> as the target's own <c>Zip</c> over 4
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip4<T1, T2, T3, T4, TResult>(Zip4Seq<T1, T2, T3, T4, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip4TupleSeq{T1, T2, T3, T4}"/> as the target's own <c>Zip</c> over
    /// 4 sources, producing tuples.
    /// </summary>
    Realized<Seq<(T1, T2, T3, T4)>> Zip4Tuple<T1, T2, T3, T4>(Zip4TupleSeq<T1, T2, T3, T4> seq);

    /// <summary>
    /// Materializes a <see cref="Zip5Seq{T1, T2, T3, T4, T5, TResult}"/> as the target's own <c>Zip</c> over 5
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip5<
    T1, T2, T3, T4, T5, TResult
    >(Zip5Seq<T1, T2, T3, T4, T5, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip5TupleSeq{T1, T2, T3, T4, T5}"/> as the target's own <c>Zip</c> over
    /// 5 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5
        )>> Zip5Tuple<
        T1, T2, T3, T4, T5
        >(
        Zip5TupleSeq<T1, T2, T3, T4, T5> seq);

    /// <summary>
    /// Materializes a <see cref="Zip6Seq{T1, T2, T3, T4, T5, T6, TResult}"/> as the target's own <c>Zip</c> over 6
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip6<
    T1, T2, T3, T4, T5, T6, TResult
    >(Zip6Seq<T1, T2, T3, T4, T5, T6, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip6TupleSeq{T1, T2, T3, T4, T5, T6}"/> as the target's own <c>Zip</c> over
    /// 6 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6
        )>> Zip6Tuple<
        T1, T2, T3, T4, T5, T6
        >(
        Zip6TupleSeq<T1, T2, T3, T4, T5, T6> seq);

    /// <summary>
    /// Materializes a <see cref="Zip7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/> as the target's own <c>Zip</c> over 7
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip7<
    T1, T2, T3, T4, T5, T6, T7, TResult
    >(Zip7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip7TupleSeq{T1, T2, T3, T4, T5, T6, T7}"/> as the target's own <c>Zip</c> over
    /// 7 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7
        )>> Zip7Tuple<
        T1, T2, T3, T4, T5, T6, T7
        >(
        Zip7TupleSeq<T1, T2, T3, T4, T5, T6, T7> seq);

    /// <summary>
    /// Materializes a <see cref="Zip8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> as the target's own <c>Zip</c> over 8
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip8<
    T1, T2, T3, T4, T5, T6, T7, T8, TResult
    >(Zip8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip8TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8}"/> as the target's own <c>Zip</c> over
    /// 8 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )>> Zip8Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        Zip8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8> seq);

    /// <summary>
    /// Materializes a <see cref="Zip9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> as the target's own <c>Zip</c> over 9
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip9<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult
    >(Zip9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip9TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> as the target's own <c>Zip</c> over
    /// 9 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )>> Zip9Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        Zip9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq);

    /// <summary>
    /// Materializes a <see cref="Zip10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> as the target's own <c>Zip</c> over 10
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip10<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult
    >(Zip10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip10TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> as the target's own <c>Zip</c> over
    /// 10 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )>> Zip10Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        >(
        Zip10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq);

    /// <summary>
    /// Materializes a <see cref="Zip11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> as the target's own <c>Zip</c> over 11
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip11<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    >(Zip11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip11TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> as the target's own <c>Zip</c> over
    /// 11 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )>> Zip11Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        >(
        Zip11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq);

    /// <summary>
    /// Materializes a <see cref="Zip12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> as the target's own <c>Zip</c> over 12
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip12<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult
    >(Zip12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip12TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> as the target's own <c>Zip</c> over
    /// 12 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )>> Zip12Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        >(
        Zip12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq);

    /// <summary>
    /// Materializes a <see cref="Zip13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> as the target's own <c>Zip</c> over 13
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip13<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult
    >(Zip13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip13TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> as the target's own <c>Zip</c> over
    /// 13 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )>> Zip13Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        >(
        Zip13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> seq);

    /// <summary>
    /// Materializes a <see cref="Zip14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> as the target's own <c>Zip</c> over 14
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip14<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(Zip14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip14TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> as the target's own <c>Zip</c> over
    /// 14 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )>> Zip14Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        >(
        Zip14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> seq);

    /// <summary>
    /// Materializes a <see cref="Zip15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> as the target's own <c>Zip</c> over 15
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip15<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(Zip15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip15TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> as the target's own <c>Zip</c> over
    /// 15 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )>> Zip15Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        >(
        Zip15TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> seq);

    /// <summary>
    /// Materializes a <see cref="Zip16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/> as the target's own <c>Zip</c> over 16
    /// sources with a result selector.
    /// </summary>
    Realized<Seq<TResult>> Zip16<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(Zip16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="Zip16TupleSeq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> as the target's own <c>Zip</c> over
    /// 16 sources, producing tuples.
    /// </summary>
    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )>> Zip16Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        >(
        Zip16TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> seq);
}
