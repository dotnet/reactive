// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Take(count)</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Take{T}(TakeSeq{T})"/>.
/// </remarks>
public sealed class TakeSeq<T>(Seq<T> source, int count) : Seq<T>
{
    /// <summary>The sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>The number of elements to return.</summary>
    public int Count => count;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.Take(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Take({count})";
}
