// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Select((x, i) =&gt; ...)</c>.</summary>
/// <remarks>Built by <see cref="SeqExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>; materialized by each target through <see cref="ISeqVisitor.SelectIndexed{TIn, TOut}(SelectIndexedSeq{TIn, TOut})"/>.</remarks>
public sealed class SelectIndexedSeq<TIn, TOut>(Seq<TIn> source, Func<TIn, int, TOut> selector, string text) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Func<TIn, int, TOut> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectIndexed(this);

    public override string ToString() => $"{source}.Select({text})";
}
