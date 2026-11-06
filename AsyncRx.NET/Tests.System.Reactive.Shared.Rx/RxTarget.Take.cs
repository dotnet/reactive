// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Take<T>(TakeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeImpl<T>, seq.Source, seq.Count);

    Realized<Seq<T>> ISeqVisitor.TakeScheduled<T>(TakeScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeScheduledImpl<T>, seq.Source, seq.Count, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TakeTime<T>(TakeTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeTimeImpl<T>, seq.Source, seq.Duration, Unwrap(seq.Scheduler));

    private static IObservable<T> TakeImpl<T>(IObservable<T> source, int count) =>
        source.Take(count);

    private static IObservable<T> TakeScheduledImpl<T>(
        IObservable<T> source,
        int count,
        IScheduler scheduler) =>
        source.Take(count, scheduler);

    private static IObservable<T> TakeTimeImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler scheduler) =>
        source.Take(duration, scheduler);

    Realized<Seq<T>> ISeqVisitor.TakeTimeDefault<T>(TakeTimeDefaultSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeTimeDefaultImpl<T>, seq.Source, seq.Duration);

    private static IObservable<T> TakeTimeDefaultImpl<T>(IObservable<T> source, TimeSpan duration) =>
        source.Take(duration);
}
