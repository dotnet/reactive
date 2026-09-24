// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class RangeSeq(int start, int count) : Seq<int>
{
    public int Start => start;

    public int Count => count;

    public override object Accept(ISeqVisitor visitor) => visitor.Range(this);

    public override string ToString() => $"Observable.Range({start}, {count})";
}
