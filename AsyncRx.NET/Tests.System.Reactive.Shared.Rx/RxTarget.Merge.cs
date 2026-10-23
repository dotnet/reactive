// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<T> MergeImpl<T>(IObservable<IObservable<T>> sources) =>
        sources.Merge();

    private static IObservable<T> MergeMaxConcurrentImpl<T>(IObservable<IObservable<T>> sources, int maxConcurrent) =>
        sources.Merge(maxConcurrent);

    private static IObservable<T> MergeTasksImpl<T>(IObservable<Task<T>> sources) =>
        sources.Merge();

    private static IObservable<T> MergeBinaryImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.Merge(second);

    private static IObservable<T> MergeBinaryScheduledImpl<T>(
        IObservable<T> first,
        IObservable<T> second,
        IScheduler scheduler) =>
        first.Merge(second, scheduler);

    private static IObservable<T> MergeParamsImpl<T>(IObservable<T>[] sources) =>
        Observable.Merge(sources);

    private static IObservable<T> MergeParamsScheduledImpl<T>(IScheduler scheduler, IObservable<T>[] sources) =>
        Observable.Merge(scheduler, sources);

    private static IObservable<T> MergeEnumerableImpl<T>(IEnumerable<IObservable<T>> sources) =>
        sources.Merge();

    private static IObservable<T> MergeEnumerableScheduledImpl<T>(
        IEnumerable<IObservable<T>> sources,
        IScheduler scheduler) =>
        sources.Merge(scheduler);

    private static IObservable<T> MergeEnumerableMaxConcurrentImpl<T>(
        IEnumerable<IObservable<T>> sources,
        int maxConcurrent) =>
        sources.Merge(maxConcurrent);

    private static IObservable<T> MergeEnumerableMaxConcurrentScheduledImpl<T>(
        IEnumerable<IObservable<T>> sources,
        int maxConcurrent,
        IScheduler scheduler) =>
        sources.Merge(maxConcurrent, scheduler);
}
