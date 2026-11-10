// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over a sequence of <c>double</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min(Seq{double})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MinDouble(MinDoubleSeq)"/>.
/// </remarks>
public sealed class MinDoubleSeq(Seq<double> source) : Seq<double>
{
    /// <summary>A sequence of <c>double</c> values to determine the minimum value of.</summary>
    public Seq<double> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<double>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinDouble(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}
