// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Return<T>(ReturnSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(ReturnImpl<T>, seq.Value)
            : _bridge.Run<Seq<T>>(ReturnOnImpl<T>, seq.Value, Unwrap(seq.Scheduler));

    Realized<Seq<int>> ISeqVisitor.Range(RangeSeq seq) =>
        Realized.Of<Seq<int>>(Observable.Range(seq.Start, seq.Count));

    Realized<Seq<int>> ISeqVisitor.RangeScheduled(RangeScheduledSeq seq) =>
        Realized.Of<Seq<int>>(Observable.Range(seq.Start, seq.Count, Unwrap(seq.Scheduler)));

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

    private static IObservable<T> EnumerableImpl<T>(IEnumerable<T> source) => source.ToObservable();

    private static IObservable<T> EnumerableScheduledImpl<T>(
        IEnumerable<T> source,
        IScheduler scheduler) =>
        source.ToObservable(scheduler);

    private static IObservable<T> ReturnImpl<T>(T value) => Observable.Return(value);

    private static IObservable<T> ReturnOnImpl<T>(T value, IScheduler scheduler) =>
        Observable.Return(value, scheduler);

    private static IObservable<T> DeferImpl<T>(Func<IObservable<T>> observableFactory) =>
        Observable.Defer(observableFactory);

    // The asynchronous factories' ValueTasks handed over as Tasks, as Create's are.
    [AdaptsLibraryCall]
    private static IObservable<T> DeferAsyncImpl<T>(
        Func<ValueTask<IObservable<T>>> observableFactory) =>
        Observable.Defer(() => observableFactory().AsTask());

    [AdaptsLibraryCall]
    private static IObservable<T> DeferAsyncWithTokenImpl<T>(
        Func<CancellationToken, ValueTask<IObservable<T>>> observableFactory) =>
        Observable.DeferAsync(token => observableFactory(token).AsTask());

    private static IObservable<T> NeverImpl<T>() => Observable.Never<T>();

    private static IObservable<T> EmptyImpl<T>() => Observable.Empty<T>();

    private static IObservable<T> EmptyOnImpl<T>(IScheduler scheduler) =>
        Observable.Empty<T>(scheduler);

    private static IObservable<T> ThrowImpl<T>(Exception error) => Observable.Throw<T>(error);

    private static IObservable<T> ThrowOnImpl<T>(Exception error, IScheduler scheduler) =>
        Observable.Throw<T>(error, scheduler);

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

    private static IObservable<T> EmptyWitnessImpl<T>(T witness) =>
        Observable.Empty(witness);

    private static IObservable<T> EmptyWitnessOnImpl<T>(
        IScheduler scheduler,
        T witness) =>
        Observable.Empty(scheduler, witness);

    private static IObservable<T> ThrowWitnessImpl<T>(Exception error, T witness) =>
        Observable.Throw(error, witness);

    private static IObservable<T> ThrowWitnessOnImpl<T>(
        Exception error,
        IScheduler scheduler,
        T witness) =>
        Observable.Throw(error, scheduler, witness);

    private static IObservable<T> NeverWitnessImpl<T>(T witness) =>
        Observable.Never(witness);
}
