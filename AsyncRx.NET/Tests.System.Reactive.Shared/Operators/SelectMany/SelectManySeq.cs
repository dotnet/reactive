// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SelectMany(other)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Seq{TOut})"/>; materialized by each
/// target through <see cref="ISeqVisitor.SelectMany{TIn, TOut}(SelectManySeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectManySeq<TIn, TOut>(Seq<TIn> source, Seq<TOut> other) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// An observable sequence to project each element from the source sequence onto.
    /// </summary>
    public Seq<TOut> Other => other;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectMany(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({other})";
}
