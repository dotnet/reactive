// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sum()</c> over a sequence of <c>float</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Sum(Seq{float})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SumSingle(SumSingleSeq)"/>.
/// </remarks>
public sealed class SumSingleSeq(Seq<float> source) : Seq<float>
{
    /// <summary>A sequence of <c>float</c> values to calculate the sum of.</summary>
    public Seq<float> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<float>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SumSingle(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sum()";
}
