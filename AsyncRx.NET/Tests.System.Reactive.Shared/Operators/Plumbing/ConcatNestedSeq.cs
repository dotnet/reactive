// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Concat()</c> over a nested sequence.</summary>
/// <remarks>
/// Built by <see cref="SeqExtensions.Concat{T}(Seq{Seq{T}})"/>; materialized by each target through
/// <see cref="ISeqVisitor.ConcatNested{T}(ConcatNestedSeq{T})"/>.
/// </remarks>
public sealed class ConcatNestedSeq<T>(Seq<Seq<T>> sources) : Seq<T>
{
    /// <summary>Observable sequence of inner observable sequences.</summary>
    public Seq<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.ConcatNested(this);

    /// <inheritdoc/>
    public override string ToString() => $"{sources}.Concat()";
}
