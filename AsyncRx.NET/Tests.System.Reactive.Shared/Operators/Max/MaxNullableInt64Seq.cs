// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max()</c> over a sequence of nullable <c>long</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Max(Seq{long?})"/>;
/// materialized by each target through <see cref="ISeqVisitor.MaxNullableInt64(MaxNullableInt64Seq)"/>.
/// </remarks>
public sealed class MaxNullableInt64Seq(Seq<long?> source) : Seq<long?>
{
    /// <summary>
    /// A sequence of nullable <c>long</c> values to determine the maximum value of.
    /// </summary>
    public Seq<long?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<long?>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxNullableInt64(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max()";
}
