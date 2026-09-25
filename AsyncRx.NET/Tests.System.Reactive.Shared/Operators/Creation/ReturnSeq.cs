// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Observable.Return(value)</c>.</summary>
/// <remarks>Built by <see cref="Observable.Return{T}(T)"/>; materialized by each target through <see cref="ISeqVisitor.Return{T}(ReturnSeq{T})"/>.</remarks>
public sealed class ReturnSeq<T>(T value) : Seq<T>
{
    public T Value => value;

    public override object Accept(ISeqVisitor visitor) => visitor.Return(this);

    public override string ToString() => $"Observable.Return({value})";
}
