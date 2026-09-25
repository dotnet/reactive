// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>SelectMany(other)</c> form the scenarios use; AsyncRx.NET spells it <c>SelectMany(_ =&gt; other)</c>.</summary>
/// <remarks>Built by <see cref="SeqExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Seq{TOut})"/>; materialized by each target through <see cref="ISeqVisitor.SelectMany{TIn, TOut}(SelectManySeq{TIn, TOut})"/>.</remarks>
public sealed class SelectManySeq<TIn, TOut>(Seq<TIn> source, Seq<TOut> other) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Seq<TOut> Other => other;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectMany(this);

    public override string ToString() => $"{source}.SelectMany({other})";
}
