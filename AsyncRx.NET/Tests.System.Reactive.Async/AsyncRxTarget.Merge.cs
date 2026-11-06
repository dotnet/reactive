// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Merge<T>(MergeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.MergeMaxConcurrent<T>(MergeMaxConcurrentSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeMaxConcurrentImpl<T>, seq.Sources, seq.MaxConcurrent);

    Realized<Seq<T>> ISeqVisitor.MergeTasks<T>(MergeTasksSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeTasksImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.MergeBinary<T>(MergeBinarySeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeBinaryImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.MergeBinaryScheduled<T>(MergeBinaryScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeBinaryScheduledImpl<T>, seq.First, seq.Second, Unwrap(seq.Scheduler));

    // The array is the one argument, not the argument list.
    Realized<Seq<T>> ISeqVisitor.MergeParams<T>(MergeParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.MergeParamsScheduled<T>(MergeParamsScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeParamsScheduledImpl<T>, Unwrap(seq.Scheduler), seq.Sources);

    Realized<Seq<T>> ISeqVisitor.MergeEnumerable<T>(MergeEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeEnumerableImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.MergeEnumerableScheduled<T>(MergeEnumerableScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeEnumerableScheduledImpl<T>, seq.Sources, Unwrap(seq.Scheduler));

    Realized<Seq<T>> ISeqVisitor.MergeEnumerableMaxConcurrent<T>(
        MergeEnumerableMaxConcurrentSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MergeEnumerableMaxConcurrentImpl<T>, seq.Sources, seq.MaxConcurrent);

    Realized<Seq<T>> ISeqVisitor.MergeEnumerableMaxConcurrentScheduled<T>(
        MergeEnumerableMaxConcurrentScheduledSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            MergeEnumerableMaxConcurrentScheduledImpl<T>,
            seq.Sources,
            seq.MaxConcurrent,
            Unwrap(seq.Scheduler));

    private static IAsyncObservable<T> MergeImpl<T>(IAsyncObservable<IAsyncObservable<T>> sources) =>
        sources.Merge();

    private static IAsyncObservable<T> MergeMaxConcurrentImpl<T>(IAsyncObservable<IAsyncObservable<T>> sources, int maxConcurrent) =>
        sources.Merge(maxConcurrent);

    private static IAsyncObservable<T> MergeTasksImpl<T>(IAsyncObservable<Task<T>> sources) =>
        sources.Merge();

    private static IAsyncObservable<T> MergeBinaryImpl<T>(IAsyncObservable<T> first, IAsyncObservable<T> second) =>
        first.Merge(second);

    private static IAsyncObservable<T> MergeBinaryScheduledImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second,
        IAsyncScheduler scheduler) =>
        first.Merge(second, scheduler);

    private static IAsyncObservable<T> MergeParamsImpl<T>(IAsyncObservable<T>[] sources) =>
        AsyncObservable.Merge(sources);

    private static IAsyncObservable<T> MergeParamsScheduledImpl<T>(IAsyncScheduler scheduler, IAsyncObservable<T>[] sources) =>
        AsyncObservable.Merge(scheduler, sources);

    private static IAsyncObservable<T> MergeEnumerableImpl<T>(IEnumerable<IAsyncObservable<T>> sources) =>
        sources.Merge();

    private static IAsyncObservable<T> MergeEnumerableScheduledImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources,
        IAsyncScheduler scheduler) =>
        sources.Merge(scheduler);

    private static IAsyncObservable<T> MergeEnumerableMaxConcurrentImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources,
        int maxConcurrent) =>
        sources.Merge(maxConcurrent);

    private static IAsyncObservable<T> MergeEnumerableMaxConcurrentScheduledImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources,
        int maxConcurrent,
        IAsyncScheduler scheduler) =>
        sources.Merge(maxConcurrent, scheduler);
}
