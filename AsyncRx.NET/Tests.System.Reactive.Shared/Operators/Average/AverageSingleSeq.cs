// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Average()</c> over a sequence of <c>float</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Average(Seq{float})"/>;
/// materialized by each target through <see cref="ISeqVisitor.AverageSingle(AverageSingleSeq)"/>.
/// </remarks>
public sealed class AverageSingleSeq(Seq<float> source) : Seq<float>
{
    /// <summary>A sequence of <c>float</c> values to calculate the average of.</summary>
    public Seq<float> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<float>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageSingle(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average()";
}
