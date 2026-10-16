// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>sources.Merge(maxConcurrent)</c> over an enumerable of sequences.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, int)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.MergeEnumerableMaxConcurrent{T}(MergeEnumerableMaxConcurrentSeq{T})"/>.
/// </remarks>
public sealed class MergeEnumerableMaxConcurrentSeq<T>(IEnumerable<Seq<T>> sources, int maxConcurrent) : Seq<T>
{
    /// <summary>Enumerable sequence of observable sequences.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <summary>Maximum number of observable sequences being subscribed to concurrently.</summary>
    public int MaxConcurrent => maxConcurrent;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeEnumerableMaxConcurrent(this);

    /// <inheritdoc/>
    public override string ToString() => $"{{{string.Join(", ", sources)}}}.Merge({maxConcurrent})";
}
