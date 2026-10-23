// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<TResult> GenerateImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector);

    private static IAsyncObservable<TResult> GenerateOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, scheduler);

    private static IAsyncObservable<TResult> GenerateTimeImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector);

    private static IAsyncObservable<TResult> GenerateTimeOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler);

    private static IAsyncObservable<TResult> GenerateAbsoluteImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector);

    private static IAsyncObservable<TResult> GenerateAbsoluteOnImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler);
}
