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
    Realized<Seq<TResult>> ISeqVisitor.FromAsync<TResult>(FromAsyncSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(FromAsyncImpl<TResult>, seq.FunctionAsync)
            : _bridge.Run<Seq<TResult>>(FromAsyncOnImpl<TResult>, seq.FunctionAsync, Unwrap(
                    seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.FromAsyncCancel<TResult>(FromAsyncCancelSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(FromAsyncCancelImpl<TResult>, seq.FunctionAsync)
            : _bridge.Run<Seq<TResult>>(FromAsyncCancelOnImpl<TResult>, seq.FunctionAsync, Unwrap(
                    seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncAction(FromAsyncActionSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(FromAsyncActionImpl, seq.ActionAsync)
            : _bridge.Run<Seq<Unit>>(FromAsyncActionOnImpl, seq.ActionAsync, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.FromAsyncActionCancel(FromAsyncActionCancelSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(FromAsyncActionCancelImpl, seq.ActionAsync)
            : _bridge.Run<Seq<Unit>>(FromAsyncActionCancelOnImpl, seq.ActionAsync, Unwrap(
                    seq.Scheduler));

    // Rx.NET's test text supplies a Task-returning function; AsyncRx.NET's idiom is ValueTask
    // (the maintainer's decision, Step 59), so the impl adapts the delegate rather than the
    // library gaining a Task-taking overload.
    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> FromAsyncImpl<TResult>(
            Func<Task<TResult>> functionAsync) =>
        AsyncObservable.FromAsync(() => new ValueTask<TResult>(functionAsync()));

    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> FromAsyncOnImpl<TResult>(
            Func<Task<TResult>> functionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.FromAsync(() => new ValueTask<TResult>(functionAsync()), scheduler);

    // Rx.NET's test text supplies a Task-returning function; AsyncRx.NET's idiom is ValueTask
    // (the maintainer's decision, Step 59), so the impl adapts the delegate rather than the
    // library gaining a Task-taking overload.
    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> FromAsyncCancelImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        AsyncObservable.FromAsync(ct => new ValueTask<TResult>(functionAsync(ct)));

    [AdaptsLibraryCall]
    private static IAsyncObservable<TResult> FromAsyncCancelOnImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.FromAsync(ct => new ValueTask<TResult>(functionAsync(ct)), scheduler);

    private static IAsyncObservable<Unit> FromAsyncActionImpl(Func<Task> actionAsync) =>
        AsyncObservable.FromAsync(actionAsync);

    private static IAsyncObservable<Unit> FromAsyncActionOnImpl(Func<Task> actionAsync,
            IAsyncScheduler scheduler) =>
        AsyncObservable.FromAsync(actionAsync, scheduler);

    private static IAsyncObservable<Unit> FromAsyncActionCancelImpl(Func<CancellationToken,
            Task> actionAsync) =>
        AsyncObservable.FromAsync(actionAsync);

    private static IAsyncObservable<Unit> FromAsyncActionCancelOnImpl(Func<CancellationToken,
            Task> actionAsync, IAsyncScheduler scheduler) =>
        AsyncObservable.FromAsync(actionAsync, scheduler);
}
