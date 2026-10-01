// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.DelayTime<T>(DelayTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DelayTimeImpl<T>, seq.Source, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.DelayAbsolute<T>(DelayAbsoluteSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DelayAbsoluteImpl<T>, seq.Source, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq) =>
        _bridge.Run<Seq<T>>(DelaySelectorImpl<T, TDelay>, seq.Source, seq.DelayDurationSelector);

    Realized<Seq<T>> ISeqVisitor.DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq) =>
        _bridge.Run<Seq<T>>(
            DelaySubscriptionImpl<T, TDelay>,
            seq.Source,
            seq.SubscriptionDelay,
            seq.DelayDurationSelector);

    private static IAsyncObservable<T> DelayTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime,
        IAsyncScheduler scheduler) =>
        source.Delay(dueTime, scheduler);

    private static IAsyncObservable<T> DelayAbsoluteImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset dueTime,
        IAsyncScheduler scheduler) =>
        source.Delay(dueTime, scheduler);

    private static IAsyncObservable<T> DelaySelectorImpl<T, TDelay>(
        IAsyncObservable<T> source,
        Func<T, IAsyncObservable<TDelay>> delayDurationSelector) =>
        source.Delay(delayDurationSelector);

    private static IAsyncObservable<T> DelaySubscriptionImpl<T, TDelay>(
        IAsyncObservable<T> source,
        IAsyncObservable<TDelay> subscriptionDelay,
        Func<T, IAsyncObservable<TDelay>> delayDurationSelector) =>
        source.Delay(subscriptionDelay, delayDurationSelector);
}
