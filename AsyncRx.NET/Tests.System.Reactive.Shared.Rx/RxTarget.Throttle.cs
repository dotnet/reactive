// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.ThrottleTime<T>(ThrottleTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(ThrottleTimeDefaultImpl<T>, seq.Source, seq.DueTime)
            : _bridge.Run<Seq<T>>(
                ThrottleTimeImpl<T>, seq.Source, seq.DueTime, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.ThrottleSelector<T, TThrottle>(
        ThrottleSelectorSeq<T, TThrottle> seq) =>
        _bridge.Run<Seq<T>>(
            ThrottleSelectorImpl<T, TThrottle>, seq.Source, seq.ThrottleDurationSelector);

    private static IObservable<T> ThrottleTimeImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime,
        IScheduler scheduler) =>
        source.Throttle(dueTime, scheduler);

    private static IObservable<T> ThrottleTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan dueTime) =>
        source.Throttle(dueTime);

    private static IObservable<T> ThrottleSelectorImpl<T, TThrottle>(
        IObservable<T> source,
        Func<T, IObservable<TThrottle>> throttleDurationSelector) =>
        source.Throttle(throttleDurationSelector);
}
