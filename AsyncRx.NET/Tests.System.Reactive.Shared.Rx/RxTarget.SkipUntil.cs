// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SkipUntil<T, TOther>(SkipUntilSeq<T, TOther> seq) =>
        _bridge.Run<Seq<T>>(SkipUntilImpl<T, TOther>, seq.Source, seq.Other);

    Realized<Seq<T>> ISeqVisitor.SkipUntilTime<T>(SkipUntilTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(SkipUntilTimeDefaultImpl<T>, seq.Source, seq.StartTime)
            : _bridge.Run<Seq<T>>(
                SkipUntilTimeImpl<T>,
                seq.Source,
                seq.StartTime,
                Unwrap(seq.Scheduler));

    private static IObservable<T> SkipUntilImpl<T, TOther>(
        IObservable<T> source,
        IObservable<TOther> other) =>
        source.SkipUntil(other);

    private static IObservable<T> SkipUntilTimeImpl<T>(
        IObservable<T> source,
        DateTimeOffset startTime,
        IScheduler scheduler) =>
        source.SkipUntil(startTime, scheduler);

    private static IObservable<T> SkipUntilTimeDefaultImpl<T>(
        IObservable<T> source,
        DateTimeOffset startTime) =>
        source.SkipUntil(startTime);
}
