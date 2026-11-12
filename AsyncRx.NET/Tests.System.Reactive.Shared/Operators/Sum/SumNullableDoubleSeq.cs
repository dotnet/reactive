// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>double</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Sum(Seq{double?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SumNullableDouble(SumNullableDoubleSeq)"/>.
/// </remarks>
public sealed class SumNullableDoubleSeq(Seq<double?> source) : Seq<double?>
{
    /// <summary>A sequence of nullable <c>double</c> values to calculate the sum of.</summary>
    public Seq<double?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SumNullableDouble(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sum()";
}
