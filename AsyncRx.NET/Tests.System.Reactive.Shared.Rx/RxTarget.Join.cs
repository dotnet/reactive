// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        JoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            JoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>,
            seq.Left,
            seq.Right,
            seq.LeftDurationSelector,
            seq.RightDurationSelector,
            seq.ResultSelector);

    private static IObservable<TResult> JoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        IObservable<TLeft> left,
        IObservable<TRight> right,
        Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
        Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
        Func<TLeft, TRight, TResult> resultSelector) =>
        left.Join(right, leftDurationSelector, rightDurationSelector, resultSelector);
}
