// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToSeq()</c>, an enumerable as a sequence.</summary>
/// <remarks>
/// Built by <see cref="Seq.ToSeq{T}(IEnumerable{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.Enumerable{T}(EnumerableSeq{T})"/>.
/// </remarks>
public sealed class EnumerableSeq<T>(IEnumerable<T> source) : Seq<T>
{
    /// <summary>Enumerable sequence to convert to an observable sequence.</summary>
    public IEnumerable<T> Source => source;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.Enumerable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{{{string.Join(", ", source)}}}.ToSeq()";
}
