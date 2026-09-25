// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The flattening idiom's projection over a nested sequence. The callback receives each inner
/// sequence as a <see cref="NativeSeq{T}"/> and returns a description; the target materializes
/// that description in place, over the real inner sequence.
/// </summary>
/// <remarks>Built by <see cref="SeqExtensions.Select{TIn, TOut}(Nested{TIn}, Func{Seq{TIn}, int, Seq{TOut}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.SelectNested{TIn, TOut}(SelectNestedSeq{TIn, TOut})"/>.</remarks>
public sealed class SelectNestedSeq<TIn, TOut>(Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, string text) : Nested<TOut>
{
    public Nested<TIn> Source => source;

    public Func<Seq<TIn>, int, Seq<TOut>> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectNested(this);

    public override string ToString() => $"{source}.Select({text})";
}
