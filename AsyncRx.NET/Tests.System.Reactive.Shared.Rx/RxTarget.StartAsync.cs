// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<TResult> StartAsyncImpl<TResult>(
            Func<Task<TResult>> functionAsync) =>
        Observable.StartAsync(functionAsync);

    private static IObservable<TResult> StartAsyncOnImpl<TResult>(Func<Task<TResult>> functionAsync,
            IScheduler scheduler) =>
        Observable.StartAsync(functionAsync, scheduler);

    private static IObservable<TResult> StartAsyncCancelImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        Observable.StartAsync(functionAsync);

    private static IObservable<TResult> StartAsyncCancelOnImpl<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, IScheduler scheduler) =>
        Observable.StartAsync(functionAsync, scheduler);

    private static IObservable<Unit> StartAsyncActionImpl(Func<Task> actionAsync) =>
        Observable.StartAsync(actionAsync);

    private static IObservable<Unit> StartAsyncActionOnImpl(Func<Task> actionAsync,
            IScheduler scheduler) =>
        Observable.StartAsync(actionAsync, scheduler);

    private static IObservable<Unit> StartAsyncActionCancelImpl(Func<CancellationToken,
            Task> actionAsync) =>
        Observable.StartAsync(actionAsync);

    private static IObservable<Unit> StartAsyncActionCancelOnImpl(Func<CancellationToken,
            Task> actionAsync, IScheduler scheduler) =>
        Observable.StartAsync(actionAsync, scheduler);
}
