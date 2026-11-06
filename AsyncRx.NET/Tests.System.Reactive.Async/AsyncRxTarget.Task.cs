// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.FromTask<TResult>(TaskSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(TaskToObservableImpl<TResult>, seq.Task)
            : _bridge.Run<Seq<TResult>>(
                TaskToObservableOnImpl<TResult>, seq.Task, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.FromVoidTask(VoidTaskSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(VoidTaskToObservableImpl, seq.Task)
            : _bridge.Run<Seq<Unit>>(VoidTaskToObservableOnImpl, seq.Task, Unwrap(seq.Scheduler));

    private static IAsyncObservable<TResult> TaskToObservableImpl<TResult>(Task<TResult> task) =>
        task.ToAsyncObservable();

    private static IAsyncObservable<TResult> TaskToObservableOnImpl<TResult>(
            Task<TResult> task, IAsyncScheduler scheduler) =>
        task.ToAsyncObservable(scheduler);

    private static IAsyncObservable<Unit> VoidTaskToObservableImpl(Task task) =>
        task.ToAsyncObservable();

    private static IAsyncObservable<Unit> VoidTaskToObservableOnImpl(
            Task task, IAsyncScheduler scheduler) =>
        task.ToAsyncObservable(scheduler);
}
