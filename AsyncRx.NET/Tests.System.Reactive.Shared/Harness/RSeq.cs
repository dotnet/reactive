// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A realized sequence.
/// </summary>
/// <typeparam name="T">
/// The element type, or <see cref="RSeq{T}"/> to represent a nested stream.
/// </typeparam>
/// <remarks>
/// <para>
/// This is the type returned by <see cref="ISeqVisitor"/> members, and it enables that interface
/// to express the shape that realised sequences should have, without imposing a specific
/// representation.
/// </para>
/// </remarks>
public abstract class RSeq<T>
{
    /// <summary>
    /// Gets the underlying realized sequence that this represents.
    /// </summary>
    /// <typeparam name="TS">
    /// The type that the sequence is expected to have.
    /// </typeparam>
    /// <returns>The underlying realized sequence.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the underlying sequence type is not <typeparamref name="TS"/>.
    /// </exception>
    public abstract TS Get<TS>();
}
