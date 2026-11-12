// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<T> SkipUntilImpl<T, TOther>(
        IAsyncObservable<T> source,
        IAsyncObservable<TOther> other) =>
        source.SkipUntil(other);

    private static IAsyncObservable<T> SkipUntilTimeImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset startTime,
        IAsyncScheduler scheduler) =>
        source.SkipUntil(startTime, scheduler);

    private static IAsyncObservable<T> SkipUntilTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset startTime) =>
        source.SkipUntil(startTime);
}
