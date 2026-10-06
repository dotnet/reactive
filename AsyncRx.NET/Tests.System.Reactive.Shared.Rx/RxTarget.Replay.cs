// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Replay<T>(ReplaySeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReplayImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.ReplayScheduled<T>(ReplayScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReplayScheduledImpl<T>, seq.Source, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.ReplayCount<T>(ReplayCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReplayCountImpl<T>, seq.Source, seq.BufferSize);

    Realized<Seq<T>> ISeqVisitor.ReplayCountScheduled<T>(
        ReplayCountScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            ReplayCountScheduledImpl<T>,
            seq.Source,
            seq.BufferSize,
            Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.ReplayTime<T>(ReplayTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReplayTimeImpl<T>, seq.Source, seq.Window);

    Realized<Seq<T>> ISeqVisitor.ReplayTimeScheduled<T>(
        ReplayTimeScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            ReplayTimeScheduledImpl<T>,
            seq.Source,
            seq.Window,
            Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.ReplayCountTime<T>(ReplayCountTimeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(ReplayCountTimeImpl<T>, seq.Source, seq.BufferSize, seq.Window);

    Realized<Seq<T>> ISeqVisitor.ReplayCountTimeScheduled<T>(
        ReplayCountTimeScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            ReplayCountTimeScheduledImpl<T>,
            seq.Source,
            seq.BufferSize,
            seq.Window,
            Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelector<T, TResult>(
        ReplaySelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorImpl<T, TResult>,
            seq.Source,
            seq.Selector);

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorScheduled<T, TResult>(
        ReplaySelectorScheduledSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorScheduledImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorCount<T, TResult>(
        ReplaySelectorCountSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorCountImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.BufferSize);

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorCountScheduled<T, TResult>(
        ReplaySelectorCountScheduledSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorCountScheduledImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.BufferSize,
            Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorTime<T, TResult>(
        ReplaySelectorTimeSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorTimeImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.Window);

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorTimeScheduled<T, TResult>(
        ReplaySelectorTimeScheduledSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorTimeScheduledImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.Window,
            Unwrap(seq.Scheduler));

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorCountTime<T, TResult>(
        ReplaySelectorCountTimeSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorCountTimeImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.BufferSize,
            seq.Window);

    Realized<Seq<TResult>> ISeqVisitor.ReplaySelectorCountTimeScheduled<T, TResult>(
        ReplaySelectorCountTimeScheduledSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            ReplaySelectorCountTimeScheduledImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.BufferSize,
            seq.Window,
            Unwrap(seq.Scheduler));

    private static IConnectableObservable<T> ReplayImpl<T>(IObservable<T> source) =>
        source.Replay();

    private static IConnectableObservable<T> ReplayScheduledImpl<T>(
        IObservable<T> source,
        IScheduler scheduler) =>
        source.Replay(scheduler);

    private static IConnectableObservable<T> ReplayCountImpl<T>(
        IObservable<T> source,
        int bufferSize) =>
        source.Replay(bufferSize);

    private static IConnectableObservable<T> ReplayCountScheduledImpl<T>(
        IObservable<T> source,
        int bufferSize,
        IScheduler scheduler) =>
        source.Replay(bufferSize, scheduler);

    private static IConnectableObservable<T> ReplayTimeImpl<T>(
        IObservable<T> source,
        TimeSpan window) =>
        source.Replay(window);

    private static IConnectableObservable<T> ReplayTimeScheduledImpl<T>(
        IObservable<T> source,
        TimeSpan window,
        IScheduler scheduler) =>
        source.Replay(window, scheduler);

    private static IConnectableObservable<T> ReplayCountTimeImpl<T>(
        IObservable<T> source,
        int bufferSize,
        TimeSpan window) =>
        source.Replay(bufferSize, window);

    private static IConnectableObservable<T> ReplayCountTimeScheduledImpl<T>(
        IObservable<T> source,
        int bufferSize,
        TimeSpan window,
        IScheduler scheduler) =>
        source.Replay(bufferSize, window, scheduler);

    private static IObservable<TResult> ReplaySelectorImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector) =>
        source.Replay(selector);

    private static IObservable<TResult> ReplaySelectorScheduledImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        IScheduler scheduler) =>
        source.Replay(selector, scheduler);

    private static IObservable<TResult> ReplaySelectorCountImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        int bufferSize) =>
        source.Replay(selector, bufferSize);

    private static IObservable<TResult> ReplaySelectorCountScheduledImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        int bufferSize,
        IScheduler scheduler) =>
        source.Replay(selector, bufferSize, scheduler);

    private static IObservable<TResult> ReplaySelectorTimeImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        TimeSpan window) =>
        source.Replay(selector, window);

    private static IObservable<TResult> ReplaySelectorTimeScheduledImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        TimeSpan window,
        IScheduler scheduler) =>
        source.Replay(selector, window, scheduler);

    private static IObservable<TResult> ReplaySelectorCountTimeImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        int bufferSize,
        TimeSpan window) =>
        source.Replay(selector, bufferSize, window);

    private static IObservable<TResult> ReplaySelectorCountTimeScheduledImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        int bufferSize,
        TimeSpan window,
        IScheduler scheduler) =>
        source.Replay(selector, bufferSize, window, scheduler);
}
