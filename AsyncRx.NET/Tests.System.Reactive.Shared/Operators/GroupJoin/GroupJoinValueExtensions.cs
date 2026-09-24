// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static class GroupJoinValueExtensions
{
    public static Seq<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        this Seq<TLeft> left,
        Seq<TRight> right,
        Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
        Func<TRight, Seq<TRightDuration>> rightDurationSelector,
        Func<TLeft, Seq<TRight>, TResult> resultSelector,
        [CallerArgumentExpression(nameof(leftDurationSelector))] string leftText = "",
        [CallerArgumentExpression(nameof(rightDurationSelector))] string rightText = "",
        [CallerArgumentExpression(nameof(resultSelector))] string resultText = "") =>
        new GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector, $"{leftText}, {rightText}, {resultText}");
}
