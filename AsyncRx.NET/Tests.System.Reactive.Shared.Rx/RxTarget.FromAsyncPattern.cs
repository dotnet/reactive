// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;

// Rx.NET marks FromAsyncPattern obsolete, pointing at FromAsync; the scenarios cover it all the
// same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc0<TResult>(FromAsyncPatternFunc0Seq<
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<IObservable<TResult>>>(FromAsyncPatternFunc0Impl<TResult>, seq.Begin,
                    seq.End)());

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction0(FromAsyncPatternAction0Seq seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<IObservable<Unit>>>(FromAsyncPatternAction0Impl, seq.Begin, seq.End)(
                    ));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc1<T1, TResult>(FromAsyncPatternFunc1Seq<
            T1, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, IObservable<TResult>>>(FromAsyncPatternFunc1Impl<T1, TResult>,
                    seq.Begin, seq.End)(seq.Arg1));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction1<T1>(FromAsyncPatternAction1Seq<
            T1> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, IObservable<Unit>>>(FromAsyncPatternAction1Impl<T1>, seq.Begin,
                    seq.End)(seq.Arg1));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc2<T1, T2, TResult>(
            FromAsyncPatternFunc2Seq<T1, T2, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, IObservable<TResult>>>(FromAsyncPatternFunc2Impl<T1, T2,
                    TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction2<T1, T2>(FromAsyncPatternAction2Seq<T1,
            T2> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, IObservable<Unit>>>(FromAsyncPatternAction2Impl<T1, T2>,
                    seq.Begin, seq.End)(seq.Arg1, seq.Arg2));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc3<T1, T2, T3, TResult>(
            FromAsyncPatternFunc3Seq<T1, T2, T3, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, IObservable<TResult>>>(FromAsyncPatternFunc3Impl<T1, T2,
                    T3, TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction3<T1, T2, T3>(FromAsyncPatternAction3Seq<
            T1, T2, T3> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, IObservable<Unit>>>(FromAsyncPatternAction3Impl<T1, T2,
                    T3>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc4<T1, T2, T3, T4, TResult>(
            FromAsyncPatternFunc4Seq<T1, T2, T3, T4, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, IObservable<TResult>>>(FromAsyncPatternFunc4Impl<T1,
                    T2, T3, T4, TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3,
                    seq.Arg4));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction4<T1, T2, T3, T4>(
            FromAsyncPatternAction4Seq<T1, T2, T3, T4> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, IObservable<Unit>>>(FromAsyncPatternAction4Impl<T1,
                    T2, T3, T4>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc5<T1, T2, T3, T4, T5, TResult>(
            FromAsyncPatternFunc5Seq<T1, T2, T3, T4, T5, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, IObservable<TResult>>>(FromAsyncPatternFunc5Impl<
                    T1, T2, T3, T4, T5, TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3,
                    seq.Arg4, seq.Arg5));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction5<T1, T2, T3, T4, T5>(
            FromAsyncPatternAction5Seq<T1, T2, T3, T4, T5> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, IObservable<Unit>>>(FromAsyncPatternAction5Impl<
                    T1, T2, T3, T4, T5>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4,
                    seq.Arg5));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc6<T1, T2, T3, T4, T5, T6, TResult>(
            FromAsyncPatternFunc6Seq<T1, T2, T3, T4, T5, T6, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>>>(
                    FromAsyncPatternFunc6Impl<T1, T2, T3, T4, T5, T6, TResult>, seq.Begin, seq.End)(
                    seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction6<T1, T2, T3, T4, T5, T6>(
            FromAsyncPatternAction6Seq<T1, T2, T3, T4, T5, T6> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>>>(
                    FromAsyncPatternAction6Impl<T1, T2, T3, T4, T5, T6>, seq.Begin, seq.End)(
                    seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc7<T1, T2, T3, T4, T5, T6, T7, TResult>(
            FromAsyncPatternFunc7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>>>(
                    FromAsyncPatternFunc7Impl<T1, T2, T3, T4, T5, T6, T7, TResult>, seq.Begin,
                    seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction7<T1, T2, T3, T4, T5, T6, T7>(
            FromAsyncPatternAction7Seq<T1, T2, T3, T4, T5, T6, T7> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>>>(
                    FromAsyncPatternAction7Impl<T1, T2, T3, T4, T5, T6, T7>, seq.Begin, seq.End)(
                    seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc8<T1, T2, T3, T4, T5, T6, T7, T8,
            TResult>(FromAsyncPatternFunc8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>>>(
                    FromAsyncPatternFunc8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, seq.Begin,
                    seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                    seq.Arg8));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction8<T1, T2, T3, T4, T5, T6, T7, T8>(
            FromAsyncPatternAction8Seq<T1, T2, T3, T4, T5, T6, T7, T8> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>>>(
                    FromAsyncPatternAction8Impl<T1, T2, T3, T4, T5, T6, T7, T8>, seq.Begin,
                    seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                    seq.Arg8));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc9<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            TResult>(FromAsyncPatternFunc9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>>>(
                    FromAsyncPatternFunc9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>,
                    seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                    seq.Arg7, seq.Arg8, seq.Arg9));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            FromAsyncPatternAction9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>>>(
                    FromAsyncPatternAction9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9>, seq.Begin,
                    seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7,
                    seq.Arg8, seq.Arg9));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc10<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, TResult>(FromAsyncPatternFunc10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>>>(
                    FromAsyncPatternFunc10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>,
                    seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                    seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction10<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(FromAsyncPatternAction10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>>>(
                    FromAsyncPatternAction10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
                    seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                    seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc11<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, TResult>(FromAsyncPatternFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>>>(
                    FromAsyncPatternFunc11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                    TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5,
                    seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction11<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(FromAsyncPatternAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>>>(
                    FromAsyncPatternAction11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>,
                    seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6,
                    seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc12<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, TResult>(FromAsyncPatternFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<
                    TResult>>>(FromAsyncPatternFunc12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                    T11, T12, TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4,
                    seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11,
                    seq.Arg12));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction12<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(FromAsyncPatternAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                    IObservable<Unit>>>(FromAsyncPatternAction12Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                    T9, T10, T11, T12>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3, seq.Arg4,
                    seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10, seq.Arg11,
                    seq.Arg12));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc13<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13, TResult>(FromAsyncPatternFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11, T12, T13, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<
                    TResult>>>(FromAsyncPatternFunc13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                    T11, T12, T13, TResult>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3,
                    seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10,
                    seq.Arg11, seq.Arg12, seq.Arg13));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction13<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13>(FromAsyncPatternAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
                    IObservable<Unit>>>(FromAsyncPatternAction13Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                    T9, T10, T11, T12, T13>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3,
                    seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10,
                    seq.Arg11, seq.Arg12, seq.Arg13));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncPatternFunc14<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13, T14, TResult>(FromAsyncPatternFunc14Seq<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, T13, T14, TResult> seq) =>
        Realized.Of<Seq<TResult>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                    IObservable<TResult>>>(FromAsyncPatternFunc14Impl<T1, T2, T3, T4, T5, T6, T7,
                    T8, T9, T10, T11, T12, T13, T14, TResult>, seq.Begin, seq.End)(seq.Arg1,
                    seq.Arg2, seq.Arg3, seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9,
                    seq.Arg10, seq.Arg11, seq.Arg12, seq.Arg13, seq.Arg14));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncPatternAction14<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13, T14>(FromAsyncPatternAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12, T13, T14> seq) =>
        Realized.Of<Seq<Unit>>(
            _bridge.Call<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
                    IObservable<Unit>>>(FromAsyncPatternAction14Impl<T1, T2, T3, T4, T5, T6, T7, T8,
                    T9, T10, T11, T12, T13, T14>, seq.Begin, seq.End)(seq.Arg1, seq.Arg2, seq.Arg3,
                    seq.Arg4, seq.Arg5, seq.Arg6, seq.Arg7, seq.Arg8, seq.Arg9, seq.Arg10,
                    seq.Arg11, seq.Arg12, seq.Arg13, seq.Arg14));

    private static Func<IObservable<TResult>> FromAsyncPatternFunc0Impl<TResult>(Func<AsyncCallback,
            object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<IObservable<Unit>> FromAsyncPatternAction0Impl(Func<AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, IObservable<TResult>> FromAsyncPatternFunc1Impl<T1, TResult>(Func<T1,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, IObservable<Unit>> FromAsyncPatternAction1Impl<T1>(Func<T1,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, IObservable<TResult>> FromAsyncPatternFunc2Impl<T1, T2, TResult>(
            Func<T1, T2, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult,
            TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, IObservable<Unit>> FromAsyncPatternAction2Impl<T1, T2>(Func<T1, T2,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, IObservable<TResult>> FromAsyncPatternFunc3Impl<T1, T2, T3,
            TResult>(Func<T1, T2, T3, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, IObservable<Unit>> FromAsyncPatternAction3Impl<T1, T2, T3>(Func<
            T1, T2, T3, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, IObservable<TResult>> FromAsyncPatternFunc4Impl<T1, T2, T3,
            T4, TResult>(Func<T1, T2, T3, T4, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, IObservable<Unit>> FromAsyncPatternAction4Impl<T1, T2, T3,
            T4>(Func<T1, T2, T3, T4, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, IObservable<TResult>> FromAsyncPatternFunc5Impl<T1, T2,
            T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, IObservable<Unit>> FromAsyncPatternAction5Impl<T1, T2,
            T3, T4, T5>(Func<T1, T2, T3, T4, T5, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> FromAsyncPatternFunc6Impl<T1,
            T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> FromAsyncPatternAction6Impl<T1,
            T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> FromAsyncPatternFunc7Impl<
            T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback,
            object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> FromAsyncPatternAction7Impl<
            T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<
            TResult>> FromAsyncPatternFunc8Impl<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1,
            T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8,
            IObservable<Unit>> FromAsyncPatternAction8Impl<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1,
            T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<
            TResult>> FromAsyncPatternFunc9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            IObservable<Unit>> FromAsyncPatternAction9Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<
            TResult>> FromAsyncPatternFunc10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            IObservable<Unit>> FromAsyncPatternAction10Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<
            TResult>> FromAsyncPatternFunc11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            IObservable<Unit>> FromAsyncPatternAction11Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<
            TResult>> FromAsyncPatternFunc12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            IObservable<Unit>> FromAsyncPatternAction12Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback,
            object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<
            TResult>> FromAsyncPatternFunc13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            IObservable<Unit>> FromAsyncPatternAction13Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<
            TResult>> FromAsyncPatternFunc14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        Observable.FromAsyncPattern(begin, end);

    private static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            IObservable<Unit>> FromAsyncPatternAction14Impl<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        Observable.FromAsyncPattern(begin, end);
}
