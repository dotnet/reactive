// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Skip<T>(SkipSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipImpl<T>, seq.Source, seq.Count);

    Realized<Seq<T>> ISeqVisitor.SkipTime<T>(SkipTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipTimeImpl<T>, seq.Source, seq.Duration, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.SkipTimeDefault<T>(SkipTimeDefaultSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipTimeDefaultImpl<T>, seq.Source, seq.Duration);

    private static IObservable<T> SkipImpl<T>(IObservable<T> source, int count) =>
        source.Skip(count);

    private static IObservable<T> SkipTimeImpl<T>(
        IObservable<T> source,
        TimeSpan duration,
        IScheduler scheduler) =>
        source.Skip(duration, scheduler);

    private static IObservable<T> SkipTimeDefaultImpl<T>(IObservable<T> source, TimeSpan duration) =>
        source.Skip(duration);
}
