// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sample(sampler)</c>.</summary>
/// <remarks>
/// Built by <see cref="SampleExtensions.Sample{T, TSample}(Seq{T}, Seq{TSample})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SampleSampler{T, TSample}(SampleSamplerSeq{T, TSample})"/>.
/// </remarks>
public sealed class SampleSamplerSeq<T, TSample>(Seq<T> source, Seq<TSample> sampler) : Seq<T>
{
    /// <summary>Source sequence to sample.</summary>
    public Seq<T> Source => source;

    /// <summary>Sampling tick sequence.</summary>
    public Seq<TSample> Sampler => sampler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SampleSampler(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sample({sampler})";
}
