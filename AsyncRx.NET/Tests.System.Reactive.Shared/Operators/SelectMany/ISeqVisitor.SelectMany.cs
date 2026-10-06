// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SelectManySeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Seq{TOut})"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManySelectorSeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, Seq{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManySelector<TIn, TOut>(SelectManySelectorSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyIndexedSeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(selector)</c> with an element index.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, int, Seq{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyIndexed<TIn, TOut>(SelectManyIndexedSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyCollectionIndexedSeq{TIn, TCollection, TOut}"/> as the
    /// target's own <c>SelectMany(collectionSelector, resultSelector)</c> with element indexes.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, int, Seq{TCollection}}, Func{TIn, int, TCollection, int, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyCollectionIndexed<TIn, TCollection, TOut>(
        SelectManyCollectionIndexedSeq<TIn, TCollection, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyCollectionSeq{TIn, TCollection, TOut}"/> as the
    /// target's own <c>SelectMany(collectionSelector, resultSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the three-argument
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, Seq{TCollection}}, Func{TIn, TCollection, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyCollection<TIn, TCollection, TOut>(
        SelectManyCollectionSeq<TIn, TCollection, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManySelectorsSeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(onNext, onError, onCompleted)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, Seq{TOut}}, Func{Exception, Seq{TOut}}, Func{Seq{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManySelectors<TIn, TOut>(SelectManySelectorsSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManySelectorsIndexedSeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(onNext, onError, onCompleted)</c> with an element index.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, int, Seq{TOut}}, Func{Exception, Seq{TOut}}, Func{Seq{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManySelectorsIndexed<TIn, TOut>(
        SelectManySelectorsIndexedSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyEnumerableSeq{TIn, TOut}"/> as the target's own
    /// <c>SelectMany(selector)</c> over an enumerable selector.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, IEnumerable{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyEnumerable<TIn, TOut>(SelectManyEnumerableSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyEnumerableIndexedSeq{TIn, TOut}"/> as the target's
    /// own <c>SelectMany(selector)</c> over an enumerable selector with an element index.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TOut}(Seq{TIn}, Func{TIn, int, IEnumerable{TOut}}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyEnumerableIndexed<TIn, TOut>(
        SelectManyEnumerableIndexedSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyEnumerableResultSeq{TIn, TCollection, TOut}"/> as the
    /// target's own <c>SelectMany(collectionSelector, resultSelector)</c> over an enumerable
    /// collection selector.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, IEnumerable{TCollection}}, Func{TIn, TCollection, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyEnumerableResult<TIn, TCollection, TOut>(
        SelectManyEnumerableResultSeq<TIn, TCollection, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectManyEnumerableResultIndexedSeq{TIn, TCollection, TOut}"/>
    /// as the target's own <c>SelectMany(collectionSelector, resultSelector)</c> over an
    /// enumerable collection selector with element indexes.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectManyExtensions.SelectMany{TIn, TCollection, TOut}(Seq{TIn}, Func{TIn, int, IEnumerable{TCollection}}, Func{TIn, int, TCollection, int, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectManyEnumerableResultIndexed<TIn, TCollection, TOut>(
        SelectManyEnumerableResultIndexedSeq<TIn, TCollection, TOut> seq);
}
