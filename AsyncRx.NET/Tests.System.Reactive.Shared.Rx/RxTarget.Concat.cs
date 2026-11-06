// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<T> ConcatImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.Concat(second);

    private static IObservable<T> ConcatNestedImpl<T>(IObservable<IObservable<T>> sources) =>
        sources.Concat();

    private static IObservable<T> ConcatParamsImpl<T>(IObservable<T>[] sources) =>
        Observable.Concat(sources);

    private static IObservable<T> ConcatEnumerableImpl<T>(IEnumerable<IObservable<T>> sources) =>
        sources.Concat();

    private static IObservable<T> ConcatTasksImpl<T>(IObservable<Task<T>> sources) =>
        sources.Concat();
}
