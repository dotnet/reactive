// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>float</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Max(Seq{float?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MaxNullableSingle(MaxNullableSingleSeq)"/>.
/// </remarks>
public sealed class MaxNullableSingleSeq(Seq<float?> source) : Seq<float?>
{
    /// <summary>
    /// A sequence of nullable <c>float</c> values to determine the maximum value of.
    /// </summary>
    public Seq<float?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<float?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxNullableSingle(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max()";
}
