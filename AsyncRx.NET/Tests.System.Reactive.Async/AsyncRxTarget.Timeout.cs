// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<T> TimeoutTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime,
        IAsyncScheduler scheduler) =>
        source.Timeout(dueTime, scheduler);

    private static IAsyncObservable<T> TimeoutTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime) =>
        source.Timeout(dueTime);

    private static IAsyncObservable<T> TimeoutTimeOtherImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime,
        IAsyncObservable<T> other,
        IAsyncScheduler scheduler) =>
        source.Timeout(dueTime, other, scheduler);

    private static IAsyncObservable<T> TimeoutTimeOtherDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime,
        IAsyncObservable<T> other) =>
        source.Timeout(dueTime, other);

    private static IAsyncObservable<T> TimeoutAbsoluteImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset dueTime,
        IAsyncScheduler scheduler) =>
        source.Timeout(dueTime, scheduler);

    private static IAsyncObservable<T> TimeoutAbsoluteDefaultImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset dueTime) =>
        source.Timeout(dueTime);

    private static IAsyncObservable<T> TimeoutAbsoluteOtherImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset dueTime,
        IAsyncObservable<T> other,
        IAsyncScheduler scheduler) =>
        source.Timeout(dueTime, other, scheduler);

    private static IAsyncObservable<T> TimeoutAbsoluteOtherDefaultImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset dueTime,
        IAsyncObservable<T> other) =>
        source.Timeout(dueTime, other);

    private static IAsyncObservable<T> TimeoutDurationImpl<T, TTimeout>(
        IAsyncObservable<T> source,
        Func<T, IAsyncObservable<TTimeout>> timeoutDurationSelector) =>
        source.Timeout(timeoutDurationSelector);

    private static IAsyncObservable<T> TimeoutDurationOtherImpl<T, TTimeout>(
        IAsyncObservable<T> source,
        Func<T, IAsyncObservable<TTimeout>> timeoutDurationSelector,
        IAsyncObservable<T> other) =>
        source.Timeout(timeoutDurationSelector, other);

    private static IAsyncObservable<T> TimeoutFirstDurationImpl<T, TTimeout>(
        IAsyncObservable<T> source,
        IAsyncObservable<TTimeout> firstTimeout,
        Func<T, IAsyncObservable<TTimeout>> timeoutDurationSelector) =>
        source.Timeout(firstTimeout, timeoutDurationSelector);

    private static IAsyncObservable<T> TimeoutFirstDurationOtherImpl<T, TTimeout>(
        IAsyncObservable<T> source,
        IAsyncObservable<TTimeout> firstTimeout,
        Func<T, IAsyncObservable<TTimeout>> timeoutDurationSelector,
        IAsyncObservable<T> other) =>
        source.Timeout(firstTimeout, timeoutDurationSelector, other);
}
