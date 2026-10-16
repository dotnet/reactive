// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IConnectableAsyncObservable<T> ReplayImpl<T>(IAsyncObservable<T> source) =>
        source.Replay();

    private static IConnectableAsyncObservable<T> ReplayScheduledImpl<T>(
        IAsyncObservable<T> source,
        IAsyncScheduler scheduler) =>
        source.Replay(scheduler);

    private static IConnectableAsyncObservable<T> ReplayCountImpl<T>(
        IAsyncObservable<T> source,
        int bufferSize) =>
        source.Replay(bufferSize);

    private static IConnectableAsyncObservable<T> ReplayCountScheduledImpl<T>(
        IAsyncObservable<T> source,
        int bufferSize,
        IAsyncScheduler scheduler) =>
        source.Replay(bufferSize, scheduler);

    private static IConnectableAsyncObservable<T> ReplayTimeImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan window) =>
        source.Replay(window);

    private static IConnectableAsyncObservable<T> ReplayTimeScheduledImpl<T>(
        IAsyncObservable<T> source,
        TimeSpan window,
        IAsyncScheduler scheduler) =>
        source.Replay(window, scheduler);

    private static IConnectableAsyncObservable<T> ReplayCountTimeImpl<T>(
        IAsyncObservable<T> source,
        int bufferSize,
        TimeSpan window) =>
        source.Replay(bufferSize, window);

    private static IConnectableAsyncObservable<T> ReplayCountTimeScheduledImpl<T>(
        IAsyncObservable<T> source,
        int bufferSize,
        TimeSpan window,
        IAsyncScheduler scheduler) =>
        source.Replay(bufferSize, window, scheduler);

    private static IAsyncObservable<TResult> ReplaySelectorImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector) =>
        source.Replay(selector);

    private static IAsyncObservable<TResult> ReplaySelectorScheduledImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        IAsyncScheduler scheduler) =>
        source.Replay(selector, scheduler);

    private static IAsyncObservable<TResult> ReplaySelectorCountImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        int bufferSize) =>
        source.Replay(selector, bufferSize);

    private static IAsyncObservable<TResult> ReplaySelectorCountScheduledImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        int bufferSize,
        IAsyncScheduler scheduler) =>
        source.Replay(selector, bufferSize, scheduler);

    private static IAsyncObservable<TResult> ReplaySelectorTimeImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        TimeSpan window) =>
        source.Replay(selector, window);

    private static IAsyncObservable<TResult> ReplaySelectorTimeScheduledImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        TimeSpan window,
        IAsyncScheduler scheduler) =>
        source.Replay(selector, window, scheduler);

    private static IAsyncObservable<TResult> ReplaySelectorCountTimeImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        int bufferSize,
        TimeSpan window) =>
        source.Replay(selector, bufferSize, window);

    private static IAsyncObservable<TResult> ReplaySelectorCountTimeScheduledImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        int bufferSize,
        TimeSpan window,
        IAsyncScheduler scheduler) =>
        source.Replay(selector, bufferSize, window, scheduler);
}
