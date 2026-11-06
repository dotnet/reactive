// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over a sequence of <c>long</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min(Seq{long})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MinInt64(MinInt64Seq)"/>.
/// </remarks>
public sealed class MinInt64Seq(Seq<long> source) : Seq<long>
{
    /// <summary>A sequence of <c>long</c> values to determine the minimum value of.</summary>
    public Seq<long> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinInt64(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}
