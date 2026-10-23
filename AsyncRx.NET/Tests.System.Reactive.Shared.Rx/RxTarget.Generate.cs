// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.Generate<TState, TResult>(
        GenerateSeq<TState, TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(
                GenerateImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector)
            : _bridge.Run<Seq<TResult>>(
                GenerateOnImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.GenerateTime<TState, TResult>(
        GenerateTimeSeq<TState, TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(
                GenerateTimeImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector)
            : _bridge.Run<Seq<TResult>>(
                GenerateTimeOnImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector,
                Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.GenerateAbsolute<TState, TResult>(
        GenerateAbsoluteSeq<TState, TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(
                GenerateAbsoluteImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector)
            : _bridge.Run<Seq<TResult>>(
                GenerateAbsoluteOnImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector,
                Unwrap(seq.Scheduler));

    private static IObservable<TResult> GenerateImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector);

    private static IObservable<TResult> GenerateOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        IScheduler scheduler) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector, scheduler);

    private static IObservable<TResult> GenerateTimeImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector);

    private static IObservable<TResult> GenerateTimeOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector,
        IScheduler scheduler) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler);

    private static IObservable<TResult> GenerateAbsoluteImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector);

    private static IObservable<TResult> GenerateAbsoluteOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector,
        IScheduler scheduler) =>
        Observable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler);
}
