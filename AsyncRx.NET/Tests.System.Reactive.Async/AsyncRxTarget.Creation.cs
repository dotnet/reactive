// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Return<T>(ReturnSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(ReturnImpl<T>, seq.Value)
            : _bridge.Run<Seq<T>>(ReturnOnImpl<T>, seq.Value, Unwrap(seq.Scheduler));

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
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(EnumerableImpl<T>, seq.Source)
            : _bridge.Run<Seq<T>>(EnumerableScheduledImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.Defer<T>(DeferSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DeferImpl<T>, seq.ObservableFactory);

    Realized<Seq<T>> ISeqVisitor.DeferAsync<T>(DeferAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DeferAsyncImpl<T>, seq.ObservableFactory);

    Realized<Seq<T>> ISeqVisitor.DeferAsyncWithToken<T>(DeferAsyncWithTokenSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DeferAsyncWithTokenImpl<T>, seq.ObservableFactory);

    Realized<Seq<T>> ISeqVisitor.Never<T>(NeverSeq<T> seq) => _bridge.Run<Seq<T>>(NeverImpl<T>);

    Realized<Seq<T>> ISeqVisitor.Empty<T>(EmptySeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(EmptyImpl<T>)
            : _bridge.Run<Seq<T>>(EmptyOnImpl<T>, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.Throw<T>(ThrowSeq<T> seq) =>
        seq.Scheduler is null ? _bridge.Run<Seq<T>>(
            ThrowImpl<T>,
            seq.Error) : _bridge.Run<Seq<T>>(ThrowOnImpl<T>,
            seq.Error,
            Unwrap(seq.Scheduler));

    // Fixes the scheduler argument (see the visitor member above), so not the call's own shape.
    [AdaptsLibraryCall]
    private static IAsyncObservable<T> EnumerableImpl<T>(IEnumerable<T> source) =>
        source.ToAsyncObservable(ImmediateAsyncScheduler.Instance);

    private static IAsyncObservable<T> EnumerableScheduledImpl<T>(
        IEnumerable<T> source,
        IAsyncScheduler scheduler) =>
        source.ToAsyncObservable(scheduler);

    private static IAsyncObservable<T> ReturnImpl<T>(T value) => AsyncObservable.Return(value);

    private static IAsyncObservable<T> ReturnOnImpl<T>(T value, IAsyncScheduler scheduler) =>
        AsyncObservable.Return(value, scheduler);

    private static IAsyncObservable<T> DeferImpl<T>(Func<IAsyncObservable<T>> observableFactory) =>
        AsyncObservable.Defer(observableFactory);

    private static IAsyncObservable<T> DeferAsyncImpl<T>(
        Func<ValueTask<IAsyncObservable<T>>> observableFactory) =>
        AsyncObservable.Defer(observableFactory);

    private static IAsyncObservable<T> DeferAsyncWithTokenImpl<T>(
        Func<CancellationToken, ValueTask<IAsyncObservable<T>>> observableFactory) =>
        AsyncObservable.DeferAsync(observableFactory);

    private static IAsyncObservable<T> NeverImpl<T>() => AsyncObservable.Never<T>();

    private static IAsyncObservable<T> EmptyImpl<T>() => AsyncObservable.Empty<T>();

    private static IAsyncObservable<T> EmptyOnImpl<T>(IAsyncScheduler scheduler) => AsyncObservable.Empty<T>(scheduler);

    private static IAsyncObservable<T> ThrowImpl<T>(Exception error) =>
        AsyncObservable.Throw<T>(error);

    private static IAsyncObservable<T> ThrowOnImpl<T>(Exception error, IAsyncScheduler scheduler) =>
        AsyncObservable.Throw<T>(error, scheduler);

    Realized<Seq<T>> ISeqVisitor.EmptyWitness<T>(EmptyWitnessSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(EmptyWitnessImpl<T>, seq.Witness)
            : _bridge.Run<Seq<T>>(EmptyWitnessOnImpl<T>, Unwrap(seq.Scheduler), seq.Witness);

    Realized<Seq<T>> ISeqVisitor.ThrowWitness<T>(ThrowWitnessSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(ThrowWitnessImpl<T>, seq.Error, seq.Witness)
            : _bridge.Run<Seq<T>>(
                ThrowWitnessOnImpl<T>, seq.Error, Unwrap(seq.Scheduler), seq.Witness);

    Realized<Seq<T>> ISeqVisitor.NeverWitness<T>(NeverWitnessSeq<T> seq) =>
        _bridge.Run<Seq<T>>(NeverWitnessImpl<T>, seq.Witness);

    private static IAsyncObservable<T> EmptyWitnessImpl<T>(T witness) =>
        AsyncObservable.Empty(witness);

    private static IAsyncObservable<T> EmptyWitnessOnImpl<T>(
        IAsyncScheduler scheduler,
        T witness) =>
        AsyncObservable.Empty(scheduler, witness);

    private static IAsyncObservable<T> ThrowWitnessImpl<T>(Exception error, T witness) =>
        AsyncObservable.Throw(error, witness);

    private static IAsyncObservable<T> ThrowWitnessOnImpl<T>(
        Exception error,
        IAsyncScheduler scheduler,
        T witness) =>
        AsyncObservable.Throw(error, scheduler, witness);

    private static IAsyncObservable<T> NeverWitnessImpl<T>(T witness) =>
        AsyncObservable.Never(witness);
}
