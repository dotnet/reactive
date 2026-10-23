// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.DelaySubscriptionTime<T>(DelaySubscriptionTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(DelaySubscriptionTimeDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                DelaySubscriptionTimeImpl<T>,
                seq.Source,
                seq.DueTime,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.DelaySubscriptionAbsolute<T>(
        DelaySubscriptionAbsoluteSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(DelaySubscriptionAbsoluteDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                DelaySubscriptionAbsoluteImpl<T>,
                seq.Source,
                seq.DueTime,
                Unwrap(seq.Scheduler));

    private static IObservable<T> DelaySubscriptionTimeImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IScheduler scheduler) =>
        source.DelaySubscription(dueTime, scheduler);

    private static IObservable<T> DelaySubscriptionTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime) =>
        source.DelaySubscription(dueTime);

    private static IObservable<T> DelaySubscriptionAbsoluteImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime,
        IScheduler scheduler) =>
        source.DelaySubscription(dueTime, scheduler);

    private static IObservable<T> DelaySubscriptionAbsoluteDefaultImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime) =>
        source.DelaySubscription(dueTime);
}
