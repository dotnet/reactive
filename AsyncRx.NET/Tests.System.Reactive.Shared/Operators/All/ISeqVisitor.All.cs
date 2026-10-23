// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="AllSeq{T}"/> as the target's own <c>All(predicate)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="AllExtensions.All{T}(Seq{T}, Func{T, bool}, string)"/>.
    /// </remarks>
    Realized<Seq<bool>> All<T>(AllSeq<T> seq);
}
