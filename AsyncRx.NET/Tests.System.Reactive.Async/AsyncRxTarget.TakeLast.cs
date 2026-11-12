// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.TakeLastCount<T>(TakeLastCountSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(TakeLastCountDefaultImpl<T>, seq.Source, seq.Count)
            : _bridge.Run<Seq<T>>(
                TakeLastCountImpl<T>,
                seq.Source,
                seq.Count,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TakeLastTime<T>(TakeLastTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(TakeLastTimeDefaultImpl<T>, seq.Source, seq.Duration)
            : _bridge.Run<Seq<T>>(
                TakeLastTimeImpl<T>,
                seq.Source,
                seq.Duration,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TakeLastTimeLoop<T>(TakeLastTimeLoopSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            TakeLastTimeLoopImpl<T>,
            seq.Source,
            seq.Duration,
            Unwrap(seq.TimerScheduler),
            Unwrap(seq.LoopScheduler));

    private static IAsyncObservable<T> TakeLastCountDefaultImpl<T>(
        IAsyncObservable<T> source,
        int count) =>
        source.TakeLast(count);

    private static IAsyncObservable<T> TakeLastCountImpl<T>(
        IAsyncObservable<T> source,
        int count,
        IAsyncScheduler scheduler) =>
        source.TakeLast(count, scheduler);

    private static IAsyncObservable<T> TakeLastTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration) =>
        source.TakeLast(duration);

    private static IAsyncObservable<T> TakeLastTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler scheduler) =>
        source.TakeLast(duration, scheduler);

    // AsyncRx.NET takes the timer scheduler as the clock (every scheduler is one).
    private static IAsyncObservable<T> TakeLastTimeLoopImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler timerScheduler,
        IAsyncScheduler loopScheduler) =>
        source.TakeLast(duration, timerScheduler, loopScheduler);
}
