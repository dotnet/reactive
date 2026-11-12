// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ToArraySeq{T}"/> as the target's own <c>ToArray()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.ToArray{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T[]>> ToArray<T>(ToArraySeq<T> seq);
}
