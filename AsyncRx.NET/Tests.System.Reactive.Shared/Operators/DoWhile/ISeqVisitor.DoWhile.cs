// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DoWhileSeq{T}"/> as the target's own
    /// <c>DoWhile(source, condition)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.DoWhile{T}(Seq{T}, Func{bool}, string)"/>.</remarks>
    Realized<Seq<T>> DoWhile<T>(DoWhileSeq<T> seq);
}
