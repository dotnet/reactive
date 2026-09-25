// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="GroupJoinSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/> (built by <see cref="GroupJoinExtensions.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>) as the target's own <c>GroupJoin(...)</c>, materializing the description the result selector returns in place over the target's group.</summary>
    object GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq);

    /// <summary>Materializes a <see cref="GroupJoinValueSeq{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/> (built by <see cref="GroupJoinValueExtensions.GroupJoin{TLeft, TRight, TLeftDuration, TRightDuration, TResult}"/>) as the target's own <c>GroupJoin(...)</c> with a value-returning result selector.</summary>
    object GroupJoinValue<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq);
}
