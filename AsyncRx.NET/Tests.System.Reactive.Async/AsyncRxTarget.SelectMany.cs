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

    // The task-returning forms. The shared text keeps Rx.NET's Task<T>-returning selectors;
    // AsyncRx.NET takes ValueTask<T>, as its other asynchronous callbacks do, so these wrap the
    // selector's task. The wrapper exposes the same result, fault and cancellation, and AsTask()
    // on it returns the original task, so a scenario that compares tasks by identity still holds.
    private static IAsyncObservable<TOut> SelectManyTaskImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, Task<TOut>> selector) =>
        source.SelectMany(x => new ValueTask<TOut>(selector(x)));

    private static IAsyncObservable<TOut> SelectManyTaskCancellableImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, CancellationToken, Task<TOut>> selector) =>
        source.SelectMany((x, ct) => new ValueTask<TOut>(selector(x, ct)));

    private static IAsyncObservable<TOut> SelectManyTaskIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, Task<TOut>> selector) =>
        source.SelectMany((x, i) => new ValueTask<TOut>(selector(x, i)));

    private static IAsyncObservable<TOut> SelectManyTaskIndexedCancellableImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, CancellationToken, Task<TOut>> selector) =>
        source.SelectMany((x, i, ct) => new ValueTask<TOut>(selector(x, i, ct)));

    private static IAsyncObservable<TOut> SelectManyTaskResultImpl<TIn, TTask, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, Task<TTask>> taskSelector,
        Func<TIn, TTask, TOut> resultSelector) =>
        source.SelectMany(x => new ValueTask<TTask>(taskSelector(x)), resultSelector);

    private static IAsyncObservable<TOut> SelectManyTaskResultCancellableImpl<TIn, TTask, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, CancellationToken, Task<TTask>> taskSelector,
        Func<TIn, TTask, TOut> resultSelector) =>
        source.SelectMany(
            (x, ct) => new ValueTask<TTask>(taskSelector(x, ct)),
            resultSelector);

    private static IAsyncObservable<TOut> SelectManyTaskResultIndexedImpl<TIn, TTask, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, Task<TTask>> taskSelector,
        Func<TIn, int, TTask, TOut> resultSelector) =>
        source.SelectMany(
            (x, i) => new ValueTask<TTask>(taskSelector(x, i)),
            resultSelector);

    private static IAsyncObservable<TOut> SelectManyTaskResultIndexedCancellableImpl<TIn, TTask, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, CancellationToken, Task<TTask>> taskSelector,
        Func<TIn, int, TTask, TOut> resultSelector) =>
        source.SelectMany(
            (x, i, ct) => new ValueTask<TTask>(taskSelector(x, i, ct)),
            resultSelector);
}
