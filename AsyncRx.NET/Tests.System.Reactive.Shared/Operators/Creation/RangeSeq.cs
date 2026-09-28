// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Range(start, count)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Range(int, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Range(RangeSeq)"/>.
/// </remarks>
public sealed class RangeSeq(int start, int count) : Seq<int>
{
    /// <summary>The value of the first integer in the sequence.</summary>
    public int Start => start;

    /// <summary>The number of sequential integers to generate.</summary>
    public int Count => count;

    /// <inheritdoc/>
    public override object Accept(ISeqVisitor visitor) => visitor.Range(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Range({start}, {count})";
}
