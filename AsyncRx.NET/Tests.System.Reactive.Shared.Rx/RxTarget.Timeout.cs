// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.TimeoutTime<T>(TimeoutTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(TimeoutTimeDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                TimeoutTimeImpl<T>,
                seq.Source,
                seq.DueTime,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TimeoutTimeOther<T>(TimeoutTimeOtherSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(
                TimeoutTimeOtherDefaultImpl<T>,
                seq.Source,
                seq.DueTime,
                seq.Other)
            : _bridge.Run<Seq<T>>(
                TimeoutTimeOtherImpl<T>,
                seq.Source,
                seq.DueTime,
                seq.Other,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TimeoutAbsolute<T>(TimeoutAbsoluteSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(TimeoutAbsoluteDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                TimeoutAbsoluteImpl<T>,
                seq.Source,
                seq.DueTime,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TimeoutAbsoluteOther<T>(TimeoutAbsoluteOtherSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(
                TimeoutAbsoluteOtherDefaultImpl<T>,
                seq.Source,
                seq.DueTime,
                seq.Other)
            : _bridge.Run<Seq<T>>(
                TimeoutAbsoluteOtherImpl<T>,
                seq.Source,
                seq.DueTime,
                seq.Other,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TimeoutDuration<T, TTimeout>(
        TimeoutDurationSeq<T, TTimeout> seq) =>
        _bridge.Run<Seq<T>>(
            TimeoutDurationImpl<T, TTimeout>,
            seq.Source,
            seq.TimeoutDurationSelector);

    Realized<Seq<T>> ISeqVisitor.TimeoutDurationOther<T, TTimeout>(
        TimeoutDurationOtherSeq<T, TTimeout> seq) =>
        _bridge.Run<Seq<T>>(
            TimeoutDurationOtherImpl<T, TTimeout>,
            seq.Source,
            seq.TimeoutDurationSelector,
            seq.Other);

    Realized<Seq<T>> ISeqVisitor.TimeoutFirstDuration<T, TTimeout>(
        TimeoutFirstDurationSeq<T, TTimeout> seq) =>
        _bridge.Run<Seq<T>>(
            TimeoutFirstDurationImpl<T, TTimeout>,
            seq.Source,
            seq.FirstTimeout,
            seq.TimeoutDurationSelector);

    Realized<Seq<T>> ISeqVisitor.TimeoutFirstDurationOther<T, TTimeout>(
        TimeoutFirstDurationOtherSeq<T, TTimeout> seq) =>
        _bridge.Run<Seq<T>>(
            TimeoutFirstDurationOtherImpl<T, TTimeout>,
            seq.Source,
            seq.FirstTimeout,
            seq.TimeoutDurationSelector,
            seq.Other);

    private static IObservable<T> TimeoutTimeImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IScheduler scheduler) =>
        source.Timeout(dueTime, scheduler);

    private static IObservable<T> TimeoutTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime) =>
        source.Timeout(dueTime);

    private static IObservable<T> TimeoutTimeOtherImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IObservable<T> other,
        IScheduler scheduler) =>
        source.Timeout(dueTime, other, scheduler);

    private static IObservable<T> TimeoutTimeOtherDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IObservable<T> other) =>
        source.Timeout(dueTime, other);

    private static IObservable<T> TimeoutAbsoluteImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime,
        IScheduler scheduler) =>
        source.Timeout(dueTime, scheduler);

    private static IObservable<T> TimeoutAbsoluteDefaultImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime) =>
        source.Timeout(dueTime);

    private static IObservable<T> TimeoutAbsoluteOtherImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime,
        IObservable<T> other,
        IScheduler scheduler) =>
        source.Timeout(dueTime, other, scheduler);

    private static IObservable<T> TimeoutAbsoluteOtherDefaultImpl<T>(
        IObservable<T> source,
        DateTimeOffset dueTime,
        IObservable<T> other) =>
        source.Timeout(dueTime, other);

    private static IObservable<T> TimeoutDurationImpl<T, TTimeout>(
        IObservable<T> source,
        Func<T, IObservable<TTimeout>> timeoutDurationSelector) =>
        source.Timeout(timeoutDurationSelector);

    private static IObservable<T> TimeoutDurationOtherImpl<T, TTimeout>(
        IObservable<T> source,
        Func<T, IObservable<TTimeout>> timeoutDurationSelector,
        IObservable<T> other) =>
        source.Timeout(timeoutDurationSelector, other);

    private static IObservable<T> TimeoutFirstDurationImpl<T, TTimeout>(
        IObservable<T> source,
        IObservable<TTimeout> firstTimeout,
        Func<T, IObservable<TTimeout>> timeoutDurationSelector) =>
        source.Timeout(firstTimeout, timeoutDurationSelector);

    private static IObservable<T> TimeoutFirstDurationOtherImpl<T, TTimeout>(
        IObservable<T> source,
        IObservable<TTimeout> firstTimeout,
        Func<T, IObservable<TTimeout>> timeoutDurationSelector,
        IObservable<T> other) =>
        source.Timeout(firstTimeout, timeoutDurationSelector, other);
}
