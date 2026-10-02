// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Merge(maxConcurrent)</c> over a nested sequence.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(Seq{Seq{T}}, int)"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeMaxConcurrent{T}(MergeMaxConcurrentSeq{T})"/>.
/// </remarks>
public sealed class MergeMaxConcurrentSeq<T>(Seq<Seq<T>> sources, int maxConcurrent) : Seq<T>
{
    /// <summary>Observable sequence of inner observable sequences.</summary>
    public Seq<Seq<T>> Sources => sources;

    /// <summary>
    /// Maximum number of inner observable sequences being subscribed to concurrently.
    /// </summary>
    public int MaxConcurrent => maxConcurrent;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.MergeMaxConcurrent(this);

    /// <inheritdoc/>
    public override string ToString() => $"{sources}.Merge({maxConcurrent})";
}
