// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="WithLatestFromSeq{TFirst, TSecond, TResult}"/> as the target's
    /// own <c>WithLatestFrom(second, resultSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.WithLatestFrom{TFirst, TSecond, TResult}(Seq{TFirst}, Seq{TSecond}, Func{TFirst, TSecond, TResult}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> WithLatestFrom<TFirst, TSecond, TResult>(
        WithLatestFromSeq<TFirst, TSecond, TResult> seq);
}
