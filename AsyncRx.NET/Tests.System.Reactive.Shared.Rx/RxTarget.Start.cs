// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.Start<TResult>(StartSeq<TResult> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TResult>>(StartImpl<TResult>, seq.Function)
            : _bridge.Run<Seq<TResult>>(StartOnImpl<TResult>, seq.Function, Unwrap(seq.Scheduler));

    Realized<Seq<Unit>> ISeqVisitor.StartAction(StartActionSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<Unit>>(StartActionImpl, seq.Action)
            : _bridge.Run<Seq<Unit>>(StartActionOnImpl, seq.Action, Unwrap(seq.Scheduler));

    private static IObservable<TResult> StartImpl<TResult>(Func<TResult> function) =>
        Observable.Start(function);

    private static IObservable<TResult> StartOnImpl<TResult>(Func<TResult> function,
            IScheduler scheduler) =>
        Observable.Start(function, scheduler);

    private static IObservable<Unit> StartActionImpl(Action action) =>
        Observable.Start(action);

    private static IObservable<Unit> StartActionOnImpl(Action action, IScheduler scheduler) =>
        Observable.Start(action, scheduler);
}
