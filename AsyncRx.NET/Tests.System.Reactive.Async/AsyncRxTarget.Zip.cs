// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<IList<T>>> ISeqVisitor.ZipList<T>(ZipListSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(ZipListImpl<T>, seq.Sources);

    Realized<Seq<IList<T>>> ISeqVisitor.ZipArray<T>(ZipArraySeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(ZipArrayImpl<T>, [seq.Sources]);

    Realized<Seq<TResult>> ISeqVisitor.ZipListSelector<T, TResult>(
        ZipListSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(ZipListSelectorImpl<T, TResult>, seq.Sources, seq.ResultSelector);

    Realized<Seq<TResult>> ISeqVisitor.ZipEnumerable<T1, T2, TResult>(
        ZipEnumerableSeq<T1, T2, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ZipEnumerableImpl<T1, T2, TResult>,
            seq.First,
            seq.Second,
            seq.ResultSelector);

    Realized<Seq<(T1, T2)>> ISeqVisitor.ZipEnumerableTuple<T1, T2>(
        ZipEnumerableTupleSeq<T1, T2> seq) =>
        _bridge.Run<Seq<(T1, T2)>>(ZipEnumerableTupleImpl<T1, T2>, seq.First, seq.Second);

    Realized<Seq<TResult>> ISeqVisitor.Zip2<T1, T2, TResult>(Zip2Seq<T1, T2, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip2Impl<T1, T2, TResult>,
            seq.Source1, seq.Source2, seq.ResultSelector);

    Realized<Seq<(T1, T2)>> ISeqVisitor.Zip2Tuple<T1, T2>(Zip2TupleSeq<T1, T2> seq) =>
        _bridge.Run<Seq<(T1, T2)>>(Zip2TupleImpl<T1, T2>, seq.Source1, seq.Source2);

    Realized<Seq<TResult>> ISeqVisitor.Zip3<
    T1, T2, T3, TResult
    >(Zip3Seq<T1, T2, T3, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip3Impl<T1, T2, T3, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.ResultSelector);

    Realized<Seq<(T1, T2, T3)>> ISeqVisitor.Zip3Tuple<T1, T2, T3>(Zip3TupleSeq<T1, T2, T3> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3
            )>>(
            Zip3TupleImpl<T1, T2, T3>,
            seq.Source1, seq.Source2, seq.Source3);

    Realized<Seq<TResult>> ISeqVisitor.Zip4<
    T1, T2, T3, T4, TResult
    >(Zip4Seq<T1, T2, T3, T4, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip4Impl<T1, T2, T3, T4, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4
        )>> ISeqVisitor.Zip4Tuple<
        T1, T2, T3, T4
        >(
        Zip4TupleSeq<T1, T2, T3, T4> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4
            )>>(
            Zip4TupleImpl<T1, T2, T3, T4>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4);

    Realized<Seq<TResult>> ISeqVisitor.Zip5<
    T1, T2, T3, T4, T5, TResult
    >(Zip5Seq<T1, T2, T3, T4, T5, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip5Impl<T1, T2, T3, T4, T5, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5
        )>> ISeqVisitor.Zip5Tuple<
        T1, T2, T3, T4, T5
        >(
        Zip5TupleSeq<T1, T2, T3, T4, T5> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5
            )>>(
            Zip5TupleImpl<T1, T2, T3, T4, T5>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5);

    Realized<Seq<TResult>> ISeqVisitor.Zip6<
    T1, T2, T3, T4, T5, T6, TResult
    >(Zip6Seq<T1, T2, T3, T4, T5, T6, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip6Impl<T1, T2, T3, T4, T5, T6, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6
        )>> ISeqVisitor.Zip6Tuple<
        T1, T2, T3, T4, T5, T6
        >(
        Zip6TupleSeq<T1, T2, T3, T4, T5, T6> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6
            )>>(
            Zip6TupleImpl<T1, T2, T3, T4, T5, T6>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6);

    Realized<Seq<TResult>> ISeqVisitor.Zip7<
    T1, T2, T3, T4, T5, T6, T7, TResult
    >(Zip7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7
        )>> ISeqVisitor.Zip7Tuple<
        T1, T2, T3, T4, T5, T6, T7
        >(
        Zip7TupleSeq<T1, T2, T3, T4, T5, T6, T7> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7
            )>>(
            Zip7TupleImpl<T1, T2, T3, T4, T5, T6, T7>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7);

    Realized<Seq<TResult>> ISeqVisitor.Zip8<
    T1, T2, T3, T4, T5, T6, T7, T8, TResult
    >(Zip8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )>> ISeqVisitor.Zip8Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        Zip8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8
            )>>(
            Zip8TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8);

    Realized<Seq<TResult>> ISeqVisitor.Zip9<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult
    >(Zip9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )>> ISeqVisitor.Zip9Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        Zip9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9
            )>>(
            Zip9TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9);

    Realized<Seq<TResult>> ISeqVisitor.Zip10<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult
    >(Zip10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )>> ISeqVisitor.Zip10Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        >(
        Zip10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
            )>>(
            Zip10TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10);

    Realized<Seq<TResult>> ISeqVisitor.Zip11<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    >(Zip11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )>> ISeqVisitor.Zip11Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        >(
        Zip11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
            )>>(
            Zip11TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11);

    Realized<Seq<TResult>> ISeqVisitor.Zip12<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult
    >(Zip12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )>> ISeqVisitor.Zip12Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        >(
        Zip12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
            )>>(
            Zip12TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12);

    Realized<Seq<TResult>> ISeqVisitor.Zip13<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult
    >(Zip13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )>> ISeqVisitor.Zip13Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        >(
        Zip13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
            )>>(
            Zip13TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13);

    Realized<Seq<TResult>> ISeqVisitor.Zip14<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(Zip14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )>> ISeqVisitor.Zip14Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        >(
        Zip14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
            )>>(
            Zip14TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14);

    Realized<Seq<TResult>> ISeqVisitor.Zip15<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(Zip15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )>> ISeqVisitor.Zip15Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        >(
        Zip15TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
            )>>(
            Zip15TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15);

    Realized<Seq<TResult>> ISeqVisitor.Zip16<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
        Zip16Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    > seq) =>
        _bridge.Run<Seq<TResult>>(
            Zip16Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15, seq.Source16, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )>> ISeqVisitor.Zip16Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        >(
        Zip16TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
            )>>(
            Zip16TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15, seq.Source16);

    private static IAsyncObservable<IList<T>> ZipListImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources) =>
        AsyncObservable.Zip(sources);

    private static IAsyncObservable<IList<T>> ZipArrayImpl<T>(
        IAsyncObservable<T>[] sources) =>
        AsyncObservable.Zip(sources);

    private static IAsyncObservable<TResult> ZipListSelectorImpl<T, TResult>(
        IEnumerable<IAsyncObservable<T>> sources,
        Func<IList<T>, TResult> resultSelector) =>
        AsyncObservable.Zip(sources, resultSelector);

    private static IAsyncObservable<TResult> ZipEnumerableImpl<T1, T2, TResult>(
        IAsyncObservable<T1> first,
        IEnumerable<T2> second,
        Func<T1, T2, TResult> resultSelector) =>
        first.Zip(second, resultSelector);

    private static IAsyncObservable<(T1, T2)> ZipEnumerableTupleImpl<T1, T2>(
        IAsyncObservable<T1> first,
        IEnumerable<T2> second) =>
        AsyncObservable.Zip(first, second);

    private static IAsyncObservable<TResult> Zip2Impl<T1, T2, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        Func<T1, T2, TResult> resultSelector) =>
        AsyncObservable.Zip(source1, source2, resultSelector);

    private static IAsyncObservable<(T1, T2)> Zip2TupleImpl<T1, T2>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2) =>
        AsyncObservable.Zip(source1, source2);

    private static IAsyncObservable<TResult> Zip3Impl<T1, T2, T3, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        Func<T1, T2, T3, TResult> resultSelector) =>
        AsyncObservable.Zip(source1, source2, source3, resultSelector);

    private static IAsyncObservable<(T1, T2, T3)> Zip3TupleImpl<T1, T2, T3>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3) =>
        AsyncObservable.Zip(source1, source2, source3);

    private static IAsyncObservable<TResult> Zip4Impl<T1, T2, T3, T4, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        Func<T1, T2, T3, T4, TResult> resultSelector) =>
        AsyncObservable.Zip(source1, source2, source3, source4, resultSelector);

    private static IAsyncObservable<(T1, T2, T3, T4)> Zip4TupleImpl<T1, T2, T3, T4>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4) =>
        AsyncObservable.Zip(source1, source2, source3, source4);

    private static IAsyncObservable<TResult> Zip5Impl<T1, T2, T3, T4, T5, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        Func<T1, T2, T3, T4, T5, TResult> resultSelector) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5, resultSelector);

    private static IAsyncObservable<(T1, T2, T3, T4, T5)> Zip5TupleImpl<T1, T2, T3, T4, T5>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5);

    private static IAsyncObservable<TResult> Zip6Impl<T1, T2, T3, T4, T5, T6, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5, source6, resultSelector);

    private static IAsyncObservable<(T1, T2, T3, T4, T5, T6)> Zip6TupleImpl<T1, T2, T3, T4, T5, T6>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5, source6);

    private static IAsyncObservable<TResult> Zip7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7
        )> Zip7TupleImpl<
        T1, T2, T3, T4, T5, T6, T7
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5, source6, source7);

    private static IAsyncObservable<TResult> Zip8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )> Zip8TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8) =>
        AsyncObservable.Zip(source1, source2, source3, source4, source5, source6, source7, source8);

    private static IAsyncObservable<TResult> Zip9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )> Zip9TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9);

    private static IAsyncObservable<TResult> Zip10Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )> Zip10TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10);

    private static IAsyncObservable<TResult> Zip11Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )> Zip11TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11);

    private static IAsyncObservable<TResult> Zip12Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )> Zip12TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12);

    private static IAsyncObservable<TResult> Zip13Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )> Zip13TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13);

    private static IAsyncObservable<TResult> Zip14Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14,
        Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
            > resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )> Zip14TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14);

    private static IAsyncObservable<TResult> Zip15Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14,
        IAsyncObservable<T15> source15,
        Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
            > resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )> Zip15TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14,
        IAsyncObservable<T15> source15) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15);

    private static IAsyncObservable<TResult> Zip16Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14,
        IAsyncObservable<T15> source15,
        IAsyncObservable<T16> source16,
        Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
            > resultSelector) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )> Zip16TupleImpl<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        IAsyncObservable<T9> source9,
        IAsyncObservable<T10> source10,
        IAsyncObservable<T11> source11,
        IAsyncObservable<T12> source12,
        IAsyncObservable<T13> source13,
        IAsyncObservable<T14> source14,
        IAsyncObservable<T15> source15,
        IAsyncObservable<T16> source16) =>
        AsyncObservable.Zip(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16);
}
