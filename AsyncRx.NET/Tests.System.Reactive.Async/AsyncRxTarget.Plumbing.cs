// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Where<T>(WhereSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhereImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.Concat<T>(ConcatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.ConcatNested<T>(ConcatNestedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatNestedImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.Repeat<T>(RepeatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RepeatCount<T>(RepeatCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatCountImpl<T>, seq.Source, seq.RepeatCount);

    private static IAsyncObservable<T> WhereImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.Where(predicate);

    private static IAsyncObservable<T> ConcatImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second) =>
        first.Concat(second);

    private static IAsyncObservable<T> ConcatNestedImpl<T>(IAsyncObservable<IAsyncObservable<T>> sources) =>
        sources.Concat();

    private static IAsyncObservable<T> RepeatImpl<T>(IAsyncObservable<T> source) => source.Repeat();

    private static IAsyncObservable<T> RepeatCountImpl<T>(
        IAsyncObservable<T> source,
        int repeatCount) =>
        source.Repeat(repeatCount);
}
