// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.If<TResult>(IfSeq<TResult> seq) =>
        seq.ElseSource is not null
            ? _bridge.Run<Seq<TResult>>(
                IfElseImpl<TResult>, seq.Condition, seq.ThenSource, seq.ElseSource)
        : seq.Scheduler is not null
            ? _bridge.Run<Seq<TResult>>(
                IfOnImpl<TResult>, seq.Condition, seq.ThenSource, Unwrap(seq.Scheduler))
        : _bridge.Run<Seq<TResult>>(IfImpl<TResult>, seq.Condition, seq.ThenSource);

    Realized<Seq<TResult>> ISeqVisitor.Case<TValue, TResult>(CaseSeq<TValue, TResult> seq) =>
        seq.DefaultSource is not null
            ? _bridge.Run<Seq<TResult>>(
                CaseDefaultImpl<TValue, TResult>, seq.Selector, seq.Sources, seq.DefaultSource)
        : seq.Scheduler is not null
            ? _bridge.Run<Seq<TResult>>(
                CaseOnImpl<TValue, TResult>, seq.Selector, seq.Sources, Unwrap(seq.Scheduler))
        : _bridge.Run<Seq<TResult>>(CaseImpl<TValue, TResult>, seq.Selector, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.While<T>(WhileSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhileImpl<T>, seq.Condition, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DoWhile<T>(DoWhileSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoWhileImpl<T>, seq.Source, seq.Condition);

    Realized<Seq<TResult>> ISeqVisitor.For<TSource, TResult>(ForSeq<TSource, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(ForImpl<TSource, TResult>, seq.Source, seq.ResultSelector);

    private static IObservable<TResult> IfImpl<TResult>(
        Func<bool> condition,
        IObservable<TResult> thenSource) =>
        Observable.If(condition, thenSource);

    private static IObservable<TResult> IfElseImpl<TResult>(
        Func<bool> condition,
        IObservable<TResult> thenSource,
        IObservable<TResult> elseSource) =>
        Observable.If(condition, thenSource, elseSource);

    private static IObservable<TResult> IfOnImpl<TResult>(
        Func<bool> condition,
        IObservable<TResult> thenSource,
        IScheduler scheduler) =>
        Observable.If(condition, thenSource, scheduler);

    private static IObservable<TResult> CaseImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IObservable<TResult>> sources)
        where TValue : notnull =>
        Observable.Case(selector, sources);

    private static IObservable<TResult> CaseDefaultImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IObservable<TResult>> sources,
        IObservable<TResult> defaultSource)
        where TValue : notnull =>
        Observable.Case(selector, sources, defaultSource);

    private static IObservable<TResult> CaseOnImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IObservable<TResult>> sources,
        IScheduler scheduler)
        where TValue : notnull =>
        Observable.Case(selector, sources, scheduler);

    private static IObservable<T> WhileImpl<T>(Func<bool> condition, IObservable<T> source) =>
        Observable.While(condition, source);

    private static IObservable<T> DoWhileImpl<T>(IObservable<T> source, Func<bool> condition) =>
        Observable.DoWhile(source, condition);

    private static IObservable<TResult> ForImpl<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, IObservable<TResult>> resultSelector) =>
        Observable.For(source, resultSelector);
}
