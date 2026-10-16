// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max()</c> over a sequence of <c>double</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Max(Seq{double})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MaxDouble(MaxDoubleSeq)"/>.
/// </remarks>
public sealed class MaxDoubleSeq(Seq<double> source) : Seq<double>
{
    /// <summary>A sequence of <c>double</c> values to determine the maximum value of.</summary>
    public Seq<double> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxDouble(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max()";
}
