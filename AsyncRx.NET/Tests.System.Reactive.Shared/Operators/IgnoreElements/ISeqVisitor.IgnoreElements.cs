// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="IgnoreElementsSeq{T}"/> as the target's own
    /// <c>IgnoreElements()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="IgnoreElementsExtensions.IgnoreElements{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> IgnoreElements<T>(IgnoreElementsSeq<T> seq);
}
