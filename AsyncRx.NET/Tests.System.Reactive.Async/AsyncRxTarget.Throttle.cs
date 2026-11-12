// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<T> ThrottleTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime,
        IAsyncScheduler scheduler) =>
        source.Throttle(dueTime, scheduler);

    private static IAsyncObservable<T> ThrottleTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan dueTime) =>
        source.Throttle(dueTime);

    private static IAsyncObservable<T> ThrottleSelectorImpl<T, TThrottle>(
        IAsyncObservable<T> source,
        Func<T, IAsyncObservable<TThrottle>> throttleDurationSelector) =>
        source.Throttle(throttleDurationSelector);
}
