// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over a sequence of nullable <c>long</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min(Seq{long?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MinNullableInt64(MinNullableInt64Seq)"/>.
/// </remarks>
public sealed class MinNullableInt64Seq(Seq<long?> source) : Seq<long?>
{
    /// <summary>
    /// A sequence of nullable <c>long</c> values to determine the minimum value of.
    /// </summary>
    public Seq<long?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<long?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MinNullableInt64(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}
