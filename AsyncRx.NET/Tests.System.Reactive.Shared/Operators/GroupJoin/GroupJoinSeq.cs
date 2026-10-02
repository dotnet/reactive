// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes
/// <c>left.GroupJoin(right, leftDurationSelector, rightDurationSelector, resultSelector)</c>
/// with a result selector that returns a sequence.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="GroupJoinExtensions.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult})"/>.
/// </remarks>
public sealed class GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
    Seq<TLeft> left,
    Seq<TRight> right,
    Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
    Func<TRight, Seq<TRightDuration>> rightDurationSelector,
    Func<TLeft, Seq<TRight>, Seq<TResult>> resultSelector,
    string text) : Seq<Seq<TResult>>
{
    /// <summary>The left observable sequence to join elements for.</summary>
    public Seq<TLeft> Left => left;

    /// <summary>The right observable sequence to join elements for.</summary>
    public Seq<TRight> Right => right;

    /// <summary>
    /// A function to select the duration of each element of the left observable sequence, used
    /// to determine overlap.
    /// </summary>
    public Func<TLeft, Seq<TLeftDuration>> LeftDurationSelector => leftDurationSelector;

    /// <summary>
    /// A function to select the duration of each element of the right observable sequence, used
    /// to determine overlap.
    /// </summary>
    public Func<TRight, Seq<TRightDuration>> RightDurationSelector => rightDurationSelector;

    /// <summary>
    /// A function invoked to compute a result element for any element of the left sequence with
    /// overlapping elements from the right observable sequence.
    /// </summary>
    public Func<TLeft, Seq<TRight>, Seq<TResult>> ResultSelector => resultSelector;

    /// <inheritdoc/>
    public override Realized<Seq<Seq<TResult>>> Accept(ISeqVisitor visitor) =>
        visitor.GroupJoin(this);

    /// <inheritdoc/>
    public override string ToString() => $"{left}.GroupJoin({right}, {text})";
}
