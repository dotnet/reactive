// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Average()</c> over a sequence of nullable <c>long</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Average(Seq{long?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.AverageNullableInt64(AverageNullableInt64Seq)"/>.
/// </remarks>
public sealed class AverageNullableInt64Seq(Seq<long?> source) : Seq<double?>
{
    /// <summary>A sequence of nullable <c>long</c> values to calculate the average of.</summary>
    public Seq<long?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AverageNullableInt64(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Average()";
}
