// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class ConcatSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    public Seq<T> First => first;

    public Seq<T> Second => second;

    public override object Accept(ISeqVisitor visitor) => visitor.Concat(this);

    public override string ToString() => $"{first}.Concat({second})";
}
