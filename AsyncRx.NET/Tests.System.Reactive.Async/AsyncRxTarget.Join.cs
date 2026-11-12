// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<TResult> JoinImpl<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        IAsyncObservable<TLeft> left,
        IAsyncObservable<TRight> right,
        Func<TLeft, IAsyncObservable<TLeftDuration>> leftDurationSelector,
        Func<TRight, IAsyncObservable<TRightDuration>> rightDurationSelector,
        Func<TLeft, TRight, TResult> resultSelector) =>
        left.Join(right, leftDurationSelector, rightDurationSelector, resultSelector);
}
