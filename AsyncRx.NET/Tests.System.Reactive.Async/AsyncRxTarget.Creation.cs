// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<long>> ISeqVisitor.Timer(TimerSeq seq) =>
        Realized.Of<Seq<long>>(AsyncObservable.Timer(seq.DueTime, Unwrap(seq.Scheduler)));

    Realized<Seq<long>> ISeqVisitor.Interval(IntervalSeq seq) =>
        Realized.Of<Seq<long>>(AsyncObservable.Interval(seq.Period, Unwrap(seq.Scheduler)));

    Realized<Seq<T>> ISeqVisitor.Return<T>(ReturnSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReturnImpl<T>, seq.Value);

    // Rx.NET's Range(start, count) runs on the current-thread scheduler; the immediate scheduler
    // is the equivalent here (a plumbing decision).
    Realized<Seq<int>> ISeqVisitor.Range(RangeSeq seq) =>
        Realized.Of<Seq<int>>(
            AsyncObservable.Range(seq.Start, seq.Count, ImmediateAsyncScheduler.Instance));

    Realized<Seq<int>> ISeqVisitor.RangeScheduled(RangeScheduledSeq seq) =>
        Realized.Of<Seq<int>>(AsyncObservable.Range(seq.Start, seq.Count, Unwrap(seq.Scheduler)));

    // Rx.NET's ToObservable() runs on the current-thread scheduler; the immediate scheduler is
    // the equivalent here (a plumbing decision, as for Range).
    Realized<Seq<T>> ISeqVisitor.Enumerable<T>(EnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(EnumerableImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.Defer<T>(DeferSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DeferImpl<T>, seq.ObservableFactory);

    Realized<Seq<T>> ISeqVisitor.Never<T>(NeverSeq<T> seq) => _bridge.Run<Seq<T>>(NeverImpl<T>);

    Realized<Seq<T>> ISeqVisitor.Empty<T>(EmptySeq<T> seq) => _bridge.Run<Seq<T>>(EmptyImpl<T>);

    Realized<Seq<T>> ISeqVisitor.Throw<T>(ThrowSeq<T> seq) =>
        seq.Scheduler is null ? _bridge.Run<Seq<T>>(
            ThrowImpl<T>,
            seq.Error) : _bridge.Run<Seq<T>>(ThrowOnImpl<T>,
            seq.Error,
            Unwrap(seq.Scheduler));

    private static IAsyncObservable<T> EnumerableImpl<T>(IEnumerable<T> source) =>
        source.ToAsyncObservable(ImmediateAsyncScheduler.Instance);

    private static IAsyncObservable<T> ReturnImpl<T>(T value) => AsyncObservable.Return(value);

    private static IAsyncObservable<T> DeferImpl<T>(Func<IAsyncObservable<T>> observableFactory) =>
        AsyncObservable.Defer(observableFactory);

    private static IAsyncObservable<T> NeverImpl<T>() => AsyncObservable.Never<T>();

    private static IAsyncObservable<T> EmptyImpl<T>() => AsyncObservable.Empty<T>();

    private static IAsyncObservable<T> ThrowImpl<T>(Exception error) =>
        AsyncObservable.Throw<T>(error);

    private static IAsyncObservable<T> ThrowOnImpl<T>(Exception error, IAsyncScheduler scheduler) =>
        AsyncObservable.Throw<T>(error, scheduler);
}
