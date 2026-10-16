// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FinallySeq{T}"/> as the target's own
    /// <c>Finally(finallyAction)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="FinallyExtensions.Finally{T}(Seq{T}, Action, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Finally<T>(FinallySeq<T> seq);
}
