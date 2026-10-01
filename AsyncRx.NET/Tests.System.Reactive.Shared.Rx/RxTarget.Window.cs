// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<IObservable<T>> WindowClosingsImpl<T, TWindowClosing>(
        IObservable<T> source,
        Func<IObservable<TWindowClosing>> windowClosingSelector) =>
        source.Window(windowClosingSelector);

    private static IObservable<IObservable<T>> WindowOpeningsImpl<T, TWindowOpening, TWindowClosing>(
        IObservable<T> source,
        IObservable<TWindowOpening> windowOpenings,
        Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector) =>
        source.Window(windowOpenings, windowClosingSelector);

    private static IObservable<IObservable<T>> WindowBoundariesImpl<T, TWindowBoundary>(
        IObservable<T> source,
        IObservable<TWindowBoundary> windowBoundaries) =>
        source.Window(windowBoundaries);

    private static IObservable<IObservable<T>> WindowCountImpl<T>(
        IObservable<T> source,
        int count,
        int skip) =>
        source.Window(count, skip);

    private static IObservable<IObservable<T>> WindowTimeImpl<T>(
        IObservable<T> source,
        TimeSpan timeSpan,
        IScheduler scheduler) =>
        source.Window(timeSpan, scheduler);

    private static IObservable<IObservable<T>> WindowTimeShiftImpl<T>(
        IObservable<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift,
        IScheduler scheduler) =>
        source.Window(timeSpan, timeShift, scheduler);

    private static IObservable<IObservable<T>> WindowTimeOrCountImpl<T>(
        IObservable<T> source,
        TimeSpan timeSpan,
        int count,
        IScheduler scheduler) =>
        source.Window(timeSpan, count, scheduler);
}
