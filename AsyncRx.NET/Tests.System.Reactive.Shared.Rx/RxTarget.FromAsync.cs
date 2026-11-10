// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<TResult> FromAsyncImpl<TResult>(Func<Task<TResult>> functionAsync) =>
        Observable.FromAsync(functionAsync);

    private static IObservable<TResult> FromAsyncOnImpl<TResult>(Func<Task<TResult>> functionAsync,
            IScheduler scheduler) =>
        Observable.FromAsync(functionAsync, scheduler);

    private static IObservable<TResult> FromAsyncCancelImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        Observable.FromAsync(functionAsync);

    private static IObservable<TResult> FromAsyncCancelOnImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, IScheduler scheduler) =>
        Observable.FromAsync(functionAsync, scheduler);

    private static IObservable<Unit> FromAsyncActionImpl(Func<Task> actionAsync) =>
        Observable.FromAsync(actionAsync);

    private static IObservable<Unit> FromAsyncActionOnImpl(Func<Task> actionAsync,
            IScheduler scheduler) =>
        Observable.FromAsync(actionAsync, scheduler);

    private static IObservable<Unit> FromAsyncActionCancelImpl(Func<CancellationToken,
            Task> actionAsync) =>
        Observable.FromAsync(actionAsync);

    private static IObservable<Unit> FromAsyncActionCancelOnImpl(Func<CancellationToken,
            Task> actionAsync, IScheduler scheduler) =>
        Observable.FromAsync(actionAsync, scheduler);
}
