// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Take<T>(TakeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeImpl<T>, seq.Source, seq.Count);

    Realized<Seq<T>> ISeqVisitor.TakeScheduled<T>(TakeScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeScheduledImpl<T>, seq.Source, seq.Count, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TakeTime<T>(TakeTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeTimeImpl<T>, seq.Source, seq.Duration, Unwrap(seq.Scheduler));

    private static IAsyncObservable<T> TakeImpl<T>(IAsyncObservable<T> source, int count) =>
        source.Take(count);

    private static IAsyncObservable<T> TakeScheduledImpl<T>(
        IAsyncObservable<T> source,
        int count,
        IAsyncScheduler scheduler) =>
        source.Take(count, scheduler);

    private static IAsyncObservable<T> TakeTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler scheduler) =>
        source.Take(duration, scheduler);
}
