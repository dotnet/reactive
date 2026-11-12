// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sum()</c> over a sequence of nullable <c>decimal</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Sum(Seq{decimal?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SumNullableDecimal(SumNullableDecimalSeq)"/>.
/// </remarks>
public sealed class SumNullableDecimalSeq(Seq<decimal?> source) : Seq<decimal?>
{
    /// <summary>A sequence of nullable <c>decimal</c> values to calculate the sum of.</summary>
    public Seq<decimal?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<decimal?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SumNullableDecimal(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sum()";
}
