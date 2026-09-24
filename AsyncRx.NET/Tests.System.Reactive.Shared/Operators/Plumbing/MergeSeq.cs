// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class MergeSeq<T>(Nested<T> sources) : Seq<T>
{
    public Nested<T> Sources => sources;

    public override object Accept(ISeqVisitor visitor) => visitor.Merge(this);

    public override string ToString() => $"{sources}.Merge()";
}
