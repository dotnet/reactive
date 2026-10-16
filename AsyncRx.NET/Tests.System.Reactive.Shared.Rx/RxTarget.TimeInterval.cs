// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TimeInterval<T>>> ISeqVisitor.TimeInterval<T>(TimeIntervalSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TimeInterval<T>>>(TimeIntervalImpl<T>, seq.Source)
            : _bridge.Run<Seq<TimeInterval<T>>>(
                TimeIntervalOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    private static IObservable<TimeInterval<T>> TimeIntervalImpl<T>(
        IObservable<T> source) =>
        source.TimeInterval();

    private static IObservable<TimeInterval<T>> TimeIntervalOnImpl<T>(
        IObservable<T> source,
        IScheduler scheduler) =>
        source.TimeInterval(scheduler);
}
