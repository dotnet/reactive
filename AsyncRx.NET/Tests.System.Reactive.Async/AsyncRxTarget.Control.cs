// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;
namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.If<TResult>(IfSeq<TResult> seq) =>
        seq.ElseSource is not null
            ? RunByForm<Seq<TResult>>(
                IfElseImpl<TResult>, IfElseAsyncImpl<TResult>, seq.Condition, seq.ThenSource,
                seq.ElseSource)
        : seq.Scheduler is not null
            ? RunByForm<Seq<TResult>>(
                IfOnImpl<TResult>, IfOnAsyncImpl<TResult>, seq.Condition, seq.ThenSource,
                Unwrap(seq.Scheduler))
        : RunByForm<Seq<TResult>>(
            IfImpl<TResult>, IfAsyncImpl<TResult>, seq.Condition, seq.ThenSource);

    Realized<Seq<TResult>> ISeqVisitor.Case<TValue, TResult>(CaseSeq<TValue, TResult> seq) =>
        seq.DefaultSource is not null
            ? RunByForm<Seq<TResult>>(
                CaseDefaultImpl<TValue, TResult>, CaseDefaultAsyncImpl<TValue, TResult>,
                seq.Selector, seq.Sources, seq.DefaultSource)
        : seq.Scheduler is not null
            ? RunByForm<Seq<TResult>>(
                CaseOnImpl<TValue, TResult>, CaseOnAsyncImpl<TValue, TResult>, seq.Selector,
                seq.Sources, Unwrap(seq.Scheduler))
        : RunByForm<Seq<TResult>>(
            CaseImpl<TValue, TResult>, CaseAsyncImpl<TValue, TResult>, seq.Selector, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.While<T>(WhileSeq<T> seq) =>
        RunByForm<Seq<T>>(WhileImpl<T>, WhileAsyncImpl<T>, seq.Condition, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DoWhile<T>(DoWhileSeq<T> seq) =>
        RunByForm<Seq<T>>(DoWhileImpl<T>, DoWhileAsyncImpl<T>, seq.Source, seq.Condition);

    Realized<Seq<TResult>> ISeqVisitor.For<TSource, TResult>(ForSeq<TSource, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            ForImpl<TSource, TResult>, ForAsyncImpl<TSource, TResult>, seq.Source,
            seq.ResultSelector);

    private static IAsyncObservable<TResult> IfImpl<TResult>(
        Func<bool> condition,
        IAsyncObservable<TResult> thenSource) =>
        AsyncObservable.If(condition, thenSource);

    private static IAsyncObservable<TResult> IfAsyncImpl<TResult>(
        Func<ValueTask<bool>> condition,
        IAsyncObservable<TResult> thenSource) =>
        AsyncObservable.If(condition, thenSource);

    private static IAsyncObservable<TResult> IfElseImpl<TResult>(
        Func<bool> condition,
        IAsyncObservable<TResult> thenSource,
        IAsyncObservable<TResult> elseSource) =>
        AsyncObservable.If(condition, thenSource, elseSource);

    private static IAsyncObservable<TResult> IfElseAsyncImpl<TResult>(
        Func<ValueTask<bool>> condition,
        IAsyncObservable<TResult> thenSource,
        IAsyncObservable<TResult> elseSource) =>
        AsyncObservable.If(condition, thenSource, elseSource);

    private static IAsyncObservable<TResult> IfOnImpl<TResult>(
        Func<bool> condition,
        IAsyncObservable<TResult> thenSource,
        IAsyncScheduler scheduler) =>
        AsyncObservable.If(condition, thenSource, scheduler);

    private static IAsyncObservable<TResult> IfOnAsyncImpl<TResult>(
        Func<ValueTask<bool>> condition,
        IAsyncObservable<TResult> thenSource,
        IAsyncScheduler scheduler) =>
        AsyncObservable.If(condition, thenSource, scheduler);

    private static IAsyncObservable<TResult> CaseImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources);

    private static IAsyncObservable<TResult> CaseAsyncImpl<TValue, TResult>(
        Func<ValueTask<TValue>> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources);

    private static IAsyncObservable<TResult> CaseDefaultImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources,
        IAsyncObservable<TResult> defaultSource)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources, defaultSource);

    private static IAsyncObservable<TResult> CaseDefaultAsyncImpl<TValue, TResult>(
        Func<ValueTask<TValue>> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources,
        IAsyncObservable<TResult> defaultSource)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources, defaultSource);

    private static IAsyncObservable<TResult> CaseOnImpl<TValue, TResult>(
        Func<TValue> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources,
        IAsyncScheduler scheduler)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources, scheduler);

    private static IAsyncObservable<TResult> CaseOnAsyncImpl<TValue, TResult>(
        Func<ValueTask<TValue>> selector,
        IDictionary<TValue, IAsyncObservable<TResult>> sources,
        IAsyncScheduler scheduler)
        where TValue : notnull =>
        AsyncObservable.Case(selector, sources, scheduler);

    private static IAsyncObservable<T> WhileImpl<T>(
        Func<bool> condition,
        IAsyncObservable<T> source) =>
        AsyncObservable.While(condition, source);

    private static IAsyncObservable<T> WhileAsyncImpl<T>(
        Func<ValueTask<bool>> condition,
        IAsyncObservable<T> source) =>
        AsyncObservable.While(condition, source);

    private static IAsyncObservable<T> DoWhileImpl<T>(
        IAsyncObservable<T> source,
        Func<bool> condition) =>
        AsyncObservable.DoWhile(source, condition);

    private static IAsyncObservable<T> DoWhileAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<ValueTask<bool>> condition) =>
        AsyncObservable.DoWhile(source, condition);

    private static IAsyncObservable<TResult> ForImpl<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, IAsyncObservable<TResult>> resultSelector) =>
        AsyncObservable.For(source, resultSelector);

    private static IAsyncObservable<TResult> ForAsyncImpl<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, ValueTask<IAsyncObservable<TResult>>> resultSelector) =>
        AsyncObservable.For(source, resultSelector);
}
