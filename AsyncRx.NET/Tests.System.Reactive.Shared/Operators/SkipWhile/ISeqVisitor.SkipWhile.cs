// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SkipWhileSeq{T}"/> as the target's own <c>SkipWhile(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipWhileExtensions.SkipWhile{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> SkipWhile<T>(SkipWhileSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SkipWhileIndexedSeq{T}"/> as the target's own indexed
    /// <c>SkipWhile((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SkipWhileExtensions.SkipWhile{T}(Seq{T}, Func{T, int, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> SkipWhileIndexed<T>(SkipWhileIndexedSeq<T> seq);
}
