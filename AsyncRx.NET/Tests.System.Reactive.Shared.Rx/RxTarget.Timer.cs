// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<long> TimerImpl(TimeSpan dueTime) => Observable.Timer(dueTime);

    private static IObservable<long> TimerOnImpl(TimeSpan dueTime, IScheduler scheduler) =>
        Observable.Timer(dueTime, scheduler);

    private static IObservable<long> TimerAbsoluteImpl(DateTimeOffset dueTime) =>
        Observable.Timer(dueTime);

    private static IObservable<long> TimerAbsoluteOnImpl(
        DateTimeOffset dueTime,
        IScheduler scheduler) =>
        Observable.Timer(dueTime, scheduler);

    private static IObservable<long> TimerPeriodicImpl(TimeSpan dueTime, TimeSpan period) =>
        Observable.Timer(dueTime, period);

    private static IObservable<long> TimerPeriodicOnImpl(
        TimeSpan dueTime,
        TimeSpan period,
        IScheduler scheduler) =>
        Observable.Timer(dueTime, period, scheduler);

    private static IObservable<long> TimerAbsolutePeriodicImpl(
        DateTimeOffset dueTime,
        TimeSpan period) =>
        Observable.Timer(dueTime, period);

    private static IObservable<long> TimerAbsolutePeriodicOnImpl(
        DateTimeOffset dueTime,
        TimeSpan period,
        IScheduler scheduler) =>
        Observable.Timer(dueTime, period, scheduler);
}
