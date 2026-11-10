// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.SelectMany(collectionSelector, resultSelector)</c> for selectors that take
/// the indexes.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, int, IEnumerable{TCollection}}, Func{TIn, int, TCollection, int, TOut}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SelectManyEnumerableResultIndexed{TIn, TCollection, TOut}(SelectManyEnumerableResultIndexedSeq{TIn, TCollection, TOut})"/>.
/// </remarks>
public sealed class SelectManyEnumerableResultIndexedSeq<TIn, TCollection, TOut>(
    Seq<TIn> source,
    Func<TIn, int, IEnumerable<TCollection>> collectionSelector,
    Func<TIn, int, TCollection, int, TOut> resultSelector,
    string text) : Seq<TOut>
{
    /// <summary>An observable sequence of elements to project.</summary>
    public Seq<TIn> Source => source;

    /// <summary>
    /// A transform function to apply to each element; the second parameter of the function
    /// represents the index of the source element.
    /// </summary>
    public Func<TIn, int, IEnumerable<TCollection>> CollectionSelector => collectionSelector;

    /// <summary>
    /// A transform function to apply to each element of the intermediate sequence; the second
    /// parameter of the function represents the index of the source element and the fourth
    /// parameter represents the index of the intermediate element.
    /// </summary>
    public Func<TIn, int, TCollection, int, TOut> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TOut>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SelectManyEnumerableResultIndexed(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SelectMany({text})";
}
