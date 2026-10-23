// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Concat<T>(ConcatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.ConcatNested<T>(ConcatNestedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatNestedImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.ConcatParams<T>(ConcatParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.ConcatEnumerable<T>(ConcatEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatEnumerableImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.ConcatTasks<T>(ConcatTasksSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatTasksImpl<T>, seq.Sources);

    private static IAsyncObservable<T> ConcatImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second) =>
        first.Concat(second);

    private static IAsyncObservable<T> ConcatNestedImpl<T>(
        IAsyncObservable<IAsyncObservable<T>> sources) =>
        sources.Concat();

    private static IAsyncObservable<T> ConcatParamsImpl<T>(IAsyncObservable<T>[] sources) =>
        AsyncObservable.Concat(sources);

    private static IAsyncObservable<T> ConcatEnumerableImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources) =>
        sources.Concat();

    private static IAsyncObservable<T> ConcatTasksImpl<T>(IAsyncObservable<Task<T>> sources) =>
        sources.Concat();
}
