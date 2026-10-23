// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<SeqPattern2<T1, T2>> ISeqVisitor.And2<T1, T2>(SeqPattern2<T1, T2> pattern) =>
        _bridge.Run<SeqPattern2<T1, T2>>(And2Impl<T1, T2>, pattern.Left, pattern.Right);

    Realized<SeqPattern3<T1, T2, T3>> ISeqVisitor.And3<T1, T2, T3>(SeqPattern3<T1, T2,
            T3> pattern) =>
        _bridge.Run<SeqPattern3<T1, T2, T3>>(And3Impl<T1, T2, T3>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern4<T1, T2, T3, T4>> ISeqVisitor.And4<T1, T2, T3, T4>(SeqPattern4<T1, T2, T3,
            T4> pattern) =>
        _bridge.Run<SeqPattern4<T1, T2, T3, T4>>(And4Impl<T1, T2, T3, T4>, pattern.Pattern,
                pattern.Other);

    Realized<SeqPattern5<T1, T2, T3, T4, T5>> ISeqVisitor.And5<T1, T2, T3, T4, T5>(SeqPattern5<T1,
            T2, T3, T4, T5> pattern) =>
        _bridge.Run<SeqPattern5<T1, T2, T3, T4, T5>>(And5Impl<T1, T2, T3, T4, T5>, pattern.Pattern,
                pattern.Other);

    Realized<SeqPattern6<T1, T2, T3, T4, T5, T6>> ISeqVisitor.And6<T1, T2, T3, T4, T5, T6>(
            SeqPattern6<T1, T2, T3, T4, T5, T6> pattern) =>
        _bridge.Run<SeqPattern6<T1, T2, T3, T4, T5, T6>>(And6Impl<T1, T2, T3, T4, T5, T6>,
                pattern.Pattern, pattern.Other);

    Realized<SeqPattern7<T1, T2, T3, T4, T5, T6, T7>> ISeqVisitor.And7<T1, T2, T3, T4, T5, T6, T7>(
            SeqPattern7<T1, T2, T3, T4, T5, T6, T7> pattern) =>
        _bridge.Run<SeqPattern7<T1, T2, T3, T4, T5, T6, T7>>(And7Impl<T1, T2, T3, T4, T5, T6, T7>,
                pattern.Pattern, pattern.Other);

    Realized<SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>> ISeqVisitor.And8<T1, T2, T3, T4, T5, T6,
            T7, T8>(SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8> pattern) =>
        _bridge.Run<SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>>(And8Impl<T1, T2, T3, T4, T5, T6,
                T7, T8>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9>> ISeqVisitor.And9<T1, T2, T3, T4, T5,
            T6, T7, T8, T9>(SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9> pattern) =>
        _bridge.Run<SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(And9Impl<T1, T2, T3, T4, T5,
                T6, T7, T8, T9>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> ISeqVisitor.And10<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10>(SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10> pattern) =>
        _bridge.Run<SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(And10Impl<T1, T2, T3, T4,
                T5, T6, T7, T8, T9, T10>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> ISeqVisitor.And11<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11>(SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11> pattern) =>
        _bridge.Run<SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(And11Impl<T1, T2,
                T3, T4, T5, T6, T7, T8, T9, T10, T11>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> ISeqVisitor.And12<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(SeqPattern12<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12> pattern) =>
        _bridge.Run<SeqPattern12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(And12Impl<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, pattern.Pattern, pattern.Other);

    Realized<SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13>> ISeqVisitor.And13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> pattern) =>
        _bridge.Run<SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(And13Impl<
                T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, pattern.Pattern,
                pattern.Other);

    Realized<SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14>> ISeqVisitor.And14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> pattern) =>
        _bridge.Run<SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
                And14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>,
                pattern.Pattern, pattern.Other);

    Realized<SeqPattern15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15>> ISeqVisitor.And15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15>(SeqPattern15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15> pattern) =>
        _bridge.Run<SeqPattern15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>(
                And15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>,
                pattern.Pattern, pattern.Other);

    Realized<SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16>> ISeqVisitor.And16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15, T16>(SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> pattern) =>
        _bridge.Run<SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
                T16>>(And16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
                T16>, pattern.Pattern, pattern.Other);

    private static AsyncPattern<T1, T2> And2Impl<T1, T2>(IAsyncObservable<T1> left,
            IAsyncObservable<T2> right) =>
        left.And(right);

    private static AsyncPattern<T1, T2, T3> And3Impl<T1, T2, T3>(AsyncPattern<T1, T2> pattern,
            IAsyncObservable<T3> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4> And4Impl<T1, T2, T3, T4>(AsyncPattern<T1, T2,
            T3> pattern, IAsyncObservable<T4> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5> And5Impl<T1, T2, T3, T4, T5>(AsyncPattern<T1,
            T2, T3, T4> pattern, IAsyncObservable<T5> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6> And6Impl<T1, T2, T3, T4, T5, T6>(
            AsyncPattern<T1, T2, T3, T4, T5> pattern, IAsyncObservable<T6> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7> And7Impl<T1, T2, T3, T4, T5, T6, T7>(
            AsyncPattern<T1, T2, T3, T4, T5, T6> pattern, IAsyncObservable<T7> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8> And8Impl<T1, T2, T3, T4, T5, T6, T7,
            T8>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7> pattern, IAsyncObservable<T8> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9> And9Impl<T1, T2, T3, T4, T5, T6,
            T7, T8, T9>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8> pattern, IAsyncObservable<
            T9> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> And10Impl<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9> pattern,
            IAsyncObservable<T10> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> And11Impl<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10> pattern, IAsyncObservable<T11> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> And12Impl<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11> pattern, IAsyncObservable<T12> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> And13Impl<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(AsyncPattern<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, T11, T12> pattern, IAsyncObservable<T13> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14> And14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> pattern,
            IAsyncObservable<T14> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15> And15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> pattern,
            IAsyncObservable<T15> other) =>
        pattern.And(other);

    private static AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> And16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> pattern,
            IAsyncObservable<T16> other) =>
        pattern.And(other);
}
