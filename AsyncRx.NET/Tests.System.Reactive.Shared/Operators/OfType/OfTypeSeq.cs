// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.OfType&lt;TResult&gt;()</c>.</summary>
/// <remarks>
/// Built by <see cref="OfTypeExtensions.OfType{TResult}(Seq{object})"/>; materialized by
/// each target through <see cref="ISeqVisitor.OfType{TResult}(OfTypeSeq{TResult})"/>. Over a
/// sequence of objects, possibly null, as Rx.NET's is, so that only the result type is named.
/// </remarks>
public sealed class OfTypeSeq<TResult>(Seq<object?> source) : Seq<TResult>
{
    /// <summary>The observable sequence that contains the elements to be filtered.</summary>
    public Seq<object?> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.OfType(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.OfType<{typeof(TResult).Name}>()";
}
