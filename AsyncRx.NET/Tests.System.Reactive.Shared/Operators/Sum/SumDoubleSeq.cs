// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sum()</c> over a sequence of <c>double</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Sum(Seq{double})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SumDouble(SumDoubleSeq)"/>.
/// </remarks>
public sealed class SumDoubleSeq(Seq<double> source) : Seq<double>
{
    /// <summary>A sequence of <c>double</c> values to calculate the sum of.</summary>
    public Seq<double> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SumDouble(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sum()";
}
