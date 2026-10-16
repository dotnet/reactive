// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sum()</c> over a sequence of <c>int</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Sum(Seq{int})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SumInt32(SumInt32Seq)"/>.
/// </remarks>
public sealed class SumInt32Seq(Seq<int> source) : Seq<int>
{
    /// <summary>A sequence of <c>int</c> values to calculate the sum of.</summary>
    public Seq<int> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<int>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SumInt32(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Sum()";
}
