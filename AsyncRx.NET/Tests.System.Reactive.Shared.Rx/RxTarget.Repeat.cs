// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Repeat<T>(RepeatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RepeatCount<T>(RepeatCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatCountImpl<T>, seq.Source, seq.RepeatCount);

    Realized<Seq<T>> ISeqVisitor.RepeatValue<T>(RepeatValueSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(RepeatValueImpl<T>, seq.Value)
            : _bridge.Run<Seq<T>>(RepeatValueOnImpl<T>, seq.Value, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.RepeatValueCount<T>(RepeatValueCountSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(RepeatValueCountImpl<T>, seq.Value, seq.RepeatCount)
            : _bridge.Run<Seq<T>>(
                RepeatValueCountOnImpl<T>,
                seq.Value,
                seq.RepeatCount,
                Unwrap(seq.Scheduler));

    private static IObservable<T> RepeatImpl<T>(IObservable<T> source) => source.Repeat();

    private static IObservable<T> RepeatCountImpl<T>(IObservable<T> source, int repeatCount) =>
        source.Repeat(repeatCount);

    private static IObservable<T> RepeatValueImpl<T>(T value) => Observable.Repeat(value);

    private static IObservable<T> RepeatValueOnImpl<T>(T value, IScheduler scheduler) =>
        Observable.Repeat(value, scheduler);

    private static IObservable<T> RepeatValueCountImpl<T>(T value, int repeatCount) =>
        Observable.Repeat(value, repeatCount);

    private static IObservable<T> RepeatValueCountOnImpl<T>(
        T value,
        int repeatCount,
        IScheduler scheduler) =>
        Observable.Repeat(value, repeatCount, scheduler);
}
