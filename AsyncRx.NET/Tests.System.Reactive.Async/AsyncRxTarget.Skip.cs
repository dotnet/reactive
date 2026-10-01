// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Skip<T>(SkipSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipImpl<T>, seq.Source, seq.Count);

    Realized<Seq<T>> ISeqVisitor.SkipTime<T>(SkipTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipTimeImpl<T>, seq.Source, seq.Duration, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SkipDuration<T>(SkipDurationSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipDurationImpl<T>, seq.Source, seq.Duration);

    private static IAsyncObservable<T> SkipImpl<T>(IAsyncObservable<T> source, int count) =>
        source.Skip(count);

    private static IAsyncObservable<T> SkipTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration,
        IAsyncScheduler scheduler) =>
        source.Skip(duration, scheduler);

    private static IAsyncObservable<T> SkipDurationImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan duration) =>
        source.Skip(duration);
}
