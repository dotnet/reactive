// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.RefCount<T>(RefCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RefCountMinObservers<T>(RefCountMinObserversSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountMinObserversImpl<T>, seq.Source, seq.MinObservers);

    Realized<Seq<T>> ISeqVisitor.RefCountDelay<T>(RefCountDelaySeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountDelayImpl<T>, seq.Source, seq.DisconnectDelay);

    Realized<Seq<T>> ISeqVisitor.RefCountDelayScheduled<T>(RefCountDelayScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountDelayScheduledImpl<T>, seq.Source, seq.DisconnectDelay, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.RefCountMinObserversDelay<T>(RefCountMinObserversDelaySeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountMinObserversDelayImpl<T>, seq.Source, seq.MinObservers, seq.DisconnectDelay);

    Realized<Seq<T>> ISeqVisitor.RefCountMinObserversDelayScheduled<T>(
        RefCountMinObserversDelayScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            RefCountMinObserversDelayScheduledImpl<T>,
            seq.Source,
            seq.MinObservers,
            seq.DisconnectDelay,
            Unwrap(seq.Scheduler));

    private static IObservable<T> RefCountDelayImpl<T>(IConnectableObservable<T> source, TimeSpan disconnectDelay) =>
        source.RefCount(disconnectDelay);

    private static IObservable<T> RefCountDelayScheduledImpl<T>(
        IConnectableObservable<T> source,
        TimeSpan disconnectDelay,
        IScheduler scheduler) =>
        source.RefCount(disconnectDelay, scheduler);

    private static IObservable<T> RefCountMinObserversDelayImpl<T>(
        IConnectableObservable<T> source,
        int minObservers,
        TimeSpan disconnectDelay) =>
        source.RefCount(minObservers, disconnectDelay);

    private static IObservable<T> RefCountMinObserversDelayScheduledImpl<T>(
        IConnectableObservable<T> source,
        int minObservers,
        TimeSpan disconnectDelay,
        IScheduler scheduler) =>
        source.RefCount(minObservers, disconnectDelay, scheduler);

    private static IObservable<T> RefCountImpl<T>(IConnectableObservable<T> source) => source.RefCount();

    private static IObservable<T> RefCountMinObserversImpl<T>(IConnectableObservable<T> source, int minObservers) =>
        source.RefCount(minObservers);
}
