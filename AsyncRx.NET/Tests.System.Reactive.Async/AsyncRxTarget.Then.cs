// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<SeqPlan<TResult>> ISeqVisitor.Then1<T1, TResult>(SeqPlan1<T1, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then1Impl<T1, TResult>, plan.Source, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then2<T1, T2, TResult>(SeqPlan2<T1, T2, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then2Impl<T1, T2, TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then3<T1, T2, T3, TResult>(SeqPlan3<T1, T2, T3,
            TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then3Impl<T1, T2, T3, TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then4<T1, T2, T3, T4, TResult>(SeqPlan4<T1, T2, T3, T4,
            TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then4Impl<T1, T2, T3, T4, TResult>, plan.Pattern,
                plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then5<T1, T2, T3, T4, T5, TResult>(SeqPlan5<T1, T2, T3,
            T4, T5, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then5Impl<T1, T2, T3, T4, T5, TResult>, plan.Pattern,
                plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then6<T1, T2, T3, T4, T5, T6, TResult>(SeqPlan6<T1, T2,
            T3, T4, T5, T6, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then6Impl<T1, T2, T3, T4, T5, T6, TResult>, plan.Pattern,
                plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then7<T1, T2, T3, T4, T5, T6, T7, TResult>(SeqPlan7<T1,
            T2, T3, T4, T5, T6, T7, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>, plan.Pattern,
                plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(SeqPlan8<
            T1, T2, T3, T4, T5, T6, T7, T8, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>,
                plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            SeqPlan9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
                plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            SeqPlan10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>,
                plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult>(SeqPlan11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(SeqPlan12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, TResult>(SeqPlan13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult>(SeqPlan14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, TResult>(SeqPlan15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15, TResult>, plan.Pattern, plan.Selector);

    Realized<SeqPlan<TResult>> ISeqVisitor.Then16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, T16, TResult>(SeqPlan16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16, TResult> plan) =>
        _bridge.Run<SeqPlan<TResult>>(Then16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15, T16, TResult>, plan.Pattern, plan.Selector);

    private static AsyncPlan<TResult> Then1Impl<T1, TResult>(IAsyncObservable<T1> source, Func<T1,
            TResult> selector) =>
        source.Then(selector);

    private static AsyncPlan<TResult> Then2Impl<T1, T2, TResult>(AsyncPattern<T1, T2> pattern, Func<
            T1, T2, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then3Impl<T1, T2, T3, TResult>(AsyncPattern<T1, T2,
            T3> pattern, Func<T1, T2, T3, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then4Impl<T1, T2, T3, T4, TResult>(AsyncPattern<T1, T2, T3,
            T4> pattern, Func<T1, T2, T3, T4, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then5Impl<T1, T2, T3, T4, T5, TResult>(AsyncPattern<T1, T2,
            T3, T4, T5> pattern, Func<T1, T2, T3, T4, T5, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then6Impl<T1, T2, T3, T4, T5, T6, TResult>(AsyncPattern<T1,
            T2, T3, T4, T5, T6> pattern, Func<T1, T2, T3, T4, T5, T6, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>(AsyncPattern<
            T1, T2, T3, T4, T5, T6, T7> pattern, Func<T1, T2, T3, T4, T5, T6, T7,
            TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8> pattern, Func<T1, T2, T3, T4, T5, T6, T7,
            T8, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9> pattern, Func<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pattern, Func<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> pattern, Func<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> pattern, Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15, TResult> selector) =>
        pattern.Then(selector);

    private static AsyncPlan<TResult> Then16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, T16, TResult>(AsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16, TResult> selector) =>
        pattern.Then(selector);
}
