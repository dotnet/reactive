// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Skip(duration)</c>, on the target's default scheduler.</summary>
/// <remarks>
/// Built by <see cref="SkipExtensions.Skip{T}(Seq{T}, TimeSpan)"/>; materialized by each target
/// through <see cref="ISeqVisitor.SkipTimeDefault{T}(SkipTimeDefaultSeq{T})"/>.
/// </remarks>
public sealed class SkipTimeDefaultSeq<T>(Seq<T> source, TimeSpan duration) : Seq<T>
{
    /// <summary>Source sequence to skip elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for skipping elements from the start of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.SkipTimeDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Skip({duration.Ticks} ticks)";
}
