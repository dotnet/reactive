// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TOut>> ISeqVisitor.Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectIndexedImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<T>> ISeqVisitor.Where<T>(WhereSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhereImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<TOut>> ISeqVisitor.SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManyImpl<TIn, TOut>, seq.Source, seq.Other);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyCollection<TIn, TCollection, TOut>(
        SelectManyCollectionSeq<TIn, TCollection, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyCollectionImpl<TIn, TCollection, TOut>,
            seq.Source,
            seq.CollectionSelector,
            seq.ResultSelector);

    Realized<Seq<TResult>> ISeqVisitor.Zip<TFirst, TSecond, TResult>(ZipSeq<TFirst, TSecond, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(ZipImpl<TFirst, TSecond, TResult>, seq.First, seq.Second, seq.ResultSelector);

    Realized<Seq<T>> ISeqVisitor.Concat<T>(ConcatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.ConcatNested<T>(ConcatNestedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatNestedImpl<T>, seq.Sources);

    private static IAsyncObservable<TOut> SelectImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, TOut> selector) =>
        source.Select(selector);

    private static IAsyncObservable<TOut> SelectIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, TOut> selector) =>
        source.Select(selector);

    private static IAsyncObservable<T> WhereImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.Where(predicate);

    // AsyncRx.NET has no SelectMany(other) overload; Rx.NET defines it as SelectMany(_ => other).
    private static IAsyncObservable<TOut> SelectManyImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        IAsyncObservable<TOut> other) =>
        source.SelectMany(_ => other);

    private static IAsyncObservable<TOut> SelectManyCollectionImpl<TIn, TCollection, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, IAsyncObservable<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    private static IAsyncObservable<TResult> ZipImpl<TFirst, TSecond, TResult>(
        IAsyncObservable<TFirst> first,
        IAsyncObservable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector) =>
        first.Zip(second, resultSelector);

    private static IAsyncObservable<T> ConcatImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second) =>
        first.Concat(second);

    private static IAsyncObservable<T> ConcatNestedImpl<T>(IAsyncObservable<IAsyncObservable<T>> sources) =>
        sources.Concat();
}
