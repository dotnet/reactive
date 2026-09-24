// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

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
