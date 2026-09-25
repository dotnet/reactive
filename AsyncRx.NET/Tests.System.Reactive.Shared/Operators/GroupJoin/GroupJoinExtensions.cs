// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>GroupJoin</c> overload whose result selector returns a sequence, as an extension
/// method on <see cref="Seq{T}"/>, so that a shared scenario writes it exactly as the Rx.NET
/// test it was migrated from does. Builds a <see cref="GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>,
/// which each target materializes through <see cref="ISeqVisitor.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult})"/>.
/// </summary>
/// <remarks>
/// The result selector receives the group as a <c>Seq&lt;TRight&gt;</c> — a <see cref="NativeSeq{T}"/>
/// wrapping the target's group — and returns a description that is materialized in place, so
/// the sync suite's <c>(x, yy) =&gt; yy.Select(y =&gt; x.Value + y.Value)</c> is written unchanged
/// and becomes the target's <c>yy.Select(...)</c> with nothing in between. This overload is
/// more specific than the value-returning one in <see cref="GroupJoinValueExtensions"/>, so
/// overload resolution prefers it when the selector returns a description.
/// </remarks>
public static class GroupJoinExtensions
{
    /// <summary>Describes <c>left.GroupJoin(right, leftDurationSelector, rightDurationSelector, resultSelector)</c> with a sequence-returning result selector.</summary>
    /// <typeparam name="TLeft">The type of the elements in the left source sequence.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the right source sequence.</typeparam>
    /// <typeparam name="TLeftDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the left source sequence.</typeparam>
    /// <typeparam name="TRightDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the right source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by invoking the result selector function for source elements with overlapping duration.</typeparam>
    /// <param name="left">The left observable sequence to join elements for.</param>
    /// <param name="right">The right observable sequence to join elements for.</param>
    /// <param name="leftDurationSelector">A function to select the duration of each element of the left observable sequence, used to determine overlap.</param>
    /// <param name="rightDurationSelector">A function to select the duration of each element of the right observable sequence, used to determine overlap.</param>
    /// <param name="resultSelector">A function invoked to compute a result element for any element of the left sequence with overlapping elements from the right observable sequence.</param>
    /// <param name="leftText">Supplied by the compiler (the source text of <paramref name="leftDurationSelector"/>), for printing the query in diagnostics; do not pass it.</param>
    /// <param name="rightText">Supplied by the compiler (the source text of <paramref name="rightDurationSelector"/>), for printing the query in diagnostics; do not pass it.</param>
    /// <param name="resultText">Supplied by the compiler (the source text of <paramref name="resultSelector"/>), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>, which each target materializes through <see cref="ISeqVisitor.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult})"/>.</remarks>
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
