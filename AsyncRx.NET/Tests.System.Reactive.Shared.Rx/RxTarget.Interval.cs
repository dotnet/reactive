// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<long>> ISeqVisitor.Interval(IntervalSeq seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<long>>(IntervalImpl, seq.Period)
            : _bridge.Run<Seq<long>>(IntervalOnImpl, seq.Period, Unwrap(seq.Scheduler));

    private static IObservable<long> IntervalImpl(TimeSpan period) => Observable.Interval(period);

    private static IObservable<long> IntervalOnImpl(TimeSpan period, IScheduler scheduler) =>
        Observable.Interval(period, scheduler);
}
