// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Observable.Range(start, count)</c>.</summary>
/// <remarks>Built by <see cref="Observable.Range(int, int)"/>; materialized by each target through <see cref="ISeqVisitor.Range(RangeSeq)"/>.</remarks>
public sealed class RangeSeq(int start, int count) : Seq<int>
{
    public int Start => start;

    public int Count => count;

    public override object Accept(ISeqVisitor visitor) => visitor.Range(this);

    public override string ToString() => $"Observable.Range({start}, {count})";
}
