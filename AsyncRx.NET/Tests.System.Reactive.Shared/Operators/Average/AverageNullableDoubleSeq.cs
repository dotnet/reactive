// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Average()</c> over a sequence of nullable <c>double</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Average(Seq{double?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.AverageNullableDouble(AverageNullableDoubleSeq)"/>.
/// </remarks>
public sealed class AverageNullableDoubleSeq(Seq<double?> source) : Seq<double?>
{
    /// <summary>A sequence of nullable <c>double</c> values to calculate the average of.</summary>
    public Seq<double?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageNullableDouble(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average()";
}
