// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TOut>> ISeqVisitor.Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectIndexedImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<T>> ISeqVisitor.Where<T>(WhereSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhereImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<TResult>> ISeqVisitor.Zip<TFirst, TSecond, TResult>(ZipSeq<TFirst, TSecond, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(ZipImpl<TFirst, TSecond, TResult>, seq.First, seq.Second, seq.ResultSelector);

    Realized<Seq<T>> ISeqVisitor.Concat<T>(ConcatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.ConcatNested<T>(ConcatNestedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatNestedImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.Repeat<T>(RepeatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RepeatCount<T>(RepeatCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatCountImpl<T>, seq.Source, seq.RepeatCount);

    private static IObservable<TOut> SelectImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, TOut> selector) =>
        source.Select(selector);

    private static IObservable<TOut> SelectIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, TOut> selector) =>
        source.Select(selector);

    private static IObservable<T> WhereImpl<T>(IObservable<T> source, Func<T, bool> predicate) =>
        source.Where(predicate);

    private static IObservable<TResult> ZipImpl<TFirst, TSecond, TResult>(
        IObservable<TFirst> first,
        IObservable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector) =>
        first.Zip(second, resultSelector);

    private static IObservable<T> ConcatImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.Concat(second);

    private static IObservable<T> ConcatNestedImpl<T>(IObservable<IObservable<T>> sources) =>
        sources.Concat();

    private static IObservable<T> RepeatImpl<T>(IObservable<T> source) => source.Repeat();

    private static IObservable<T> RepeatCountImpl<T>(IObservable<T> source, int repeatCount) =>
        source.Repeat(repeatCount);
}
