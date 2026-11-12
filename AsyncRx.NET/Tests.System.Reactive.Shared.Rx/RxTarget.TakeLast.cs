// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<T> TakeLastCountDefaultImpl<T>(IObservable<T> source, int count) =>
        source.TakeLast(count);

    private static IObservable<T> TakeLastCountImpl<T>(
        IObservable<T> source,
        int count,
        IScheduler scheduler) =>
        source.TakeLast(count, scheduler);

    private static IObservable<T> TakeLastTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan duration) =>
        source.TakeLast(duration);

    private static IObservable<T> TakeLastTimeImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler scheduler) =>
        source.TakeLast(duration, scheduler);

    private static IObservable<T> TakeLastTimeLoopImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler timerScheduler,
        IScheduler loopScheduler) =>
        source.TakeLast(duration, timerScheduler, loopScheduler);
}
