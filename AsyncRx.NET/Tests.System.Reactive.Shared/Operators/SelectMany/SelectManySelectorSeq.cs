// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SelectMany(selector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, Seq{TOut}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManySelector{TIn, TOut}(SelectManySelectorSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectManySelectorSeq<TIn, TOut>(
    Seq<TIn> source,
    Func<TIn, Seq<TOut>> selector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TIn, Seq<TOut>> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManySelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
