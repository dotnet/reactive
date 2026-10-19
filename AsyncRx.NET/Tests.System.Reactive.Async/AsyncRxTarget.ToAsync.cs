// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc0<TResult>(ToAsyncFunc0Seq<TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<IAsyncObservable<TResult>>>(ToAsyncFunc0Impl<TResult>,
                        seq.Function)
                : _bridge.Call<Func<IAsyncObservable<TResult>>>(ToAsyncFunc0OnImpl<TResult>,
                        seq.Function, Unwrap(seq.Scheduler)))());

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction0(ToAsyncAction0Seq seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<IAsyncObservable<Unit>>>(ToAsyncAction0Impl, seq.Action)
                : _bridge.Call<Func<IAsyncObservable<Unit>>>(ToAsyncAction0OnImpl, seq.Action,
                        Unwrap(seq.Scheduler)))());

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc1<T1, TResult>(ToAsyncFunc1Seq<T1,
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, IAsyncObservable<TResult>>>(ToAsyncFunc1Impl<T1, TResult>,
                        seq.Function)
                : _bridge.Call<Func<T1, IAsyncObservable<TResult>>>(ToAsyncFunc1OnImpl<T1, TResult>,
                        seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction1<T1>(ToAsyncAction1Seq<T1> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, IAsyncObservable<Unit>>>(ToAsyncAction1Impl<T1>, seq.Action)
                : _bridge.Call<Func<T1, IAsyncObservable<Unit>>>(ToAsyncAction1OnImpl<T1>,
                        seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc2<T1, T2, TResult>(ToAsyncFunc2Seq<T1, T2,
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, IAsyncObservable<TResult>>>(ToAsyncFunc2Impl<T1, T2,
                        TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, IAsyncObservable<TResult>>>(ToAsyncFunc2OnImpl<T1, T2,
                        TResult>, seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction2<T1, T2>(ToAsyncAction2Seq<T1, T2> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, IAsyncObservable<Unit>>>(ToAsyncAction2Impl<T1, T2>,
                        seq.Action)
                : _bridge.Call<Func<T1, T2, IAsyncObservable<Unit>>>(ToAsyncAction2OnImpl<T1, T2>,
                        seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc3<T1, T2, T3, TResult>(ToAsyncFunc3Seq<T1, T2, T3,
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, IAsyncObservable<TResult>>>(ToAsyncFunc3Impl<T1, T2,
                        T3, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, IAsyncObservable<TResult>>>(ToAsyncFunc3OnImpl<T1,
                        T2, T3, TResult>, seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction3<T1, T2, T3>(ToAsyncAction3Seq<T1, T2, T3> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, IAsyncObservable<Unit>>>(ToAsyncAction3Impl<T1, T2,
                        T3>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, IAsyncObservable<Unit>>>(ToAsyncAction3OnImpl<T1,
                        T2, T3>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc4<T1, T2, T3, T4, TResult>(ToAsyncFunc4Seq<T1, T2,
            T3, T4, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, IAsyncObservable<TResult>>>(ToAsyncFunc4Impl<T1,
                        T2, T3, T4, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, IAsyncObservable<TResult>>>(ToAsyncFunc4OnImpl<
                        T1, T2, T3, T4, TResult>, seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1,
                        seq.Arg2, seq.Arg3, seq.Arg4));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction4<T1, T2, T3, T4>(ToAsyncAction4Seq<T1, T2, T3,
            T4> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, IAsyncObservable<Unit>>>(ToAsyncAction4Impl<T1,
                        T2, T3, T4>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, IAsyncObservable<Unit>>>(ToAsyncAction4OnImpl<
                        T1, T2, T3, T4>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3, seq.Arg4));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc5<T1, T2, T3, T4, T5, TResult>(ToAsyncFunc5Seq<T1,
            T2, T3, T4, T5, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>>>(
                        ToAsyncFunc5Impl<T1, T2, T3, T4, T5, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>>>(
                        ToAsyncFunc5OnImpl<T1, T2, T3, T4, T5, TResult>, seq.Function, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction5<T1, T2, T3, T4, T5>(ToAsyncAction5Seq<T1, T2, T3,
            T4, T5> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>>>(ToAsyncAction5Impl<
                        T1, T2, T3, T4, T5>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>>>(
                        ToAsyncAction5OnImpl<T1, T2, T3, T4, T5>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc6<T1, T2, T3, T4, T5, T6, TResult>(
            ToAsyncFunc6Seq<T1, T2, T3, T4, T5, T6, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>>>(
                        ToAsyncFunc6Impl<T1, T2, T3, T4, T5, T6, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>>>(
                        ToAsyncFunc6OnImpl<T1, T2, T3, T4, T5, T6, TResult>, seq.Function, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                        seq.Arg6));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction6<T1, T2, T3, T4, T5, T6>(ToAsyncAction6Seq<T1, T2,
            T3, T4, T5, T6> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>>>(
                        ToAsyncAction6Impl<T1, T2, T3, T4, T5, T6>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>>>(
                        ToAsyncAction6OnImpl<T1, T2, T3, T4, T5, T6>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                        seq.Arg6));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc7<T1, T2, T3, T4, T5, T6, T7, TResult>(
            ToAsyncFunc7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>>>(
                        ToAsyncFunc7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>>>(
                        ToAsyncFunc7OnImpl<T1, T2, T3, T4, T5, T6, T7, TResult>, seq.Function,
                        Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                        seq.Arg6, seq.Arg7));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction7<T1, T2, T3, T4, T5, T6, T7>(ToAsyncAction7Seq<T1,
            T2, T3, T4, T5, T6, T7> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>>>(
                        ToAsyncAction7Impl<T1, T2, T3, T4, T5, T6, T7>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>>>(
                        ToAsyncAction7OnImpl<T1, T2, T3, T4, T5, T6, T7>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            ToAsyncFunc8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>>>(
                        ToAsyncFunc8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>>>(
                        ToAsyncFunc8OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, seq.Function,
                        Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                        seq.Arg6, seq.Arg7, seq.Arg8));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction8<T1, T2, T3, T4, T5, T6, T7, T8>(
            ToAsyncAction8Seq<T1, T2, T3, T4, T5, T6, T7, T8> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>>>(
                        ToAsyncAction8Impl<T1, T2, T3, T4, T5, T6, T7, T8>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>>>(
                        ToAsyncAction8OnImpl<T1, T2, T3, T4, T5, T6, T7, T8>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7, seq.Arg8));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            ToAsyncFunc9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<TResult>>>(
                        ToAsyncFunc9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<TResult>>>(
                        ToAsyncFunc9OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
                        seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3,
                        seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            ToAsyncAction9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<Unit>>>(
                        ToAsyncAction9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<Unit>>>(
                        ToAsyncAction9OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>, seq.Action,
                        Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                        seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            TResult>(ToAsyncFunc10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<
                        TResult>>>(ToAsyncFunc10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                        TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<
                        TResult>>>(ToAsyncFunc10OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                        TResult>, seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9,
                        seq.Arg10));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            ToAsyncAction10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                        IAsyncObservable<Unit>>>(ToAsyncAction10Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                        T9, T10>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                        IAsyncObservable<Unit>>>(ToAsyncAction10OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9,
                        seq.Arg10));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult>(ToAsyncFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<
                        TResult>>>(ToAsyncFunc11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                        TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<
                        TResult>>>(ToAsyncFunc11OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                        TResult>, seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9,
                        seq.Arg10, seq.Arg11));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            ToAsyncAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                        IAsyncObservable<Unit>>>(ToAsyncAction11Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                        T9, T10, T11>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                        IAsyncObservable<Unit>>>(ToAsyncAction11OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2,
                        seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9,
                        seq.Arg10, seq.Arg11));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, TResult>(ToAsyncFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                        IAsyncObservable<TResult>>>(ToAsyncFunc12Impl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                        IAsyncObservable<TResult>>>(ToAsyncFunc12OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, TResult>, seq.Function, Unwrap(seq.Scheduler)))(
                        seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                        seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12>(ToAsyncAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                        IAsyncObservable<Unit>>>(ToAsyncAction12Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                        T9, T10, T11, T12>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                        IAsyncObservable<Unit>>>(ToAsyncAction12OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1,
                        seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8,
                        seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, TResult>(ToAsyncFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
                        IAsyncObservable<TResult>>>(ToAsyncFunc13Impl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
                        IAsyncObservable<TResult>>>(ToAsyncFunc13OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13, TResult>, seq.Function, Unwrap(seq.Scheduler)))(
                        seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                        seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13>(ToAsyncAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
                        IAsyncObservable<Unit>>>(ToAsyncAction13Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                        T9, T10, T11, T12, T13>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
                        IAsyncObservable<Unit>>>(ToAsyncAction13OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13>, seq.Action, Unwrap(seq.Scheduler)))(seq.Arg1,
                        seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8,
                        seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, TResult>(ToAsyncFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        IAsyncObservable<TResult>>>(ToAsyncFunc14Impl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13, T14, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        IAsyncObservable<TResult>>>(ToAsyncFunc14OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13, T14, TResult>, seq.Function, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13,
                        seq.Arg14));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14>(ToAsyncAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        IAsyncObservable<Unit>>>(ToAsyncAction14Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                        T9, T10, T11, T12, T13, T14>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        IAsyncObservable<Unit>>>(ToAsyncAction14OnImpl<T1, T2, T3, T4, T5, T6, T7,
                        T8, T9, T10, T11, T12, T13, T14>, seq.Action, Unwrap(seq.Scheduler)))(
                        seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                        seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13, seq.Arg14));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, TResult>(ToAsyncFunc15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, IAsyncObservable<TResult>>>(ToAsyncFunc15Impl<T1, T2, T3, T4, T5, T6,
                        T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, IAsyncObservable<TResult>>>(ToAsyncFunc15OnImpl<T1, T2, T3, T4, T5, T6,
                        T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, seq.Function, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13,
                        seq.Arg14, seq.Arg15));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15>(ToAsyncAction15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, IAsyncObservable<Unit>>>(ToAsyncAction15Impl<T1, T2, T3, T4, T5, T6,
                        T7, T8, T9, T10, T11, T12, T13, T14, T15>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, IAsyncObservable<Unit>>>(ToAsyncAction15OnImpl<T1, T2, T3, T4, T5, T6,
                        T7, T8, T9, T10, T11, T12, T13, T14, T15>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13,
                        seq.Arg14, seq.Arg15));

    Realized<Seq<TResult>> ISeqVisitor.ToAsyncFunc16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16, TResult>(ToAsyncFunc16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13, T14, T15, T16, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, T16, IAsyncObservable<TResult>>>(ToAsyncFunc16Impl<T1, T2, T3, T4, T5,
                        T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, seq.Function)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, T16, IAsyncObservable<TResult>>>(ToAsyncFunc16OnImpl<T1, T2, T3, T4,
                        T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>,
                        seq.Function, Unwrap(seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3,
                        seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10,
                        seq.Arg11, seq.Arg12, seq.Arg13, seq.Arg14, seq.Arg15, seq.Arg16));

    Realized<Seq<Unit>> ISeqVisitor.ToAsyncAction16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16>(ToAsyncAction16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16> seq) =>
        Realized.Of<Seq<Unit>>(
            (seq.Scheduler is null
                ? _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, T16, IAsyncObservable<Unit>>>(ToAsyncAction16Impl<T1, T2, T3, T4, T5,
                        T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, seq.Action)
                : _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                        T15, T16, IAsyncObservable<Unit>>>(ToAsyncAction16OnImpl<T1, T2, T3, T4, T5,
                        T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, seq.Action, Unwrap(
                        seq.Scheduler)))(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                        seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13,
                        seq.Arg14, seq.Arg15, seq.Arg16));

    private static Func<IAsyncObservable<TResult>> ToAsyncFunc0Impl<TResult>(Func<
            TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<IAsyncObservable<TResult>> ToAsyncFunc0OnImpl<TResult>(Func<
            TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<IAsyncObservable<Unit>> ToAsyncAction0Impl(Action action) =>
        AsyncObservable.ToAsync(action);

    private static Func<IAsyncObservable<Unit>> ToAsyncAction0OnImpl(Action action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, IAsyncObservable<TResult>> ToAsyncFunc1Impl<T1, TResult>(Func<T1,
            TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, IAsyncObservable<TResult>> ToAsyncFunc1OnImpl<T1, TResult>(Func<T1,
            TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, IAsyncObservable<Unit>> ToAsyncAction1Impl<T1>(Action<T1> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, IAsyncObservable<Unit>> ToAsyncAction1OnImpl<T1>(Action<T1> action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, IAsyncObservable<TResult>> ToAsyncFunc2Impl<T1, T2, TResult>(Func<
            T1, T2, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, IAsyncObservable<TResult>> ToAsyncFunc2OnImpl<T1, T2, TResult>(Func<
            T1, T2, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, IAsyncObservable<Unit>> ToAsyncAction2Impl<T1, T2>(Action<T1,
            T2> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, IAsyncObservable<Unit>> ToAsyncAction2OnImpl<T1, T2>(Action<T1,
            T2> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, IAsyncObservable<TResult>> ToAsyncFunc3Impl<T1, T2, T3,
            TResult>(Func<T1, T2, T3, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, IAsyncObservable<TResult>> ToAsyncFunc3OnImpl<T1, T2, T3,
            TResult>(Func<T1, T2, T3, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, IAsyncObservable<Unit>> ToAsyncAction3Impl<T1, T2, T3>(Action<
            T1, T2, T3> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, IAsyncObservable<Unit>> ToAsyncAction3OnImpl<T1, T2, T3>(Action<
            T1, T2, T3> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, IAsyncObservable<TResult>> ToAsyncFunc4Impl<T1, T2, T3, T4,
            TResult>(Func<T1, T2, T3, T4, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, IAsyncObservable<TResult>> ToAsyncFunc4OnImpl<T1, T2, T3,
            T4, TResult>(Func<T1, T2, T3, T4, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, IAsyncObservable<Unit>> ToAsyncAction4Impl<T1, T2, T3, T4>(
            Action<T1, T2, T3, T4> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, IAsyncObservable<Unit>> ToAsyncAction4OnImpl<T1, T2, T3,
            T4>(Action<T1, T2, T3, T4> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>> ToAsyncFunc5Impl<T1, T2, T3,
            T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, IAsyncObservable<TResult>> ToAsyncFunc5OnImpl<T1, T2,
            T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>> ToAsyncAction5Impl<T1, T2, T3,
            T4, T5>(Action<T1, T2, T3, T4, T5> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, IAsyncObservable<Unit>> ToAsyncAction5OnImpl<T1, T2, T3,
            T4, T5>(Action<T1, T2, T3, T4, T5> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>> ToAsyncFunc6Impl<T1, T2,
            T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<TResult>> ToAsyncFunc6OnImpl<T1,
            T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>> ToAsyncAction6Impl<T1, T2,
            T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, IAsyncObservable<Unit>> ToAsyncAction6OnImpl<T1, T2,
            T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>> ToAsyncFunc7Impl<T1,
            T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<TResult>> ToAsyncFunc7OnImpl<
            T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>> ToAsyncAction7Impl<T1,
            T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IAsyncObservable<Unit>> ToAsyncAction7OnImpl<T1,
            T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<TResult>> ToAsyncFunc8Impl<
            T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8,
            TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<
            TResult>> ToAsyncFunc8OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3,
            T4, T5, T6, T7, T8, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, IAsyncObservable<Unit>> ToAsyncAction8Impl<
            T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8,
            IAsyncObservable<Unit>> ToAsyncAction8OnImpl<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1,
            T2, T3, T4, T5, T6, T7, T8> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<
            TResult>> ToAsyncFunc9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncObservable<
            TResult>> ToAsyncFunc9OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            IAsyncObservable<Unit>> ToAsyncAction9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<
            T1, T2, T3, T4, T5, T6, T7, T8, T9> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            IAsyncObservable<Unit>> ToAsyncAction9OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<
            T1, T2, T3, T4, T5, T6, T7, T8, T9> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<
            TResult>> ToAsyncFunc10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IAsyncObservable<
            TResult>> ToAsyncFunc10OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            IAsyncObservable<Unit>> ToAsyncAction10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            IAsyncObservable<Unit>> ToAsyncAction10OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<
            TResult>> ToAsyncFunc11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IAsyncObservable<
            TResult>> ToAsyncFunc11OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            IAsyncObservable<Unit>> ToAsyncAction11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            IAsyncObservable<Unit>> ToAsyncAction11OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<
            TResult>> ToAsyncFunc12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IAsyncObservable<
            TResult>> ToAsyncFunc12OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            IAsyncObservable<Unit>> ToAsyncAction12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            IAsyncObservable<Unit>> ToAsyncAction12OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<
            TResult>> ToAsyncFunc13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IAsyncObservable<
            TResult>> ToAsyncFunc13OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            IAsyncObservable<Unit>> ToAsyncAction13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            IAsyncObservable<Unit>> ToAsyncAction13OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            IAsyncObservable<TResult>> ToAsyncFunc14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            IAsyncObservable<TResult>> ToAsyncFunc14OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            IAsyncObservable<Unit>> ToAsyncAction14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            IAsyncObservable<Unit>> ToAsyncAction14OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            IAsyncObservable<TResult>> ToAsyncFunc15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            IAsyncObservable<TResult>> ToAsyncFunc15OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            IAsyncObservable<Unit>> ToAsyncAction15Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            IAsyncObservable<Unit>> ToAsyncAction15OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            IAsyncObservable<TResult>> ToAsyncFunc16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16, TResult> function) =>
        AsyncObservable.ToAsync(function);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            IAsyncObservable<TResult>> ToAsyncFunc16OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16, TResult> function, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(function, scheduler);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            IAsyncObservable<Unit>> ToAsyncAction16Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, T16> action) =>
        AsyncObservable.ToAsync(action);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            IAsyncObservable<Unit>> ToAsyncAction16OnImpl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, T16> action, IAsyncScheduler scheduler) =>
        AsyncObservable.ToAsync(action, scheduler);
}
