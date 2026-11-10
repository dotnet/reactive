// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Average()</c> over a sequence of <c>int</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Average(Seq{int})"/>;
/// materialized by each target through <see cref="ISeqVisitor.AverageInt32(AverageInt32Seq)"/>.
/// </remarks>
public sealed class AverageInt32Seq(Seq<int> source) : Seq<double>
{
    /// <summary>A sequence of <c>int</c> values to calculate the average of.</summary>
    public Seq<int> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageInt32(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average()";
}
