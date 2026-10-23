// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.ObserveOn<T>(ObserveOnSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ObserveOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.ObserveOnContext<T>(ObserveOnContextSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ObserveOnContextImpl<T>, seq.Source, seq.Context);

    private static IObservable<T> ObserveOnImpl<T>(IObservable<T> source, IScheduler scheduler) =>
        source.ObserveOn(scheduler);

    private static IObservable<T> ObserveOnContextImpl<T>(
        IObservable<T> source,
        SynchronizationContext context) =>
        source.ObserveOn(context);
}
