// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Scan(seed, accumulator)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ScanExtensions.Scan{T, TAccumulate}(Seq{T}, TAccumulate, Func{TAccumulate, T, TAccumulate}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ScanSeed{T, TAccumulate}(ScanSeedSeq{T, TAccumulate})"/>.
/// </remarks>
public sealed class ScanSeedSeq<T, TAccumulate>(
    Seq<T> source,
    TAccumulate seed,
    Func<TAccumulate, T, TAccumulate> accumulator,
    string text)
    : Seq<TAccumulate>
{
    /// <summary>An observable sequence to accumulate over.</summary>
    public Seq<T> Source => source;

    /// <summary>The initial accumulator value.</summary>
    public TAccumulate Seed => seed;

    /// <summary>An accumulator function to be invoked on each element.</summary>
    public Func<TAccumulate, T, TAccumulate> Accumulator => accumulator;

    /// <inheritdoc/>
    protected override Realized<Seq<TAccumulate>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ScanSeed(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Scan({seed}, {text})";
}
