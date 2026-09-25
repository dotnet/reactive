// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>left.GroupJoin(right, leftDurationSelector, rightDurationSelector, resultSelector)</c> with a result selector that returns a sequence.</summary>
/// <remarks>Built by <see cref="GroupJoinExtensions.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>; materialized by each target through <see cref="ISeqVisitor.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult})"/>.</remarks>
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
