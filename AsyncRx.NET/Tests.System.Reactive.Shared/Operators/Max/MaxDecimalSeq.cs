// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max()</c> over a sequence of <c>decimal</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Max(Seq{decimal})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MaxDecimal(MaxDecimalSeq)"/>.
/// </remarks>
public sealed class MaxDecimalSeq(Seq<decimal> source) : Seq<decimal>
{
    /// <summary>A sequence of <c>decimal</c> values to determine the maximum value of.</summary>
    public Seq<decimal> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<decimal>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxDecimal(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max()";
}
