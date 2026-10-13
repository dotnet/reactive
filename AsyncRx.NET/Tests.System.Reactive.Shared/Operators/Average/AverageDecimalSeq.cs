// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Average()</c> over a sequence of <c>decimal</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Average(Seq{decimal})"/>;
/// materialized by each target through <see cref="ISeqVisitor.AverageDecimal(AverageDecimalSeq)"/>.
/// </remarks>
public sealed class AverageDecimalSeq(Seq<decimal> source) : Seq<decimal>
{
    /// <summary>A sequence of <c>decimal</c> values to calculate the average of.</summary>
    public Seq<decimal> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<decimal>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageDecimal(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average()";
}
