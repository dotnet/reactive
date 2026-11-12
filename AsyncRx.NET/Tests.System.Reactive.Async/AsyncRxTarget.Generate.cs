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
            ? RunByForm<Seq<TResult>>(
                GenerateImpl<TState, TResult>, GenerateAsyncImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector)
            : RunByForm<Seq<TResult>>(
                GenerateOnImpl<TState, TResult>, GenerateOnAsyncImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.GenerateTime<TState, TResult>(
        GenerateTimeSeq<TState, TResult> seq) =>
        seq.Scheduler is null
            ? RunByForm<Seq<TResult>>(
                GenerateTimeImpl<TState, TResult>, GenerateTimeAsyncImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector)
            : RunByForm<Seq<TResult>>(
                GenerateTimeOnImpl<TState, TResult>, GenerateTimeOnAsyncImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector,
                Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.GenerateAbsolute<TState, TResult>(
        GenerateAbsoluteSeq<TState, TResult> seq) =>
        seq.Scheduler is null
            ? RunByForm<Seq<TResult>>(
                GenerateAbsoluteImpl<TState, TResult>, GenerateAbsoluteAsyncImpl<TState, TResult>,
                seq.InitialState,
                seq.Condition,
                seq.Iterate,
                seq.ResultSelector,
                seq.TimeSelector)
            : RunByForm<Seq<TResult>>(
                GenerateAbsoluteOnImpl<TState, TResult>,
                GenerateAbsoluteOnAsyncImpl<TState, TResult>, seq.InitialState, seq.Condition,
                seq.Iterate, seq.ResultSelector, seq.TimeSelector, Unwrap(seq.Scheduler));

    private static IAsyncObservable<TResult> GenerateImpl<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector);

    private static IAsyncObservable<TResult> GenerateAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector) =>
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

    private static IAsyncObservable<TResult> GenerateOnAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector,
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

    private static IAsyncObservable<TResult> GenerateTimeAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector,
        Func<TState, ValueTask<TimeSpan>> timeSelector) =>
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

    private static IAsyncObservable<TResult> GenerateTimeOnAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector,
        Func<TState, ValueTask<TimeSpan>> timeSelector,
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

    private static IAsyncObservable<TResult> GenerateAbsoluteAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector,
        Func<TState, ValueTask<DateTimeOffset>> timeSelector) =>
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

    private static IAsyncObservable<TResult> GenerateAbsoluteOnAsyncImpl<TState, TResult>(
        TState initialState,
        Func<TState, ValueTask<bool>> condition,
        Func<TState, ValueTask<TState>> iterate,
        Func<TState, ValueTask<TResult>> resultSelector,
        Func<TState, ValueTask<DateTimeOffset>> timeSelector,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Generate(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler);
}
