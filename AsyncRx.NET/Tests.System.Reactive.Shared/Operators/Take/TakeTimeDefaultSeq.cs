// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Take(duration)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, TimeSpan)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.TakeTimeDefault{T}(TakeTimeDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class TakeTimeDefaultSeq<T>(Seq<T> source, TimeSpan duration) : Seq<T>
{
    /// <summary>Source sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for taking elements from the start of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.TakeTimeDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Take({duration.Ticks} ticks)";
}
