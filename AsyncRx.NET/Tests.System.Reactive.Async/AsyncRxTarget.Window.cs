// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<Seq<T>>> ISeqVisitor.WindowClosings<T, TWindowClosing>(
        WindowClosingsSeq<T, TWindowClosing> seq) =>
        _bridge.Run<Seq<Seq<T>>>(
            WindowClosingsImpl<T, TWindowClosing>,
            seq.Source,
            seq.WindowClosingSelector);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowOpenings<T, TWindowOpening, TWindowClosing>(
        WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq) =>
        _bridge.Run<Seq<Seq<T>>>(
            WindowOpeningsImpl<T, TWindowOpening, TWindowClosing>,
            seq.Source,
            seq.WindowOpenings,
            seq.WindowClosingSelector);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowBoundaries<T, TWindowBoundary>(
        WindowBoundariesSeq<T, TWindowBoundary> seq) =>
        _bridge.Run<Seq<Seq<T>>>(
            WindowBoundariesImpl<T, TWindowBoundary>,
            seq.Source,
            seq.WindowBoundaries);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowCount<T>(WindowCountSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowCountImpl<T>, seq.Source, seq.Count, seq.Skip);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTime<T>(WindowTimeSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowTimeImpl<T>, seq.Source, seq.TimeSpan, Unwrap(seq.Scheduler));

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTimeShift<T>(WindowTimeShiftSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(
            WindowTimeShiftImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.TimeShift,
            Unwrap(seq.Scheduler));

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(
            WindowTimeOrCountImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.Count,
            Unwrap(seq.Scheduler));

    private static IAsyncObservable<IAsyncObservable<T>> WindowClosingsImpl<T, TWindowClosing>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<TWindowClosing>> windowClosingSelector) =>
        source.Window(windowClosingSelector);

    private static IAsyncObservable<IAsyncObservable<T>> WindowOpeningsImpl<T, TWindowOpening, TWindowClosing>(
        IAsyncObservable<T> source,
        IAsyncObservable<TWindowOpening> windowOpenings,
        Func<TWindowOpening, IAsyncObservable<TWindowClosing>> windowClosingSelector) =>
        source.Window(windowOpenings, windowClosingSelector);

    private static IAsyncObservable<IAsyncObservable<T>> WindowBoundariesImpl<T, TWindowBoundary>(
        IAsyncObservable<T> source,
        IAsyncObservable<TWindowBoundary> windowBoundaries) =>
        source.Window(windowBoundaries);

    private static IAsyncObservable<IAsyncObservable<T>> WindowCountImpl<T>(
        IAsyncObservable<T> source,
        int count,
        int skip) =>
        source.Window(count, skip);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        IAsyncScheduler scheduler) =>
        source.Window(timeSpan, scheduler);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeShiftImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift,
        IAsyncScheduler scheduler) =>
        source.Window(timeSpan, timeShift, scheduler);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeOrCountImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        int count,
        IAsyncScheduler scheduler) =>
        source.Window(timeSpan, count, scheduler);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowCountOnly<T>(WindowCountOnlySeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowCountOnlyImpl<T>, seq.Source, seq.Count);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTimeDefault<T>(WindowTimeDefaultSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowTimeDefaultImpl<T>, seq.Source, seq.TimeSpan);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTimeShiftDefault<T>(WindowTimeShiftDefaultSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowTimeShiftDefaultImpl<T>, seq.Source, seq.TimeSpan, seq.TimeShift);

    Realized<Seq<Seq<T>>> ISeqVisitor.WindowTimeOrCountDefault<T>(WindowTimeOrCountDefaultSeq<T> seq) =>
        _bridge.Run<Seq<Seq<T>>>(WindowTimeOrCountDefaultImpl<T>, seq.Source, seq.TimeSpan, seq.Count);

    private static IAsyncObservable<IAsyncObservable<T>> WindowCountOnlyImpl<T>(IAsyncObservable<T> source, int count) =>
        source.Window(count);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeDefaultImpl<T>(IAsyncObservable<T> source, TimeSpan timeSpan) =>
        source.Window(timeSpan);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeShiftDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift) =>
        source.Window(timeSpan, timeShift);

    private static IAsyncObservable<IAsyncObservable<T>> WindowTimeOrCountDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        int count) =>
        source.Window(timeSpan, count);
}
