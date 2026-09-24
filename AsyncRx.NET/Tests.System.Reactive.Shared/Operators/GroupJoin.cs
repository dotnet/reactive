// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// GroupJoin as data. The result selector receives the group as a Seq<TRight> — a NativeSeq
// wrapping the platform's group — and returns a neutral sequence that is materialized in
// place, so the sync suite's (x, yy) => yy.Select(y => x.Value + y.Value) is written
// unchanged and becomes the platform's yy.Select(...) with nothing in between.

public sealed class GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
    Seq<TLeft> left,
    Seq<TRight> right,
    Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
    Func<TRight, Seq<TRightDuration>> rightDurationSelector,
    Func<TLeft, Seq<TRight>, Seq<TResult>> resultSelector,
    string text) : Nested<TResult>
{
    public Seq<TLeft> Left => left;

    public Seq<TRight> Right => right;

    public Func<TLeft, Seq<TLeftDuration>> LeftDurationSelector => leftDurationSelector;

    public Func<TRight, Seq<TRightDuration>> RightDurationSelector => rightDurationSelector;

    public Func<TLeft, Seq<TRight>, Seq<TResult>> ResultSelector => resultSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.GroupJoin(this);

    public override string ToString() => $"{left}.GroupJoin({right}, {text})";
}

public partial interface ISeqVisitor
{
    object GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq);
}

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

/// <summary>The general form: a result selector that returns a value rather than a sequence.</summary>
public sealed class GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
    Seq<TLeft> left,
    Seq<TRight> right,
    Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
    Func<TRight, Seq<TRightDuration>> rightDurationSelector,
    Func<TLeft, Seq<TRight>, TResult> resultSelector,
    string text) : Seq<TResult>
{
    public Seq<TLeft> Left => left;

    public Seq<TRight> Right => right;

    public Func<TLeft, Seq<TLeftDuration>> LeftDurationSelector => leftDurationSelector;

    public Func<TRight, Seq<TRightDuration>> RightDurationSelector => rightDurationSelector;

    public Func<TLeft, Seq<TRight>, TResult> ResultSelector => resultSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.GroupJoinValue(this);

    public override string ToString() => $"{left}.GroupJoin({right}, {text})";
}

public partial interface ISeqVisitor
{
    object GroupJoinValue<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq);
}

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
