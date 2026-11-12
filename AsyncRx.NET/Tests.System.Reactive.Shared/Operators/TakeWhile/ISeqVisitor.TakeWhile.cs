// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TakeWhileSeq{T}"/> as the target's own <c>TakeWhile(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeWhileExtensions.TakeWhile{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeWhile<T>(TakeWhileSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeWhileIndexedSeq{T}"/> as the target's own indexed
    /// <c>TakeWhile((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeWhileExtensions.TakeWhile{T}(Seq{T}, Func{T, int, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeWhileIndexed<T>(TakeWhileIndexedSeq<T> seq);
}
