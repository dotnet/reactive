// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class WindowCountSeq<T>(Seq<T> source, int count, int skip) : Nested<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public int Skip => skip;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowCount(this);

    public override string ToString() => $"{source}.Window({count}, {skip})";
}
