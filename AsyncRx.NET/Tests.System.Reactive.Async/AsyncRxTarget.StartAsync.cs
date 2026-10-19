// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.StartAsync<TResult>(StartAsyncSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(StartAsyncImpl<TResult>, seq.FunctionAsync)
            : _bridge.Run<Seq<TResult>>(StartAsyncOnImpl<TResult>, seq.FunctionAsync, Unwrap(
                    seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.StartAsyncCancel<TResult>(
            StartAsyncCancelSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(StartAsyncCancelImpl<TResult>, seq.FunctionAsync)
            : _bridge.Run<Seq<TResult>>(StartAsyncCancelOnImpl<TResult>, seq.FunctionAsync, Unwrap(
                    seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.StartAsyncAction(StartAsyncActionSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(StartAsyncActionImpl, seq.ActionAsync)
            : _bridge.Run<Seq<Unit>>(StartAsyncActionOnImpl, seq.ActionAsync, Unwrap(
                    seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.StartAsyncActionCancel(StartAsyncActionCancelSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(StartAsyncActionCancelImpl, seq.ActionAsync)
            : _bridge.Run<Seq<Unit>>(StartAsyncActionCancelOnImpl, seq.ActionAsync, Unwrap(
                    seq.Scheduler));

    // Rx.NET's test text supplies a Task-returning function; AsyncRx.NET's idiom is ValueTask
    // (the maintainer's decision, Step 59), so the impl adapts the delegate rather than the
    // library gaining a Task-taking overload.
    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> StartAsyncImpl<TResult>(
            Func<Task<TResult>> functionAsync) =>
        AsyncObservable.StartAsync(() => new ValueTask<TResult>(functionAsync()));

    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> StartAsyncOnImpl<TResult>(
            Func<Task<TResult>> functionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.StartAsync(() => new ValueTask<TResult>(functionAsync()), scheduler);

    // Rx.NET's test text supplies a Task-returning function; AsyncRx.NET's idiom is ValueTask
    // (the maintainer's decision, Step 59), so the impl adapts the delegate rather than the
    // library gaining a Task-taking overload.
    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> StartAsyncCancelImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        AsyncObservable.StartAsync(ct => new ValueTask<TResult>(functionAsync(ct)));

    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> StartAsyncCancelOnImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.StartAsync(ct => new ValueTask<TResult>(functionAsync(ct)), scheduler);

    private static IAsyncObservable<Unit> StartAsyncActionImpl(Func<Task> actionAsync) =>
        AsyncObservable.StartAsync(actionAsync);

    private static IAsyncObservable<Unit> StartAsyncActionOnImpl(Func<Task> actionAsync,
            IAsyncScheduler scheduler) =>
        AsyncObservable.StartAsync(actionAsync, scheduler);

    private static IAsyncObservable<Unit> StartAsyncActionCancelImpl(Func<CancellationToken,
            Task> actionAsync) =>
        AsyncObservable.StartAsync(actionAsync);

    private static IAsyncObservable<Unit> StartAsyncActionCancelOnImpl(Func<CancellationToken,
            Task> actionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.StartAsync(actionAsync, scheduler);
}
