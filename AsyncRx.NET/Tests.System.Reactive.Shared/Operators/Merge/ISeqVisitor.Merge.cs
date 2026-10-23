// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MergeSeq{T}"/> as the target's own <c>Merge()</c> over a
    /// nested sequence.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> Merge<T>(MergeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeMaxConcurrentSeq{T}"/> as the target's own
    /// <c>Merge(maxConcurrent)</c> over a nested sequence.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{Seq{T}}, int)"/>.</remarks>
    Realized<Seq<T>> MergeMaxConcurrent<T>(MergeMaxConcurrentSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeTasksSeq{T}"/> as the target's own <c>Merge()</c> over a
    /// sequence of tasks.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{Task{T}})"/>.</remarks>
    Realized<Seq<T>> MergeTasks<T>(MergeTasksSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeBinarySeq{T}"/> as the target's own <c>Merge(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> MergeBinary<T>(MergeBinarySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeBinaryScheduledSeq{T}"/> as the target's own
    /// <c>Merge(second, scheduler)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{T}, Seq{T}, SchedulerRef)"/>.</remarks>
    Realized<Seq<T>> MergeBinaryScheduled<T>(MergeBinaryScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeParamsSeq{T}"/> as the target's own
    /// <c>Merge(params sources)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> MergeParams<T>(MergeParamsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeParamsScheduledSeq{T}"/> as the target's own
    /// <c>Merge(scheduler, params sources)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(SchedulerRef, Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> MergeParamsScheduled<T>(MergeParamsScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeEnumerableSeq{T}"/> as the target's own <c>Merge()</c>
    /// over an enumerable of sequences.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> MergeEnumerable<T>(MergeEnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeEnumerableScheduledSeq{T}"/> as the target's own
    /// <c>Merge(scheduler)</c> over an enumerable of sequences.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, SchedulerRef)"/>.</remarks>
    Realized<Seq<T>> MergeEnumerableScheduled<T>(MergeEnumerableScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeEnumerableMaxConcurrentSeq{T}"/> as the target's own
    /// <c>Merge(maxConcurrent)</c> over an enumerable of sequences.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, int)"/>.</remarks>
    Realized<Seq<T>> MergeEnumerableMaxConcurrent<T>(MergeEnumerableMaxConcurrentSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MergeEnumerableMaxConcurrentScheduledSeq{T}"/> as the target's
    /// own <c>Merge(maxConcurrent, scheduler)</c> over an enumerable of sequences.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Merge{T}(IEnumerable{Seq{T}}, int, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> MergeEnumerableMaxConcurrentScheduled<T>(
        MergeEnumerableMaxConcurrentScheduledSeq<T> seq);
}
