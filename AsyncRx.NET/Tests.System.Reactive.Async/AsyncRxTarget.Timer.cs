// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<long>> ISeqVisitor.Timer(TimerSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(TimerImpl, seq.DueTime)
            : _bridge.Run<Seq<long>>(TimerOnImpl, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<long>> ISeqVisitor.TimerAbsolute(TimerAbsoluteSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(TimerAbsoluteImpl, seq.DueTime)
            : _bridge.Run<Seq<long>>(TimerAbsoluteOnImpl, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<long>> ISeqVisitor.TimerPeriodic(TimerPeriodicSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(TimerPeriodicImpl, seq.DueTime, seq.Period)
            : _bridge.Run<Seq<long>>(
                TimerPeriodicOnImpl, seq.DueTime, seq.Period, Unwrap(seq.Scheduler));

    Realized<Seq<long>> ISeqVisitor.TimerAbsolutePeriodic(TimerAbsolutePeriodicSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(TimerAbsolutePeriodicImpl, seq.DueTime, seq.Period)
            : _bridge.Run<Seq<long>>(
                TimerAbsolutePeriodicOnImpl, seq.DueTime, seq.Period, Unwrap(seq.Scheduler));

    private static IAsyncObservable<long> TimerImpl(TimeSpan dueTime) =>
        AsyncObservable.Timer(dueTime);

    private static IAsyncObservable<long> TimerOnImpl(
        TimeSpan dueTime,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Timer(dueTime, scheduler);

    private static IAsyncObservable<long> TimerAbsoluteImpl(DateTimeOffset dueTime) =>
        AsyncObservable.Timer(dueTime);

    private static IAsyncObservable<long> TimerAbsoluteOnImpl(
        DateTimeOffset dueTime,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Timer(dueTime, scheduler);

    private static IAsyncObservable<long> TimerPeriodicImpl(TimeSpan dueTime, TimeSpan period) =>
        AsyncObservable.Timer(dueTime, period);

    private static IAsyncObservable<long> TimerPeriodicOnImpl(
        TimeSpan dueTime,
        TimeSpan period,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Timer(dueTime, period, scheduler);

    private static IAsyncObservable<long> TimerAbsolutePeriodicImpl(
        DateTimeOffset dueTime,
        TimeSpan period) =>
        AsyncObservable.Timer(dueTime, period);

    private static IAsyncObservable<long> TimerAbsolutePeriodicOnImpl(
        DateTimeOffset dueTime,
        TimeSpan period,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Timer(dueTime, period, scheduler);
}
