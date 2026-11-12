// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<IList<T>> TakeLastBufferCountImpl<T>(
        IAsyncObservable<T> source,
        int count) =>
        source.TakeLastBuffer(count);

    private static IAsyncObservable<IList<T>> TakeLastBufferTimeDefaultImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration) =>
        source.TakeLastBuffer(duration);

    // AsyncRx.NET takes the scheduler as the clock (every scheduler is one).
    private static IAsyncObservable<IList<T>> TakeLastBufferTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler scheduler) =>
        source.TakeLastBuffer(duration, scheduler);
}
