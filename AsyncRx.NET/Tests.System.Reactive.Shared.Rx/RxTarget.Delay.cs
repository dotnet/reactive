// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.DelayTime<T>(DelayTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(DelayTimeDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(DelayTimeImpl<T>, seq.Source, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.DelayAbsolute<T>(DelayAbsoluteSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(DelayAbsoluteDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                DelayAbsoluteImpl<T>,
                seq.Source,
                seq.DueTime,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq) =>
        _bridge.Run<Seq<T>>(DelaySelectorImpl<T, TDelay>, seq.Source, seq.DelayDurationSelector);

    Realized<Seq<T>> ISeqVisitor.DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq) =>
        _bridge.Run<Seq<T>>(
            DelaySubscriptionImpl<T, TDelay>,
            seq.Source,
            seq.SubscriptionDelay,
            seq.DelayDurationSelector);

    private static IObservable<T> DelayTimeImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IScheduler scheduler) =>
        source.Delay(dueTime, scheduler);

    private static IObservable<T> DelayAbsoluteImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime,
        IScheduler scheduler) =>
        source.Delay(dueTime, scheduler);

    private static IObservable<T> DelayTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime) =>
        source.Delay(dueTime);

    private static IObservable<T> DelayAbsoluteDefaultImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime) =>
        source.Delay(dueTime);

    private static IObservable<T> DelaySelectorImpl<T, TDelay>(
        IObservable<T> source,
        Func<T, IObservable<TDelay>> delayDurationSelector) =>
        source.Delay(delayDurationSelector);

    private static IObservable<T> DelaySubscriptionImpl<T, TDelay>(
        IObservable<T> source,
        IObservable<TDelay> subscriptionDelay,
        Func<T, IObservable<TDelay>> delayDurationSelector) =>
        source.Delay(subscriptionDelay, delayDurationSelector);
}
