// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Cast&lt;TResult&gt;()</c>.</summary>
/// <remarks>
/// Built by <see cref="CastExtensions.Cast{TResult}(Seq{object})"/>; materialized by each
/// target through <see cref="ISeqVisitor.Cast{TResult}(CastSeq{TResult})"/>. Over a sequence
/// of objects, as Rx.NET's is, so that only the result type is named.
/// </remarks>
public sealed class CastSeq<TResult>(Seq<object> source) : Seq<TResult>
{
    /// <summary>The observable sequence that contains the elements to be converted.</summary>
    public Seq<object> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Cast(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Cast<{typeof(TResult).Name}>()";
}
