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

    Realized<Seq<TOut>> ISeqVisitor.SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectManyImpl<TIn, TOut>, seq.Source, seq.Other);

    Realized<Seq<T>> ISeqVisitor.Concat<T>(ConcatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ConcatImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.Merge<T>(MergeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeImpl<T>, seq.Sources);

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

    private static IObservable<TOut> SelectManyImpl<TIn, TOut>(
        IObservable<TIn> source,
        IObservable<TOut> other) =>
        source.SelectMany(other);

    private static IObservable<T> ConcatImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.Concat(second);

    private static IObservable<T> MergeImpl<T>(IObservable<IObservable<T>> sources) =>
        sources.Merge();
}
