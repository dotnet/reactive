// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.SkipLastCount<T>(SkipLastCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipLastCountImpl<T>, seq.Source, seq.Count);

    Realized<Seq<T>> ISeqVisitor.SkipLastTime<T>(SkipLastTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<T>>(SkipLastTimeDefaultImpl<T>, seq.Source, seq.Duration)
            : _bridge.Run<Seq<T>>(
                SkipLastTimeImpl<T>,
                seq.Source,
                seq.Duration,
                Unwrap(seq.Scheduler));

    private static IAsyncObservable<T> SkipLastCountImpl<T>(
        IAsyncObservable<T> source,
        int count) =>
        source.SkipLast(count);

    private static IAsyncObservable<T> SkipLastTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration) =>
        source.SkipLast(duration);

    // AsyncRx.NET takes the scheduler as the clock (every scheduler is one).
    private static IAsyncObservable<T> SkipLastTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler scheduler) =>
        source.SkipLast(duration, scheduler);
}
