// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Merge()</c> over an enumerable of sequences.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}})"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeEnumerable{T}(MergeEnumerableSeq{T})"/>.
/// </remarks>
public sealed class MergeEnumerableSeq<T>(IEnumerable<Seq<T>> sources) : Seq<T>
{
    /// <summary>Enumerable sequence of observable sequences.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.MergeEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{{{string.Join(", ", sources)}}}.Merge()";
}
