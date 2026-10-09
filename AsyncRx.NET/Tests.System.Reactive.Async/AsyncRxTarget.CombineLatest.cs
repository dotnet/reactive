// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<IList<T>>> ISeqVisitor.CombineLatestList<T>(CombineLatestListSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(CombineLatestListImpl<T>, seq.Sources);

    Realized<Seq<IList<T>>> ISeqVisitor.CombineLatestArray<T>(CombineLatestArraySeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(CombineLatestArrayImpl<T>, [seq.Sources]);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatestListSelector<T, TResult>(
        CombineLatestListSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            CombineLatestListSelectorImpl<T, TResult>,
            seq.Sources,
            seq.ResultSelector);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest2<
    T1, T2, TResult
    >(CombineLatest2Seq<T1, T2, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest2Impl<T1, T2, TResult>,
    seq.Source1,
    seq.Source2,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2
        )>> ISeqVisitor.CombineLatest2Tuple<
        T1, T2
        >(
        CombineLatest2TupleSeq<T1, T2> seq) =>
        _bridge.Run<Seq<(T1, T2)>>(CombineLatest2TupleImpl<T1, T2>, seq.Source1, seq.Source2);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest3<
    T1, T2, T3, TResult
    >(CombineLatest3Seq<T1, T2, T3, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest3Impl<T1, T2, T3, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3
        )>> ISeqVisitor.CombineLatest3Tuple<
        T1, T2, T3
        >(
        CombineLatest3TupleSeq<T1, T2, T3> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3
            )>>(
            CombineLatest3TupleImpl<T1, T2, T3>,
            seq.Source1, seq.Source2, seq.Source3);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest4<
    T1, T2, T3, T4, TResult
    >(CombineLatest4Seq<T1, T2, T3, T4, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest4Impl<T1, T2, T3, T4, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4
        )>> ISeqVisitor.CombineLatest4Tuple<
        T1, T2, T3, T4
        >(
        CombineLatest4TupleSeq<T1, T2, T3, T4> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4
            )>>(
            CombineLatest4TupleImpl<T1, T2, T3, T4>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest5<
    T1, T2, T3, T4, T5, TResult
    >(CombineLatest5Seq<T1, T2, T3, T4, T5, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest5Impl<T1, T2, T3, T4, T5, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5
        )>> ISeqVisitor.CombineLatest5Tuple<
        T1, T2, T3, T4, T5
        >(
        CombineLatest5TupleSeq<T1, T2, T3, T4, T5> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5
            )>>(
            CombineLatest5TupleImpl<T1, T2, T3, T4, T5>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest6<
    T1, T2, T3, T4, T5, T6, TResult
    >(CombineLatest6Seq<T1, T2, T3, T4, T5, T6, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest6Impl<T1, T2, T3, T4, T5, T6, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6
        )>> ISeqVisitor.CombineLatest6Tuple<
        T1, T2, T3, T4, T5, T6
        >(
        CombineLatest6TupleSeq<T1, T2, T3, T4, T5, T6> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6
            )>>(
            CombineLatest6TupleImpl<T1, T2, T3, T4, T5, T6>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest7<
    T1, T2, T3, T4, T5, T6, T7, TResult
    >(CombineLatest7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7
        )>> ISeqVisitor.CombineLatest7Tuple<
        T1, T2, T3, T4, T5, T6, T7
        >(
        CombineLatest7TupleSeq<T1, T2, T3, T4, T5, T6, T7> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7
            )>>(
            CombineLatest7TupleImpl<T1, T2, T3, T4, T5, T6, T7>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest8<
    T1, T2, T3, T4, T5, T6, T7, T8, TResult
    >(CombineLatest8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )>> ISeqVisitor.CombineLatest8Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8
        >(
        CombineLatest8TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8
            )>>(
            CombineLatest8TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest9<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult
    >(CombineLatest9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )>> ISeqVisitor.CombineLatest9Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        >(
        CombineLatest9TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9
            )>>(
            CombineLatest9TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest10<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult
    >(CombineLatest10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )>> ISeqVisitor.CombineLatest10Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        >(
        CombineLatest10TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
            )>>(
            CombineLatest10TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest11<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    >(CombineLatest11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.Source11,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )>> ISeqVisitor.CombineLatest11Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        >(
        CombineLatest11TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
            )>>(
            CombineLatest11TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest12<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult
    >(CombineLatest12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.Source11,
    seq.Source12,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )>> ISeqVisitor.CombineLatest12Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        >(
        CombineLatest12TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
            )>>(
            CombineLatest12TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest13<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult
    >(CombineLatest13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.Source11,
    seq.Source12,
    seq.Source13,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )>> ISeqVisitor.CombineLatest13Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        >(
        CombineLatest13TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
            )>>(
            CombineLatest13TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest14<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    >(
        CombineLatest14Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult
    > seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.Source11,
    seq.Source12,
    seq.Source13,
    seq.Source14,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )>> ISeqVisitor.CombineLatest14Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        >(
        CombineLatest14TupleSeq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
            )>>(
            CombineLatest14TupleImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest15<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    >(
        CombineLatest15Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult
    > seq) =>
        _bridge.Run<Seq<TResult>>(
    CombineLatest15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>,
    seq.Source1,
    seq.Source2,
    seq.Source3,
    seq.Source4,
    seq.Source5,
    seq.Source6,
    seq.Source7,
    seq.Source8,
    seq.Source9,
    seq.Source10,
    seq.Source11,
    seq.Source12,
    seq.Source13,
    seq.Source14,
    seq.Source15,
    seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )>> ISeqVisitor.CombineLatest15Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        >(
        CombineLatest15TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
    > seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
            )>>(
            CombineLatest15TupleImpl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
    >,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15);

    Realized<Seq<TResult>> ISeqVisitor.CombineLatest16<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >(
        CombineLatest16Seq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    > seq) =>
        _bridge.Run<Seq<TResult>>(
            CombineLatest16Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult
    >,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15, seq.Source16, seq.ResultSelector);

    Realized<Seq<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )>> ISeqVisitor.CombineLatest16Tuple<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        >(
        CombineLatest16TupleSeq<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
    > seq) =>
        _bridge.Run<Seq<(
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
            )>>(
            CombineLatest16TupleImpl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
    >,
            seq.Source1, seq.Source2, seq.Source3, seq.Source4, seq.Source5, seq.Source6,
            seq.Source7, seq.Source8, seq.Source9, seq.Source10, seq.Source11, seq.Source12,
            seq.Source13, seq.Source14, seq.Source15, seq.Source16);

    private static IAsyncObservable<IList<T>> CombineLatestListImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources) =>
        AsyncObservable.CombineLatest(sources);

    private static IAsyncObservable<IList<T>> CombineLatestArrayImpl<T>(
        IAsyncObservable<T>[] sources) =>
        AsyncObservable.CombineLatest(sources);

    private static IAsyncObservable<TResult> CombineLatestListSelectorImpl<T, TResult>(
        IEnumerable<IAsyncObservable<T>> sources,
        Func<IList<T>, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(sources, resultSelector);

    private static IAsyncObservable<TResult> CombineLatest2Impl<T1, T2, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        Func<T1, T2, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(source1, source2, resultSelector);

    private static IAsyncObservable<(T1, T2)> CombineLatest2TupleImpl<T1, T2>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2) =>
        AsyncObservable.CombineLatest(source1, source2);

    private static IAsyncObservable<TResult> CombineLatest3Impl<T1, T2, T3, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        Func<T1, T2, T3, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(source1, source2, source3, resultSelector);

    private static IAsyncObservable<(T1, T2, T3)> CombineLatest3TupleImpl<T1, T2, T3>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3) =>
        AsyncObservable.CombineLatest(source1, source2, source3);

    private static IAsyncObservable<TResult> CombineLatest4Impl<T1, T2, T3, T4, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        Func<T1, T2, T3, T4, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(source1, source2, source3, source4, resultSelector);

    private static IAsyncObservable<(T1, T2, T3, T4)> CombineLatest4TupleImpl<T1, T2, T3, T4>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4) =>
        AsyncObservable.CombineLatest(source1, source2, source3, source4);

    private static IAsyncObservable<TResult> CombineLatest5Impl<T1, T2, T3, T4, T5, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        Func<T1, T2, T3, T4, T5, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(source1, source2, source3, source4, source5, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5
        )> CombineLatest5TupleImpl<
        T1, T2, T3, T4, T5
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5) =>
        AsyncObservable.CombineLatest(source1, source2, source3, source4, source5);

    private static IAsyncObservable<TResult> CombineLatest6Impl<T1, T2, T3, T4, T5, T6, TResult>(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6
        )> CombineLatest6TupleImpl<
        T1, T2, T3, T4, T5, T6
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6) =>
        AsyncObservable.CombineLatest(source1, source2, source3, source4, source5, source6);

    private static IAsyncObservable<TResult> CombineLatest7Impl<
    T1, T2, T3, T4, T5, T6, T7, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7
        )> CombineLatest7TupleImpl<
        T1, T2, T3, T4, T5, T6, T7
        >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7) =>
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7);

    private static IAsyncObservable<TResult> CombineLatest8Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, TResult
    >(
        IAsyncObservable<T1> source1,
        IAsyncObservable<T2> source2,
        IAsyncObservable<T3> source3,
        IAsyncObservable<T4> source4,
        IAsyncObservable<T5> source5,
        IAsyncObservable<T6> source6,
        IAsyncObservable<T7> source7,
        IAsyncObservable<T8> source8,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8
        )> CombineLatest8TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8);

    private static IAsyncObservable<TResult> CombineLatest9Impl<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult
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
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector) =>
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9
        )> CombineLatest9TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9);

    private static IAsyncObservable<TResult> CombineLatest10Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        )> CombineLatest10TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10);

    private static IAsyncObservable<TResult> CombineLatest11Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11
        )> CombineLatest11TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11);

    private static IAsyncObservable<TResult> CombineLatest12Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12
        )> CombineLatest12TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12);

    private static IAsyncObservable<TResult> CombineLatest13Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13
        )> CombineLatest13TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13);

    private static IAsyncObservable<TResult> CombineLatest14Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14
        )> CombineLatest14TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14);

    private static IAsyncObservable<TResult> CombineLatest15Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15
        )> CombineLatest15TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15);

    private static IAsyncObservable<TResult> CombineLatest16Impl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16, resultSelector);

    private static IAsyncObservable<(
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16
        )> CombineLatest16TupleImpl<
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
        AsyncObservable.CombineLatest(
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11, source12, source13, source14, source15, source16);
}
