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

    private static IObservable<TOut> SelectManySelectorsImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, IObservable<TOut>> onNext,
        Func<Exception, IObservable<TOut>> onError,
        Func<IObservable<TOut>> onCompleted) =>
        source.SelectMany(onNext, onError, onCompleted);

    private static IObservable<TOut> SelectManySelectorsIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, IObservable<TOut>> onNext,
        Func<Exception, IObservable<TOut>> onError,
        Func<IObservable<TOut>> onCompleted) =>
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

    private static IObservable<TOut> SelectManyEnumerableImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, IEnumerable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyEnumerableIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, IEnumerable<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyEnumerableResultImpl<TIn, TCollection, TOut>(
        IObservable<TIn> source,
        Func<TIn, IEnumerable<TCollection>> collectionSelector,
        Func<TIn, TCollection, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    private static IObservable<TOut> SelectManyEnumerableResultIndexedImpl<TIn, TCollection, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, IEnumerable<TCollection>> collectionSelector,
        Func<TIn, int, TCollection, int, TOut> resultSelector) =>
        source.SelectMany(collectionSelector, resultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTask<TIn, TOut>(SelectManyTaskSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskImpl<TIn, TOut>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskCancellable<TIn, TOut>(
        SelectManyTaskCancellableSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskCancellableImpl<TIn, TOut>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskIndexed<TIn, TOut>(
        SelectManyTaskIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskIndexedImpl<TIn, TOut>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskIndexedCancellable<TIn, TOut>(
        SelectManyTaskIndexedCancellableSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskIndexedCancellableImpl<TIn, TOut>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskResult<TIn, TTask, TOut>(
        SelectManyTaskResultSeq<TIn, TTask, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskResultImpl<TIn, TTask, TOut>,
            seq.Source,
            seq.TaskSelector,
            seq.ResultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskResultCancellable<TIn, TTask, TOut>(
        SelectManyTaskResultCancellableSeq<TIn, TTask, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskResultCancellableImpl<TIn, TTask, TOut>,
            seq.Source,
            seq.TaskSelector,
            seq.ResultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskResultIndexed<TIn, TTask, TOut>(
        SelectManyTaskResultIndexedSeq<TIn, TTask, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskResultIndexedImpl<TIn, TTask, TOut>,
            seq.Source,
            seq.TaskSelector,
            seq.ResultSelector);

    Realized<Seq<TOut>> ISeqVisitor.SelectManyTaskResultIndexedCancellable<TIn, TTask, TOut>(
        SelectManyTaskResultIndexedCancellableSeq<TIn, TTask, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(
            SelectManyTaskResultIndexedCancellableImpl<TIn, TTask, TOut>,
            seq.Source,
            seq.TaskSelector,
            seq.ResultSelector);

    private static IObservable<TOut> SelectManyTaskImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, Task<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyTaskCancellableImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, CancellationToken, Task<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyTaskIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, Task<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyTaskIndexedCancellableImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, CancellationToken, Task<TOut>> selector) =>
        source.SelectMany(selector);

    private static IObservable<TOut> SelectManyTaskResultImpl<TIn, TTask, TOut>(
        IObservable<TIn> source,
        Func<TIn, Task<TTask>> taskSelector,
        Func<TIn, TTask, TOut> resultSelector) =>
        source.SelectMany(taskSelector, resultSelector);

    private static IObservable<TOut> SelectManyTaskResultCancellableImpl<TIn, TTask, TOut>(
        IObservable<TIn> source,
        Func<TIn, CancellationToken, Task<TTask>> taskSelector,
        Func<TIn, TTask, TOut> resultSelector) =>
        source.SelectMany(taskSelector, resultSelector);

    private static IObservable<TOut> SelectManyTaskResultIndexedImpl<TIn, TTask, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, Task<TTask>> taskSelector,
        Func<TIn, int, TTask, TOut> resultSelector) =>
        source.SelectMany(taskSelector, resultSelector);

    private static IObservable<TOut> SelectManyTaskResultIndexedCancellableImpl<TIn, TTask, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, CancellationToken, Task<TTask>> taskSelector,
        Func<TIn, int, TTask, TOut> resultSelector) =>
        source.SelectMany(taskSelector, resultSelector);
}
