// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.StartWith<T>(StartWithSeq<T> seq) =>
        _bridge.Run<Seq<T>>(StartWithImpl<T>, seq.Source, seq.Values);

    private static IAsyncObservable<T> StartWithImpl<T>(
        IAsyncObservable<T> source,
        T[] values) =>
        source.StartWith(values);

    Realized<Seq<T>> ISeqVisitor.StartWithEnumerable<T>(StartWithEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(StartWithEnumerableImpl<T>, seq.Source, seq.Values);

    Realized<Seq<T>> ISeqVisitor.StartWithScheduler<T>(StartWithSchedulerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            StartWithSchedulerImpl<T>,
            seq.Source,
            Unwrap(seq.Scheduler),
            seq.Values);

    Realized<Seq<T>> ISeqVisitor.StartWithSchedulerEnumerable<T>(
        StartWithSchedulerEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            StartWithSchedulerEnumerableImpl<T>,
            seq.Source,
            Unwrap(seq.Scheduler),
            seq.Values);

    private static IAsyncObservable<T> StartWithEnumerableImpl<T>(
        IAsyncObservable<T> source,
        IEnumerable<T> values) =>
        source.StartWith(values);

    private static IAsyncObservable<T> StartWithSchedulerImpl<T>(
        IAsyncObservable<T> source,
        IAsyncScheduler scheduler,
        T[] values) =>
        source.StartWith(scheduler, values);

    private static IAsyncObservable<T> StartWithSchedulerEnumerableImpl<T>(
        IAsyncObservable<T> source,
        IAsyncScheduler scheduler,
        IEnumerable<T> values) =>
        source.StartWith(scheduler, values);
}
