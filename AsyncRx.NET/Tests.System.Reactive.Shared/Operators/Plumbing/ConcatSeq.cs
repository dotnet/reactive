// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Concat(second)</c>.</summary>
/// <remarks>Built by <see cref="SeqExtensions.Concat{T}(Seq{T}, Seq{T})"/>; materialized by each target through <see cref="ISeqVisitor.Concat{T}(ConcatSeq{T})"/>.</remarks>
public sealed class ConcatSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    public Seq<T> First => first;

    public Seq<T> Second => second;

    public override object Accept(ISeqVisitor visitor) => visitor.Concat(this);

    public override string ToString() => $"{first}.Concat({second})";
}
