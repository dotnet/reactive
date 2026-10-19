// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<long>> ISeqVisitor.Interval(IntervalSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(IntervalImpl, seq.Period)
            : _bridge.Run<Seq<long>>(IntervalOnImpl, seq.Period, Unwrap(seq.Scheduler));

    private static IAsyncObservable<long> IntervalImpl(TimeSpan period) =>
        AsyncObservable.Interval(period);

    private static IAsyncObservable<long> IntervalOnImpl(
        TimeSpan period,
        IAsyncScheduler scheduler) =>
        AsyncObservable.Interval(period, scheduler);
}
