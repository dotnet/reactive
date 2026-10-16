// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.SubscribeOn<T>(SubscribeOnSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SubscribeOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SubscribeOnContext<T>(SubscribeOnContextSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SubscribeOnContextImpl<T>, seq.Source, seq.Context);

    private static IAsyncObservable<T> SubscribeOnImpl<T>(
        IAsyncObservable<T> source,
        IAsyncScheduler scheduler) =>
        source.SubscribeOn(scheduler);

    private static IAsyncObservable<T> SubscribeOnContextImpl<T>(
        IAsyncObservable<T> source,
        SynchronizationContext context) =>
        source.SubscribeOn(context);
}
