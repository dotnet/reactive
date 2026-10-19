// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="JoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>
    /// as the target's own
    /// <c>left.Join(right, leftDurationSelector, rightDurationSelector, resultSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="JoinExtensions.Join{TLeft, TRight, TLeftDuration, TRightDuration, TResult}(Seq{TLeft}, Seq{TRight}, Func{TLeft, Seq{TLeftDuration}}, Func{TRight, Seq{TRightDuration}}, Func{TLeft, TRight, TResult}, string, string, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
        JoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq);
}
