// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<IList<T>>> ISeqVisitor.TakeLastBufferCount<T>(TakeLastBufferCountSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(TakeLastBufferCountImpl<T>, seq.Source, seq.Count);

    Realized<Seq<IList<T>>> ISeqVisitor.TakeLastBufferTime<T>(TakeLastBufferTimeSeq<T> seq) =>
        seq.Scheduler is null
            ? _bridge.Run<Seq<IList<T>>>(TakeLastBufferTimeDefaultImpl<T>, seq.Source, seq.Duration)
            : _bridge.Run<Seq<IList<T>>>(
                TakeLastBufferTimeImpl<T>,
                seq.Source,
                seq.Duration,
                Unwrap(seq.Scheduler));

    private static IObservable<IList<T>> TakeLastBufferCountImpl<T>(
        IObservable<T> source,
        int count) =>
        source.TakeLastBuffer(count);

    private static IObservable<IList<T>> TakeLastBufferTimeDefaultImpl<T>(
        IObservable<T> source,
        TimeSpan duration) =>
        source.TakeLastBuffer(duration);

    private static IObservable<IList<T>> TakeLastBufferTimeImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler scheduler) =>
        source.TakeLastBuffer(duration, scheduler);
}
