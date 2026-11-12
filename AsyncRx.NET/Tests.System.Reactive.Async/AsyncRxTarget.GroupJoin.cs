// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<Seq<TResult>>> ISeqVisitor.GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        _bridge.Run<Seq<Seq<TResult>>>(
            GroupJoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, Seq<TResult>>,
            seq.Left,
            seq.Right,
            seq.LeftDurationSelector,
            seq.RightDurationSelector,
            seq.ResultSelector);

    Realized<Seq<TResult>> ISeqVisitor.GroupJoinValue<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            GroupJoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>,
            seq.Left,
            seq.Right,
            seq.LeftDurationSelector,
            seq.RightDurationSelector,
            seq.ResultSelector);

    private static IAsyncObservable<TResult> GroupJoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        IAsyncObservable<TLeft> left,
        IAsyncObservable<TRight> right,
        Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector,
        Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector,
        Func<TLeft, IAsyncObservable<TRight>, TResult> resultSelector) =>
        left.GroupJoin(right, leftDurationSelector, rightDurationSelector, resultSelector);
}
