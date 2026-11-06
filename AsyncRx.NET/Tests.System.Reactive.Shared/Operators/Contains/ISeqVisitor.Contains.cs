// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ContainsSeq{T}"/> as the target's own <c>Contains(value)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="ContainsExtensions.Contains{T}(Seq{T}, T)"/>.</remarks>
    Realized<Seq<bool>> Contains<T>(ContainsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ContainsComparerSeq{T}"/> as the target's own
    /// <c>Contains(value, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="ContainsExtensions.Contains{T}(Seq{T}, T, IEqualityComparer{T})"/>.
    /// </remarks>
    Realized<Seq<bool>> ContainsComparer<T>(ContainsComparerSeq<T> seq);
}
