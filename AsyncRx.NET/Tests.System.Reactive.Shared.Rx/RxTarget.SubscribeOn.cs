// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SubscribeOn<T>(SubscribeOnSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SubscribeOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SubscribeOnContext<T>(SubscribeOnContextSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SubscribeOnContextImpl<T>, seq.Source, seq.Context);

    private static IObservable<T> SubscribeOnImpl<T>(IObservable<T> source, IScheduler scheduler) =>
        source.SubscribeOn(scheduler);

    private static IObservable<T> SubscribeOnContextImpl<T>(
        IObservable<T> source,
        SynchronizationContext context) =>
        source.SubscribeOn(context);
}
