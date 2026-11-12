// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TimeInterval<T>>> ISeqVisitor.TimeInterval<T>(TimeIntervalSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<TimeInterval<T>>>(TimeIntervalImpl<T>, seq.Source)
            : _bridge.Run<Seq<TimeInterval<T>>>(
                TimeIntervalOnImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    private static IAsyncObservable<TimeInterval<T>> TimeIntervalImpl<T>(
        IAsyncObservable<T> source) =>
        source.TimeInterval();

    private static IAsyncObservable<TimeInterval<T>> TimeIntervalOnImpl<T>(
        IAsyncObservable<T> source,
        IClock clock) =>
        source.TimeInterval(clock);
}
