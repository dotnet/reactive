// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// GroupJoin as data. The result selector receives the group as a Seq<TRight> — a NativeSeq
// wrapping the target's group — and returns a neutral sequence that is materialized in
// place, so the sync suite's (x, yy) => yy.Select(y => x.Value + y.Value) is written
// unchanged and becomes the target's yy.Select(...) with nothing in between.

public static class GroupJoinExtensions
{
    public static Nested<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        this Seq<TLeft> left,
        Seq<TRight> right,
        Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
        Func<TRight, Seq<TRightDuration>> rightDurationSelector,
        Func<TLeft, Seq<TRight>, Seq<TResult>> resultSelector,
        [CallerArgumentExpression(nameof(leftDurationSelector))] string leftText = "",
        [CallerArgumentExpression(nameof(rightDurationSelector))] string rightText = "",
        [CallerArgumentExpression(nameof(resultSelector))] string resultText = "") =>
        new GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector, $"{leftText}, {rightText}, {resultText}");
}
