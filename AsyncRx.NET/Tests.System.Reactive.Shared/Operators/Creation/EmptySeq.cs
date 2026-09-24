// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class EmptySeq<T> : Seq<T>
{
    public override object Accept(ISeqVisitor visitor) => visitor.Empty(this);

    public override string ToString() => $"Observable.Empty<{typeof(T).Name}>()";
}
