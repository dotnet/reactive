// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="ElementAtSeq{T}"/> as the target's own <c>ElementAt(index)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="ElementAtExtensions.ElementAt{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<T>> ElementAt<T>(ElementAtSeq<T> seq);
}
