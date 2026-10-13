// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.TakeUntil<T, TOther>(TakeUntilSeq<T, TOther> seq) =>
        _bridge.Run<Seq<T>>(TakeUntilImpl<T, TOther>, seq.Source, seq.Other);

    Realized<Seq<T>> ISeqVisitor.TakeUntilTime<T>(TakeUntilTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(TakeUntilTimeDefaultImpl<T>, seq.Source, seq.EndTime)
            : _bridge.Run<Seq<T>>(
                TakeUntilTimeImpl<T>,
                seq.Source,
                seq.EndTime,
                Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.TakeUntilPredicate<T>(TakeUntilPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeUntilPredicateImpl<T>, seq.Source, seq.StopPredicate);

    Realized<Seq<T>> ISeqVisitor.TakeUntilToken<T>(TakeUntilTokenSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeUntilTokenImpl<T>, seq.Source, seq.CancellationToken);

    private static IAsyncObservable<T> TakeUntilImpl<T, TOther>(
        IAsyncObservable<T> source,
        IAsyncObservable<TOther> other) =>
        source.TakeUntil(other);

    private static IAsyncObservable<T> TakeUntilTimeImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset endTime,
        IAsyncScheduler scheduler) =>
        source.TakeUntil(endTime, scheduler);

    private static IAsyncObservable<T> TakeUntilTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        DateTimeOffset endTime) =>
        source.TakeUntil(endTime);

    private static IAsyncObservable<T> TakeUntilPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> stopPredicate) =>
        source.TakeUntil(stopPredicate);

    private static IAsyncObservable<T> TakeUntilTokenImpl<T>(
        IAsyncObservable<T> source,
        CancellationToken cancellationToken) =>
        source.TakeUntil(cancellationToken);
}
