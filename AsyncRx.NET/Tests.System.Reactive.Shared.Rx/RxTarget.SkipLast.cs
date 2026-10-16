// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<T> SkipLastCountImpl<T>(IObservable<T> source, int count) =>
        source.SkipLast(count);

    private static IObservable<T> SkipLastTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan duration) =>
        source.SkipLast(duration);

    private static IObservable<T> SkipLastTimeImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler scheduler) =>
        source.SkipLast(duration, scheduler);
}
