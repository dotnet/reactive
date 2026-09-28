// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The flattening idiom's projection over a nested sequence.</summary>
/// <remarks>
/// The callback receives each inner sequence as a <see cref="NativeSeq{T}"/> and returns a
/// description, which the target materializes in place over the real inner sequence. Built by
/// <see cref="SeqExtensions.Select{TIn, TOut}(Nested{TIn}, Func{Seq{TIn}, int, Seq{TOut}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectNested{TIn, TOut}(SelectNestedSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectNestedSeq<TIn, TOut>(Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, string text) : Nested<TOut>
{
    /// <summary>A sequence of elements to invoke a transform function on.</summary>
    public Nested<TIn> Source => source;

    /// <summary>A transform function to apply to each source element.</summary>
    /// <remarks>
    /// The second parameter of the function represents the index of the source element.
    /// </remarks>
    public Func<Seq<TIn>, int, Seq<TOut>> Selector => selector;

    /// <inheritdoc/>
    public override object Accept(ISeqVisitor visitor) => visitor.SelectNested(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Select({text})";
}
