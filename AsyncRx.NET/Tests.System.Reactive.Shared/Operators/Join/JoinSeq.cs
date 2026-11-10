// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>left.Join(right, leftDurationSelector, rightDurationSelector, resultSelector)</c>.
/// </summary>
/// <typeparam name="TLeft">The type of the elements in the left source sequence.</typeparam>
/// <typeparam name="TRight">The type of the elements in the right source sequence.</typeparam>
/// <typeparam name="TLeftDuration">
/// The type of the elements in the duration sequence denoting the computed duration of each
/// element in the left source sequence.
/// </typeparam>
/// <typeparam name="TRightDuration">
/// The type of the elements in the duration sequence denoting the computed duration of each
/// element in the right source sequence.
/// </typeparam>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by
/// <see cref="JoinExtensions.Join{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(Seq{TLeft}, Seq{TRight}, Func{TLeft, Seq{TLeftDuration}}, Func{TRight, Seq{TRightDuration}}, Func{TLeft, TRight, TResult}, string, string, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Join{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(JoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult})"/>,
/// with the duration selectors adapted to return the target's own sequences.
/// </remarks>
public sealed class JoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
    Seq<TLeft> left,
    Seq<TRight> right,
    Func<TLeft, Seq<TLeftDuration>> leftDurationSelector,
    Func<TRight, Seq<TRightDuration>> rightDurationSelector,
    Func<TLeft, TRight, TResult> resultSelector,
    string text)
    : Seq<TResult>
{
    /// <summary>The left observable sequence to join elements for.</summary>
    public Seq<TLeft> Left => left;

    /// <summary>The right observable sequence to join elements for.</summary>
    public Seq<TRight> Right => right;

    /// <summary>
    /// A function to select the duration of each element of the left observable sequence,
    /// used to determine overlap.
    /// </summary>
    public Func<TLeft, Seq<TLeftDuration>> LeftDurationSelector => leftDurationSelector;

    /// <summary>
    /// A function to select the duration of each element of the right observable sequence,
    /// used to determine overlap.
    /// </summary>
    public Func<TRight, Seq<TRightDuration>> RightDurationSelector => rightDurationSelector;

    /// <summary>A function invoked to compute a result element for any two overlapping elements.</summary>
    public Func<TLeft, TRight, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Join(this);

    /// <inheritdoc/>
    public override string ToString() => $"{left}.Join({right}, {text})";
}
