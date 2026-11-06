// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Threading.Tasks;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    // Without a scheduler Rx.NET queues the conversion's continuation to the ambient
    // TaskScheduler.Current; the originals run inside a task factory over an inline task
    // scheduler so that it runs on the completing thread, and so does each subscription here.
    Realized<Seq<TResult>> ISeqVisitor.FromTask<TResult>(TaskSeq<TResult> seq) =>
        seq.Scheduler is null
            ? Realized.Of<Seq<TResult>>(InlineTaskScheduler.Subscribing(
                _bridge.Run<Seq<TResult>>(TaskToObservableImpl<TResult>, seq.Task)
                    .Get<IObservable<TResult>>()))
            : _bridge.Run<Seq<TResult>>(
                TaskToObservableOnImpl<TResult>, seq.Task, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.FromVoidTask(VoidTaskSeq seq) =>
        seq.Scheduler is null
            ? Realized.Of<Seq<Unit>>(InlineTaskScheduler.Subscribing(
                _bridge.Run<Seq<Unit>>(VoidTaskToObservableImpl, seq.Task)
                    .Get<IObservable<Unit>>()))
            : _bridge.Run<Seq<Unit>>(VoidTaskToObservableOnImpl, seq.Task, Unwrap(seq.Scheduler));

    private static IObservable<TResult> TaskToObservableImpl<TResult>(Task<TResult> task) =>
        task.ToObservable();

    private static IObservable<TResult> TaskToObservableOnImpl<TResult>(
            Task<TResult> task, IScheduler scheduler) =>
        task.ToObservable(scheduler);

    private static IObservable<Unit> VoidTaskToObservableImpl(Task task) => task.ToObservable();

    private static IObservable<Unit> VoidTaskToObservableOnImpl(Task task, IScheduler scheduler) =>
        task.ToObservable(scheduler);
}
