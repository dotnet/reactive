// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Select((x, i) =&gt; ...)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SelectExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectIndexed{TIn, TOut}(SelectIndexedSeq{TIn, TOut})"/>.
/// </remarks>
public sealed class SelectIndexedSeq<TIn, TOut>(
    Seq<TIn> source,
    Func<TIn, int, TOut> selector,
    string text) : Seq<TOut>
{
    /// <summary>A sequence of elements to invoke a transform function on.</summary>
    public Seq<TIn> Source => source;

    /// <summary>A transform function to apply to each source element.</summary>
    /// <remarks>
    /// The second parameter of the function represents the index of the source element.
    /// </remarks>
    public Func<TIn, int, TOut> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectIndexed(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Select({text})";
}
