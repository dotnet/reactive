// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A node of a query description.
/// </summary>
/// <remarks>
/// What <see cref="Seq{T}"/> (a sequence) and <see cref="Nested{T}"/> (a sequence of sequences)
/// have in common, which is only that a target can materialize it.
/// </remarks>
public interface ISeq
{
    /// <summary>Materializes this description on <paramref name="visitor"/>, a target.</summary>
    /// <param name="visitor">The target materializing the query.</param>
    /// <returns>The target's own observable for this node.</returns>
    /// <remarks>
    /// Dispatches to the visitor member for this node type and returns the target's own
    /// observable as <see cref="object"/>. Only the target casts it.
    /// </remarks>
    object Accept(ISeqVisitor visitor);
}
