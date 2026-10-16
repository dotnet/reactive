// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<IList<T>>> ISeqVisitor.BufferClosings<T, TBufferClosing>(
        BufferClosingsSeq<T, TBufferClosing> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferClosingsImpl<T, TBufferClosing>,
            seq.Source,
            seq.BufferClosingSelector);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferOpenings<T, TBufferOpening, TBufferClosing>(
        BufferOpeningsSeq<T, TBufferOpening, TBufferClosing> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferOpeningsImpl<T, TBufferOpening, TBufferClosing>,
            seq.Source,
            seq.BufferOpenings,
            seq.BufferClosingSelector);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferBoundaries<T, TBufferBoundary>(
        BufferBoundariesSeq<T, TBufferBoundary> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferBoundariesImpl<T, TBufferBoundary>,
            seq.Source,
            seq.BufferBoundaries);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferCount<T>(BufferCountSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(BufferCountImpl<T>, seq.Source, seq.Count, seq.Skip);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTime<T>(BufferTimeSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferTimeImpl<T>,
            seq.Source,
            seq.TimeSpan,
            Unwrap(seq.Scheduler));

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTimeShift<T>(BufferTimeShiftSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferTimeShiftImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.TimeShift,
            Unwrap(seq.Scheduler));

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTimeOrCount<T>(BufferTimeOrCountSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferTimeOrCountImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.Count,
            Unwrap(seq.Scheduler));

    private static IAsyncObservable<IList<T>> BufferClosingsImpl<T, TBufferClosing>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<TBufferClosing>> bufferClosingSelector) =>
        source.Buffer(bufferClosingSelector);

    private static IAsyncObservable<IList<T>> BufferOpeningsImpl<T, TBufferOpening, TBufferClosing>(
        IAsyncObservable<T> source,
        IAsyncObservable<TBufferOpening> bufferOpenings,
        Func<TBufferOpening, IAsyncObservable<TBufferClosing>> bufferClosingSelector) =>
        source.Buffer(bufferOpenings, bufferClosingSelector);

    private static IAsyncObservable<IList<T>> BufferBoundariesImpl<T, TBufferBoundary>(
        IAsyncObservable<T> source,
        IAsyncObservable<TBufferBoundary> bufferBoundaries) =>
        source.Buffer(bufferBoundaries);

    private static IAsyncObservable<IList<T>> BufferCountImpl<T>(
        IAsyncObservable<T> source,
        int count,
        int skip) =>
        source.Buffer(count, skip);

    private static IAsyncObservable<IList<T>> BufferTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        IAsyncScheduler scheduler) =>
        source.Buffer(timeSpan, scheduler);

    private static IAsyncObservable<IList<T>> BufferTimeShiftImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift,
        IAsyncScheduler scheduler) =>
        source.Buffer(timeSpan, timeShift, scheduler);

    private static IAsyncObservable<IList<T>> BufferTimeOrCountImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        int count,
        IAsyncScheduler scheduler) =>
        source.Buffer(timeSpan, count, scheduler);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferCountOnly<T>(BufferCountOnlySeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(BufferCountOnlyImpl<T>, seq.Source, seq.Count);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTimeDefault<T>(BufferTimeDefaultSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(BufferTimeDefaultImpl<T>, seq.Source, seq.TimeSpan);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTimeShiftDefault<T>(
        BufferTimeShiftDefaultSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferTimeShiftDefaultImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.TimeShift);

    Realized<Seq<IList<T>>> ISeqVisitor.BufferTimeOrCountDefault<T>(
        BufferTimeOrCountDefaultSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(
            BufferTimeOrCountDefaultImpl<T>,
            seq.Source,
            seq.TimeSpan,
            seq.Count);

    private static IAsyncObservable<IList<T>> BufferCountOnlyImpl<T>(
        IAsyncObservable<T> source,
        int count) =>
        source.Buffer(count);

    private static IAsyncObservable<IList<T>> BufferTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan) =>
        source.Buffer(timeSpan);

    private static IAsyncObservable<IList<T>> BufferTimeShiftDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift) =>
        source.Buffer(timeSpan, timeShift);

    private static IAsyncObservable<IList<T>> BufferTimeOrCountDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan timeSpan,
        int count) =>
        source.Buffer(timeSpan, count);
}
