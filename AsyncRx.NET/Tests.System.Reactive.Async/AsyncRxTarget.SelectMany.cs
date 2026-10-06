// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TOut>> ISeqVisitor.SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManyImpl<TIn, TOut>, seq.Source, seq.Other);

    Realized<Seq<TOut>> ISeqVisitor.SelectManySelector<TIn, TOut>(
        SelectManySelectorSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManySelectorImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyIndexed<TIn, TOut>(
        SelectManyIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManyIndexedImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyCollectionIndexed<TIn, TCollection, TOut>(
        SelectManyCollectionIndexedSeq<TIn, TCollection, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyCollectionIndexedImpl<TIn, TCollection, TOut>,
            seq.Source,
            seq.CollectionSelector,
            seq.ResultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyCollection<TIn, TCollection, TOut>(
        SelectManyCollectionSeq<TIn, TCollection, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyCollectionImpl<TIn, TCollection, TOut>,
            seq.Source,
            seq.CollectionSelector,
            seq.ResultSelector);

    // AsyncRx.NET has no SelectMany(other) overload; Rx.NET defines it as SelectMany(_ => other).
    private static IAsyncObservable<TOut> SelectManyImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        IAsyncObservable<TOut> other) =>
        source.SelectMany(other);

    private static IAsyncObservable<TOut> SelectManySelectorImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IAsyncObservable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IAsyncObservable<TOut> SelectManyIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, IAsyncObservable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IAsyncObservable<TOut> SelectManyCollectionIndexedImpl<TIn, TCollection, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, IAsyncObservable<TCollection>> collectionSelector,
        Func<TIn, int, TCollection, int, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    private static IAsyncObservable<TOut> SelectManyCollectionImpl<TIn, TCollection, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IAsyncObservable<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManySelectors<TIn, TOut>(
        SelectManySelectorsSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManySelectorsImpl<TIn, TOut>,
            seq.Source,
            seq.OnNext,
            seq.OnError,
            seq.OnCompleted);

    Realized<Seq<TOut>> ISeqVisitor.SelectManySelectorsIndexed<TIn, TOut>(
        SelectManySelectorsIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManySelectorsIndexedImpl<TIn, TOut>,
            seq.Source,
            seq.OnNext,
            seq.OnError,
            seq.OnCompleted);

    private static IAsyncObservable<TOut> SelectManySelectorsImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IAsyncObservable<TOut>> onNext,
        Func<Exception, IAsyncObservable<TOut>> onError,
        Func<IAsyncObservable<TOut>> onCompleted) =>
        source.SelectMany(onNext, onError, onCompleted);

    private static IAsyncObservable<TOut> SelectManySelectorsIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, IAsyncObservable<TOut>> onNext,
        Func<Exception, IAsyncObservable<TOut>> onError,
        Func<IAsyncObservable<TOut>> onCompleted) =>
        source.SelectMany(onNext, onError, onCompleted);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyEnumerable<TIn, TOut>(
        SelectManyEnumerableSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManyEnumerableImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyEnumerableIndexed<TIn, TOut>(
        SelectManyEnumerableIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyEnumerableIndexedImpl<TIn, TOut>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyEnumerableResult<TIn, TCollection, TOut>(
        SelectManyEnumerableResultSeq<TIn, TCollection, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyEnumerableResultImpl<TIn, TCollection, TOut>,
            seq.Source,
            seq.CollectionSelector,
            seq.ResultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyEnumerableResultIndexed<TIn, TCollection, TOut>(
        SelectManyEnumerableResultIndexedSeq<TIn, TCollection, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyEnumerableResultIndexedImpl<TIn, TCollection, TOut>,
            seq.Source,
            seq.CollectionSelector,
            seq.ResultSelector);

    private static IAsyncObservable<TOut> SelectManyEnumerableImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IEnumerable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IAsyncObservable<TOut> SelectManyEnumerableIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, IEnumerable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IAsyncObservable<TOut> SelectManyEnumerableResultImpl<TIn, TCollection, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IEnumerable<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    private static IAsyncObservable<TOut> SelectManyEnumerableResultIndexedImpl<TIn, TCollection, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, IEnumerable<TCollection>> collectionSelector,
        Func<TIn, int, TCollection, int, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);
}
