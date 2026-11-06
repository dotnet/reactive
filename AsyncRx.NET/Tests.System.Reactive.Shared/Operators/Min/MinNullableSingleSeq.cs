// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>float</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min(Seq{float?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MinNullableSingle(MinNullableSingleSeq)"/>.
/// </remarks>
public sealed class MinNullableSingleSeq(Seq<float?> source) : Seq<float?>
{
    /// <summary>
    /// A sequence of nullable <c>float</c> values to determine the minimum value of.
    /// </summary>
    public Seq<float?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<float?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinNullableSingle(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}
