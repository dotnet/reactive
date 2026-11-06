// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>int</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min(Seq{int?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MinNullableInt32(MinNullableInt32Seq)"/>.
/// </remarks>
public sealed class MinNullableInt32Seq(Seq<int?> source) : Seq<int?>
{
    /// <summary>
    /// A sequence of nullable <c>int</c> values to determine the minimum value of.
    /// </summary>
    public Seq<int?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<int?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinNullableInt32(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}
