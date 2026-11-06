// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.StartWith<T>(StartWithSeq<T> seq) =>
        _bridge.Run<Seq<T>>(StartWithImpl<T>, seq.Source, seq.Values);

    private static IObservable<T> StartWithImpl<T>(
        IObservable<T> source,
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

    private static IObservable<T> StartWithEnumerableImpl<T>(
        IObservable<T> source,
        IEnumerable<T> values) =>
        source.StartWith(values);

    private static IObservable<T> StartWithSchedulerImpl<T>(
        IObservable<T> source,
        IScheduler scheduler,
        T[] values) =>
        source.StartWith(scheduler, values);

    private static IObservable<T> StartWithSchedulerEnumerableImpl<T>(
        IObservable<T> source,
        IScheduler scheduler,
        IEnumerable<T> values) =>
        source.StartWith(scheduler, values);
}
