// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Aggregate(accumulator)</c>, without a seed.</summary>
/// <remarks>
/// Built by <see cref="AggregateExtensions.Aggregate{T}(Seq{T}, Func{T, T, T}, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Aggregate{T}(AggregateSeq{T})"/>.
/// </remarks>
public sealed class AggregateSeq<T>(Seq<T> source, Func<T, T, T> accumulator, string text)
    : Seq<T>
{
    /// <summary>An observable sequence to aggregate over.</summary>
    public Seq<T> Source => source;

    /// <summary>An accumulator function to be invoked on each element.</summary>
    public Func<T, T, T> Accumulator => accumulator;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Aggregate(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Aggregate({text})";
}
