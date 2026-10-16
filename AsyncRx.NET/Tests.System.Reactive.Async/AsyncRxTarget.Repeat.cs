// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<T> RepeatImpl<T>(IAsyncObservable<T> source) => source.Repeat();

    private static IAsyncObservable<T> RepeatCountImpl<T>(
        IAsyncObservable<T> source,
        int repeatCount) =>
        source.Repeat(repeatCount);

    private static IAsyncObservable<T> RepeatValueImpl<T>(T value) => AsyncObservable.Repeat(value);

    private static IAsyncObservable<T> RepeatValueOnImpl<T>(T value, IAsyncScheduler scheduler) =>
        AsyncObservable.Repeat(value, scheduler);

    private static IAsyncObservable<T> RepeatValueCountImpl<T>(T value, int repeatCount) =>
        AsyncObservable.Repeat(value, repeatCount);

    private static IAsyncObservable<T> RepeatValueCountOnImpl<T>(
        T value,
        int repeatCount,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Repeat(value, repeatCount, scheduler);
}
