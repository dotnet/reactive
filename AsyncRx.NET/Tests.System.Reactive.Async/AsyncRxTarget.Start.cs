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
    Realized<Seq<TResult>> ISeqVisitor.Start<TResult>(StartSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(StartImpl<TResult>, seq.Function)
            : _bridge.Run<Seq<TResult>>(StartOnImpl<TResult>, seq.Function, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.StartAction(StartActionSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(StartActionImpl, seq.Action)
            : _bridge.Run<Seq<Unit>>(StartActionOnImpl, seq.Action, Unwrap(seq.Scheduler));

    private static IAsyncObservable<TResult> StartImpl<TResult>(Func<TResult> function) =>
        AsyncObservable.Start<TResult>(function);

    private static IAsyncObservable<TResult> StartOnImpl<TResult>(Func<TResult> function,
            IAsyncScheduler scheduler) =>
        AsyncObservable.Start<TResult>(function, scheduler);

    private static IAsyncObservable<Unit> StartActionImpl(Action action) =>
        AsyncObservable.Start(action);

    private static IAsyncObservable<Unit> StartActionOnImpl(Action action,
            IAsyncScheduler scheduler) =>
        AsyncObservable.Start(action, scheduler);
}
