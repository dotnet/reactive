// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<TOut> SelectManyImpl<TIn, TOut>(
        IObservable<TIn> source,
        IObservable<TOut> other) =>
        source.SelectMany(other);

    private static IObservable<TOut> SelectManySelectorImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, IObservable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, IObservable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyCollectionIndexedImpl<TIn, TCollection, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, IObservable<TCollection>> collectionSelector,
        Func<TIn, int, TCollection, int, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    private static IObservable<TOut> SelectManyCollectionImpl<TIn, TCollection, TOut>(
        IObservable<TIn> source,
        Func<TIn, IObservable<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);
}
