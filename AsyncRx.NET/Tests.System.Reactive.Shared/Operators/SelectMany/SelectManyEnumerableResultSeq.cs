// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SelectMany(collectionSelector, resultSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, IEnumerable{TCollection}}, Func{TIn, TCollection, TOut}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManyEnumerableResult{TIn, TCollection, TOut}(SelectManyEnumerableResultSeq{TIn, TCollection, TOut})"/>.
/// </remarks>
public sealed class SelectManyEnumerableResultSeq<TIn, TCollection, TOut>(
    Seq<TIn> source,
    Func<TIn, IEnumerable<TCollection>> collectionSelector,
    Func<TIn, TCollection, TOut> resultSelector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>A transform function to apply to each element.</summary>
    public Func<TIn, IEnumerable<TCollection>> CollectionSelector => collectionSelector;

    /// <summary>
    /// A transform function to apply to each element of the intermediate sequence.
    /// </summary>
    public Func<TIn, TCollection, TOut> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManyEnumerableResult(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
