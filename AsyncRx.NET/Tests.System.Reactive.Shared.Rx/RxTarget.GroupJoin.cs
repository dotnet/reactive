// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<TResult> GroupJoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        IObservable<TLeft> left,
        IObservable<TRight> right,
        Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
        Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
        Func<TLeft, IObservable<TRight>, TResult> resultSelector) =>
        left.GroupJoin(right, leftDurationSelector, rightDurationSelector, resultSelector);
}
