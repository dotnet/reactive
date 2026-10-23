// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="WhileSeq{T}"/> as the target's own <c>While(condition, source)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.While{T}(Func{bool}, Seq{T}, string)"/>.</remarks>
    Realized<Seq<T>> While<T>(WhileSeq<T> seq);
}
